/* Copyright 2021-2025 Lutz Groﬂhennig

Use of this source code is governed by an MIT-style
license that can be found in the LICENSE file or at
https://opensource.org/licenses/MIT.
*/


using UnityEngine;
/// <summary>
/// The cardinal compass directions including their heading in degrees.
/// </summary>
public enum Directions
{
    North = 0,
    East = 90,
    South = 180,
    West = 270
}

/// <summary>
/// Some enum extensions to ease the usage of the Directions enum.
/// </summary>
public static class DirectionsExtensions
{
    /// <summary>
    /// Rotates the given directions value 90∞ left resulting in a new value.
    /// Handles wrap-arounds.
    /// </summary>
    /// <param name="direction">The enum value to turn left.</param>
    /// <returns>the new Direction enum value.</returns>
    public static Directions RotateLeft(this Directions direction)
    {
        int newDir = ((int)direction - 90 + 360) % 360;
        return (Directions)newDir;
    }

    /// <summary>
    /// Rotates the given directions value 90∞ right resulting in a new value.
    /// Handles wrap-arounds.
    /// </summary>
    /// <param name="direction">The enum value to turn right.</param>
    /// <returns>the new Direction enum value.</returns>
    public static Directions RotateRight(this Directions direction)
    {
        int newDir = ((int)direction + 90) % 360;
        return (Directions)newDir;
    }

    /// <summary>
    /// Converts the current Directions Enum value into a Unity Vector2Int.
    /// </summary>
    /// <param name="direction">The enum value</param>
    /// <returns>The corresponding Vector2Int represantation of the Directions value.</returns>
    public static Vector2Int ToVector2Int(this Directions direction)
    {
        switch (direction)
        {
            case Directions.North:
                return Vector2Int.up;
            case Directions.East:
                return Vector2Int.right;
            case Directions.South:
                return Vector2Int.down;
            case Directions.West:
                return Vector2Int.left;
            default:
                return Vector2Int.zero;
        }
    }
}
