/* Copyright 2021-2025 Lutz Groﬂhennig

Use of this source code is governed by an MIT-style
license that can be found in the LICENSE file or at
https://opensource.org/licenses/MIT.
*/

using System;
using UnityEngine;

[Serializable]
public class MapElement
{
    public GameObject Prefab;
    public Vector2Int Position;
    public float Elevation;
}
