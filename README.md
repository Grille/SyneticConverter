# WIP SyneticConverter

## SyneticConverter
An WinForms app on top of SyneticLib that allow viewing & converting scenarios and models.

## SyneticLib
A library that allows reading and writing to most of Synetic file formats.
Also includes an basic OpenTK renderer for preview.

### Supported formats

#### Scenarios
| Game | Import | Export |
| --- | --- | --- |
| NICE1 | - | - |
| NICE2 | - | - |
| MBTR | - | - |
| WR1 | Yes | - |
| WR2 | Yes | Partial |
| C11 | Partial | Partial |
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
| WR2 | Partial | Yes |
| C11 | Partial | Yes |
| CT1 | Partial | Yes |
| CT2 | Partial | Yes |
| CT3 | Partial | Yes |
| CT4 | Partial | Yes |
| CT5 | Partial | Yes |
| FVR | Partial | Yes |

#### Cars
| Game | Import | Export |
| --- | --- | --- |
| NICE1 | - | - |
| NICE2 | - | - |
| MBTR | - | - |
| WR1 | Partial | Partial |
| WR2 | Partial | - |
| C11 | Partial | - |
| CT1 | Partial | - |
| CT2 | Partial | - |
| CT3 | Partial | - |
| CT4 | Partial | - |
| CT5 | Partial | - |
| FVR | Partial | - |

### Planed
- Importer & Exporter for all scenario files from MBWR to CT5, maybe also NICE to MBTR depending on file formats.
- Utils for content creation: like chunk calculation, and may some other things.
- An simple ray-racer for backed lights.

### Credits
- Krom for the original tool suite
- Silent 

