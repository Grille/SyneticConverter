# This program is free software; you can redistribute it and/or modify
# it under the terms of the GNU General Public License as published by
# the Free Software Foundation; either version 3 of the License, or
# (at your option) any later version.
#
# This program is distributed in the hope that it will be useful, but
# WITHOUT ANY WARRANTY; without even the implied warranty of
# MERCHANTIBILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
# General Public License for more details.
#
# You should have received a copy of the GNU General Public License
# along with this program. If not, see <http://www.gnu.org/licenses/>.

import array
import os
import time
import bpy
import mathutils
from bpy.props import (
    BoolProperty,
    FloatProperty,
    StringProperty,
    EnumProperty,
)
from bpy_extras.io_utils import (
    ImportHelper,
    ExportHelper,
    orientation_helper,
    path_reference_mode,
    axis_conversion,
)

from bpy_extras.io_utils import unpack_list
from bpy_extras.image_utils import load_image
from bpy_extras.wm_utils.progress_report import ProgressReport

bl_info = {
    "name" : "Synetic interchange format",
    "author" : "Grille",
    "description" : "Import-Export sbi, Import sbi models, cars and scenarios",
    "blender" : (3, 0, 0),
    "version" : (0, 0, 1),
    "location" : "File > Import-Export",
    "warning" : "",
    "category" : "Import-Export"
}

@orientation_helper(axis_forward='-Z', axis_up='Y')
class ImportSBI(bpy.types.Operator, ImportHelper):
    """Load a Synetic Interchange File"""
    bl_idname = "import_scene.sbi"
    bl_label = "Import SBI"
    bl_options = {'PRESET', 'UNDO'}

    filename_ext = ".sbi"
    filter_glob: StringProperty(
        default="*.sbi",
        options={'HIDDEN'},
    )

    def execute(self, context):
        # print("Selected: " + context.active_object.name)
        from . import import_sbx

        keywords = self.as_keywords()
        if bpy.data.is_saved and context.preferences.filepaths.use_relative_paths:
            import os
            keywords["relpath"] = os.path.dirname(bpy.data.filepath)

        return import_sbx.load(context, **keywords)

    def draw(self, context):
        pass

@orientation_helper(axis_forward='-Z', axis_up='Y')
class ExportSBI(bpy.types.Operator, ExportHelper):
    """Save a Synetic Interchange File"""

    bl_idname = "export_scene_.sbi"
    bl_label = 'Export SBI'
    bl_options = {'PRESET'}

    filename_ext = ".sbi"
    filter_glob: StringProperty(
        default="*.sbi",
        options={'HIDDEN'},
    )

    # context group
    use_selection: BoolProperty(
        name="Selection Only",
        description="Export selected objects only",
        default=False,
    )

    path_mode: path_reference_mode

    check_extension = True

    def execute(self, context):
        from . import export_sbx

        keywords = []
        return export_sbx.save(context, **keywords)

    def draw(self, context):
        pass


def menu_func_import(self, context):
    self.layout.operator(ImportSBI.bl_idname, text="Synetic (.sbi)")


def menu_func_export(self, context):
    self.layout.operator(ExportSBI.bl_idname, text="Synetic (.sbi)")


classes = (
    ImportSBI,
    ExportSBI,
)


def register():
    for cls in classes:
        bpy.utils.register_class(cls)

    bpy.types.TOPBAR_MT_file_import.append(menu_func_import)
    bpy.types.TOPBAR_MT_file_export.append(menu_func_export)


def unregister():
    bpy.types.TOPBAR_MT_file_import.remove(menu_func_import)
    bpy.types.TOPBAR_MT_file_export.remove(menu_func_export)

    for cls in classes:
        bpy.utils.unregister_class(cls)


if __name__ == "__main__":
    register()
