/* Copyright 2021-2025 Lutz Großhennig

Use of this source code is governed by an MIT-style
license that can be found in the LICENSE file or at
https://opensource.org/licenses/MIT.
*/


using System;
using UnityEngine;

[Serializable]
public class Level 
{
    public Vector2Int size;

    MapElement[] level;

    public Level(Vector2Int size)
    {
        this.size = size;
        level = new MapElement[size.x * size.y];       
    }

    public MapElement GetElement(Vector2Int pos)
    {
        if (CheckBounds(pos))
        {
            return level[pos.x + LineOffset(pos)];
        }
        return default;
    }

    public void SetElement(Vector2Int pos, MapElement element)
    {
        if (CheckBounds(pos))
        {
            level[pos.x + LineOffset(pos)] = element;
        }
    }

    bool CheckBounds(Vector2Int pos)
    {
        if (pos.x >= 0 || pos.x < size.x || pos.y >= 0 || pos.y < size.y)
        {
            return true; // Position is inside bounds
        }
        return false;
    }

    int LineOffset(Vector2Int pos)
    {
        return pos.y * size.y;
    }
}
