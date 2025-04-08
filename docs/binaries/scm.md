# Supreme Commander Mesh

Documentation on the Supreme Commander Mesh (SCM) binary file. All multi-byte data is written in little-endian ("Intel") format. The file consists of several sections. Each section is padded to ensure that the next section is 32-byte aligned. Omitted sections are indicated by an offset of 0.

## Sections

There are 5 required and 2 optional sections in an SCM file, each indicated by a leading FOURCC code. A FourCC code is a sequence of four bytes used to uniquely identify data. The sections are:

| FOURCC | Contents                                                       |
| ------ | -------------------------------------------------------------- |
| 'MODL' | Header info                                                    |
| 'NAME' | List of bone name strings                                      |
| 'SKEL' | Array of bone data                                             |
| 'VTXL' | Array of basic vertex data                                     |
| 'TRIS' | Array of triangle indices                                      |
| 'VEXT' | Array of extra vertex data (OPTIONAL SECTION)                  |
| 'INFO' | List of null terminated information strings (OPTIONAL SECTION) |

### `MODL`: File header

All byte offsets that are defined in the header file are relative to the start of the file.

| Field                        | Offset | Size    | Type  |
| ---------------------------- | ------ | ------- | ----- |
| Magic Header (= `MODL`)      | `0`    | 4 bytes | chars |
| Version (= 5)                | `4`    | 4 bytes | int32 |
| Offset for bone data         | `8`    | 4 bytes | int32 |
| Weighted bone count          | `12`   | 4 bytes | int32 |
| Offset for vertex data       | `16`   | 4 bytes | int32 |
| Offset for extra vertex data | `20`   | 4 bytes | int32 |
| Vertex count                 | `24`   | 4 bytes | int32 |
| Offset for index data        | `28`   | 4 bytes | int32 |
| Index count                  | `32`   | 4 bytes | int32 |
| Information offset           | `36`   | 4 bytes | int32 |
| Information size in bytes    | `40`   | 4 bytes | int32 |
| Bone count                   | `44`   | 4 bytes | int32 |

### `NAME`: Bone names

A series of strings that terminate with the null byte (`0`). These bone names should be in the same order of the bone data, but it is not guaranteed.

### `SKEL`: Bone data

A sequence of bone data with the following fields:

| Field                    | Offset | Size     | Type                 |
| ------------------------ | ------ | -------- | -------------------- |
| Inverse rest pose matrix | `0`    | 64 bytes | Matrix4x4            |
| Position                 | `64`   | 12 bytes | Vector 3 (x,y,z)     |
| Rotation                 | `76`   | 16 bytes | Quaternion (w,x,y,z) |
| Offset for bone name     | `92`   | 4 bytes  | int32                |
| Bone parent              | `96`   | 4 bytes  | int32                |
| Reserved 1               | `100`  | 4 bytes  | ???                  |
| Reserved 2               | `104`  | 4 bytes  | ???                  |

What is interesting is that the name of the bone is not referenced directly in the bone data. Instead, the offset of where you can find the name of the bone in the `NAME` section is in the stream.

### `VRTX`: Vertex data

A sequence of vertex data with the following fields:

| Field                 | Offset | Size     | Type             |
| --------------------- | ------ | -------- | ---------------- |
| Position              | `0`    | 12 bytes | Vector 3 (x,y,z) |
| Tangent               | `12`   | 12 bytes | Vector 3 (x,y,z) |
| Normal                | `24`   | 12 bytes | Vector 3 (x,y,z) |
| Binormal              | `36`   | 12 bytes | Vector 3 (x,y,z) |
| Texture coordinates 1 | `44`   | 8 bytes  | Vector 2 (u, v)  |
| Texture coordinates 2 | `52`   | 8 bytes  | Vector 2 (u, v)  |
| Bone 1                | `60`   | 1 byte   | int8             |
| Bone 2                | `61`   | 1 byte   | int8             |
| Bone 3                | `62`   | 1 byte   | int8             |
| Bone 4                | `63`   | 1 byte   | int8             |

### `TRIS`: Triangle data

A sequence of triangulation data with the following fields:

| Field   | Offset | Size    | Type  |
| ------- | ------ | ------- | ----- |
| Index 1 | `0`    | 2 bytes | int16 |
| Index 2 | `2`    | 2 bytes | int16 |
| Index 3 | `4`    | 2 bytes | int16 |

### `INFO`: Other data

A series of strings that terminate with the null byte (`0`). The information in this section can be anything. The information that ships with the mesh files that you can retrieve through Steam is usually a machine name, the directory and the export time.

## References

- Plugins for Blender:
- - [v2.79](https://github.com/Exotic-Retard/SupCom_Import_Export_Blender/tree/Blender2.79)
- - [v3.0](https://github.com/Solstice245/scstudio)
- - [v4.2](https://github.com/Exotic-Retard/SupCom_Import_Export_Blender)
- [Plugin for 3DS Max](https://ftp.zx.net.nz/pub/Game-Files/SupCom/3rd-party/utils/3dsmax/)
- [Original header files](https://ftp.zx.net.nz/pub/Game-Files/SupCom/util/)
