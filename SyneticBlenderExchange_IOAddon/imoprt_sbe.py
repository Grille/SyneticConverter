import os
import bpy
import struct

from io import BufferedReader
from bpy.types import Operator
from bpy_extras.io_utils import ImportHelper
from bpy.props import StringProperty


def read_string(f: BufferedReader):
    length_bytes = f.read(4)
    if not length_bytes:
        return ""
    (length,) = struct.unpack("<I", length_bytes)
    return f.read(length).decode("utf-8")



class SbeVertex:
    def __init__(self, position, normal, uv0, uv1, blending, light_color, shadow, unknown0):
        self.position = position        # (x, y, z)
        self.normal = normal            # (x, y, z)
        self.uv0 = uv0                  # (x, y)
        self.uv1 = uv1                  # (x, y)
        self.blending = blending        # (x, y, z)
        self.light_color = light_color  # (x, y, z)
        self.shadow = shadow            # float
        self.unknown0 = unknown0        # float


class SbeMaterial:
    name: str
    type: int
    textures: list[str] = []  # store texture paths


class SbeRegion:
    material: str
    offset: int
    length: int


class SbeModel:
    name: str
    regions: list[SbeRegion] = []


class SbeData:
    vertices: list[SbeVertex]
    triangles: list
    materials: list[SbeMaterial] = []
    models: list[SbeModel] = []
    

class ImportSbe(Operator, ImportHelper):
    bl_idname = "import_scene.custom_model"
    bl_label = "Import Custom Model"
    filename_ext = ".sbe"
    filter_glob = StringProperty(default="*.sbe", options={'HIDDEN'})



    def load_data(self, f: BufferedReader) -> SbeData:
        data = SbeData()

        version, tex_count = struct.unpack("<ii", f.read(8))
        vert_count, tri_count, mat_count, model_count = struct.unpack("<iiii", f.read(16))

        print(vert_count)

        data.vertices = []
        for _ in range(vert_count):
            bytes = f.read(72)
            (
                px, py, pz,
                nx, ny, nz,
                uvx, uvy,
                uv1x, uv1y,
                bx, by, bz,
                lx, ly, lz,
                shadow,
                unknown0
            ) = struct.unpack("<3f3f2f2f3f3f1f1f", bytes)

            vertex = SbeVertex(
                position=(px, py, pz),
                normal=(nx, ny, nz),
                uv0=(uvx, uvy),
                uv1=(uv1x, uv1y),
                blending=(bx, by, bz),
                light_color=(lx, ly, lz),
                shadow=shadow,
                unknown0=unknown0
            )
            data.vertices.append(vertex)

        data.triangles = [struct.unpack("<3i", f.read(12)) for _ in range(tri_count)]

        for _ in range(mat_count):
            mat = SbeMaterial()
            mat.name = read_string(f)
            mat.type = struct.unpack("<i", f.read(4))[0]
            mat.textures = [read_string(f) for _ in range(tex_count)]

            data.materials.append(mat)


        for _ in range(model_count):
            model = SbeModel()

            model.name = read_string(f)
            region_count = struct.unpack("<i", f.read(4))[0]

            for _ in range(region_count):
                region = SbeRegion()

                region.material = read_string(f)
                region.offset, region.length = struct.unpack("<ii", f.read(8))

                model.regions.append(region)

            data.models.append(model)

        return data



    def resolve_texture_path(self, texture_relpath):
        base_dir = os.path.dirname(self.filepath)
        full_path = os.path.normpath(os.path.join(base_dir, texture_relpath))
        return full_path



    def load_image_once(self, image_path):

        full_texture_path = self.resolve_texture_path(image_path)


        # Try to find already loaded image by its file path
        for img in bpy.data.images:
            if img.filepath == full_texture_path:
                return img  # Return existing image

        # If not found, load it
        try:
            img = bpy.data.images.load(full_texture_path)
            return img
        except Exception as e:
            print(f"Failed to load image {full_texture_path}: {e}")
            return None



    def create_blender_material(self, mat_data: SbeMaterial):
        # Create new Blender material
        mat = bpy.data.materials.new(name=mat_data.name)
        mat.use_nodes = True
        nodes = mat.node_tree.nodes
        links = mat.node_tree.links

        # Clear default nodes
        for node in nodes:
            nodes.remove(node)

        # Create nodes
        output_node = nodes.new(type='ShaderNodeOutputMaterial')
        output_node.location = (400, 0)

        principled_node = nodes.new(type='ShaderNodeBsdfPrincipled')
        principled_node.location = (0, 0)

        links.new(principled_node.outputs['BSDF'], output_node.inputs['Surface'])

        if mat_data.textures:
            texture0_path = mat_data.textures[0]

            try:
                # Load image
                img = self.load_image_once(texture0_path)

                # Create Image Texture node
                tex_node = nodes.new(type='ShaderNodeTexImage')
                tex_node.image = img
                tex_node.location = (-400, 0)

                # Link texture color to Base Color of Principled BSDF
                links.new(tex_node.outputs['Color'], principled_node.inputs['Base Color'])

            except Exception as e:
                print(f"Failed to load texture {texture0_path}: {e}")

        return mat



    def build_scene(self, context, data):
        for model in data.models:
            model_verts = [v.position for v in data.vertices]
            model_normals = [v.normal for v in data.vertices]
            model_uv0 = [v.uv0 for v in data.vertices]
            model_uv1 = [v.uv1 for v in data.vertices]
            model_blending = [v.blending for v in data.vertices]
            model_light_color = [v.light_color for v in data.vertices]
            model_shadow = [v.shadow for v in data.vertices]

            model_tris = []
            mat_slots = []
            mat_lookup = {}

            for region in model.regions:
                tris = data.triangles[region.offset : region.offset + region.length]

                mat = next((m for m in data.materials if m.name == region.material), None)
                if mat is None:
                    continue

                if region.material not in mat_lookup:
                    mat_index = len(mat_slots)
                    mat_slots.append(mat)
                    mat_lookup[region.material] = mat_index
                else:
                    mat_index = mat_lookup[region.material]

                for tri in tris:
                    model_tris.append((tri, mat_index))

            mesh = bpy.data.meshes.new(name=model.name)
            mesh.from_pydata(model_verts, [], [tri for tri, _ in model_tris])
            mesh.update()

            # Materials
            for mat in mat_slots:
                blender_mat = bpy.data.materials.get(mat.name)
                if blender_mat is None:
                    blender_mat = self.create_blender_material(mat)
                mesh.materials.append(blender_mat)

            # Assign material indices
            for i, (_, mat_index) in enumerate(model_tris):
                mesh.polygons[i].material_index = mat_index

            # Normals
            mesh.normals_split_custom_set_from_vertices(model_normals)
            mesh.update()

            # UV Layers
            uv_layer0 = mesh.uv_layers.new(name="UVMap0")
            uv_layer1 = mesh.uv_layers.new(name="UVMap1")
            for loop in mesh.loops:
                vi = loop.vertex_index
                uv_layer0.data[loop.index].uv = model_uv0[vi]
                uv_layer1.data[loop.index].uv = model_uv1[vi]

            # Create Color Attributes for Blending, LightColor, Shadow
            ca_blending = mesh.color_attributes.new(name="Blending", type='FLOAT_COLOR', domain='POINT')
            ca_lightcolor = mesh.color_attributes.new(name="LightColor", type='FLOAT_COLOR', domain='POINT')
            ca_shadow = mesh.color_attributes.new(name="Shadow", type='FLOAT_COLOR', domain='POINT')

            for i, vi in enumerate(range(len(model_verts))):
                # Blending: RGB from vertex data + alpha 1.0
                r, g, b = model_blending[vi]
                ca_blending.data[i].color = (r, g, b, 1.0)

                # LightColor: RGB + alpha 1.0
                lr, lg, lb = model_light_color[vi]
                ca_lightcolor.data[i].color = (lr, lg, lb, 1.0)

                # Shadow: single float to grayscale color + alpha 1.0
                s = model_shadow[vi]
                ca_shadow.data[i].color = (s, s, s, 1.0)

            # Create object and link
            obj = bpy.data.objects.new(model.name, mesh)
            context.collection.objects.link(obj)
            


    def execute(self, context):
        with open(self.filepath, "rb") as f:
            data = self.load_data(f)

        self.build_scene(context, data)

        return {'FINISHED'}
    