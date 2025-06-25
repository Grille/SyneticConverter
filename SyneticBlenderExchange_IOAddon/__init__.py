bl_info = {
    "name": "Synetic IO",
    "author": "Grille",
    "version": (1, 0),
    "blender": (3, 0, 0),
    "location": "File > Import",
    "category": "Import-Export",
    "description": "Import custom binary model format",
}

import bpy
from .imoprt_sbe import ImportSbe

def menu_func_import(self, context):
    self.layout.operator(ImportSbe.bl_idname, text="Synetic Model (.sbe)")

def register():
    bpy.utils.register_class(ImportSbe)
    bpy.types.TOPBAR_MT_file_import.append(menu_func_import)

def unregister():
    bpy.utils.unregister_class(ImportSbe)
    bpy.types.TOPBAR_MT_file_import.remove(menu_func_import)