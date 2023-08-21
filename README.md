# WIP SyneticConverter
Its code is still very messy, most things don’t work probably and it will probably eat your computer.
Feel free to experiment with it, but don’t expect a working program yet.

##

## SyneticPipelineTool
Windows application that allows chaining predefined operations together, Program I used to convert the MBWR scenarios to WR2.

## SyneticConverter
An WinForms app on top of SyneticLib that allow viewing & converting scenarios and models.

## SyneticLib
Abstract representation of game data without having to worry too much about format specifics.
End goal is that formats from all games can be loaded and are represented the same.
This part is pretty much still WIP, I would advise against using it yet.

## SyneticLib.LowLevel
Classes for file formats, allows to read synetic formats and edit the fields directly.
This layer reads and writes data “as is” conversion between versions and other work has to be done manually.

## SyneticLib.Graphics
Layer on top of SyneticLib, uses OpenGL4(OpenTK_4.8) for rendering of meshes, scenarios and textures.

### Supported formats

#### Scenarios
| Game | Import | Export |
| --- | --- | --- |
| NICE1 | - | - |
| NICE2 | - | - |
| MBTR | - | - |
| WR1 | Yes | - |
| WR2 | Yes | Partial |
| C11 | Partial | - | // no materials / terrain mesh only
| CT1 | Partial | - |
| CT2 | Partial | - |
| CT3 | Partial | - |
| CT4 | Partial | - |
| CT5 | Partial | - |
| FVR | Partial | - |

#### Models
| Game | Import | Export |
| --- | --- | --- |
| NICE1 | - | - |
| NICE2 | - | - |
| MBTR | - | - |
| WR1 | Yes | Yes |
| WR2 | Partial | Partial | // only simple models
| C11 | Partial | Partial |
| CT1 | Partial | Partial |
| CT2 | Partial | Partial |
| CT3 | Partial | Partial |
| CT4 | Partial | Partial |
| CT5 | Partial | Partial |
| FVR | Partial | Partial |

#### Cars
| Game | Import | Export |
| --- | --- | --- |
| NICE1 | - | - |
| NICE2 | - | - |
| MBTR | - | - |
| WR1 | - | - |
| WR2 | - | - |
| C11 | - | - |
| CT1 | - | - |
| CT2 | - | - |
| CT3 | - | - |
| CT4 | - | - |
| CT5 | - | - |
| FVR | - | - |

#### Database
| Game | Edit |
| --- | --- |
| NICE1 | - |
| NICE2 | - |
| MBTR | - |
| WR1 | - |
| WR2 | - |
| C11 | - |
| CT1 | - |
| CT2 | - |
| CT3 | - |
| CT4 | - |
| CT5 | - |
| FVR | - |

### Planed | Considered
- Importer & Exporter for all scenario files from MBWR to CT5, maybe also NICE to MBTR depending on file formats.
- Seamless interchange format with blender. 
- Utils for content creation: like chunk calculation, and may some other things.
- An simple ray-racer for backed lights.
- Some basic edtiting tools, for editing cars and scenarios.
- Included patches like traffic for MBWR to WR2 conversions.

### Credits
- Krom: for the original tool suite
- Silent: for some help with file formats

