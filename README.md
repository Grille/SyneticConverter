# WIP SyneticConverter
## SyneticLib
A library that allows reading and writing to most of Synetic file formats.
### Supported formats
```
- Scenarios
  - Import: MBWR, WR2, C11, CT1, CT2, CT3, CT4, CT5, FVR
  - Export: #
  - EUtils: BeamNG
 ```
 ```
- Cars
  - Import: #
  - Export: #
 ```
### Planed
- Importer & exporter for all scenario files from MBWR to CT5, maybe also NICE to MBTR depending on file formats.
- Later maybe also support for car models.
- Calculating chunks for terrain meshes.
- An simple ray-racer for backed lights.
- OpenGL(OpenTK) renderer for meshes & scenarios.
## SyneticConverter
An GUI layer on top of SyneticLib that allow viewing & converting Scenarios, and later maybe also cars.

