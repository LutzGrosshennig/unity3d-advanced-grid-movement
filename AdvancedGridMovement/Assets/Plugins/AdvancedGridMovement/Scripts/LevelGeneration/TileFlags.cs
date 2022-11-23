using System;

[Serializable, Flags]
public enum TileFlags 
{
    // Empty tile / off map
    Empty = 0b_0000_0000_0000_0000,

    // Walls
    North = 0b_0000_0000_0000_0001,
    South = 0b_0000_0000_0000_0010,  
    West  = 0b_0000_0000_0000_0100,
    East  = 0b_0000_0000_0000_1000,

    // Player starting tile
    Start = 0b_0000_0000_0001_0000
}
