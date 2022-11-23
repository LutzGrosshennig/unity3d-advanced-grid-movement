using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [Header("Size of the sqaure grid")]
    public int gridSize;

    [Header("The current dungeon map")]
    public Texture2D map;

    [Header("Array of the tile to prefab mapings")]
    public TileMapping[] tileToPreFab;

    private const string LevelRootObjectName = "LevelRoot";

    private Dictionary<Color32, TileMapping> lookupTable;
    private void Awake()
    {
        // Convert the mapping array into an hashmap (the Unity Editor does not offer an default inspector for hashmaps)
        // otherwise we could directly use a map in the editor.
        lookupTable = tileToPreFab.ToDictionary(v => (Color32)v.tileColor, v => v);
        GenerateLevel();
    }

    public void GenerateLevel()
    {
        ClearLevel();

        var rootObj = new GameObject(LevelRootObjectName);

        for (ushort y = 0; y < map.height; y++)
        {
            for (ushort x = 0; x < map.width; x++)
            {
                GenerateTile(x, y, rootObj);
            }
        }

        // Lets do the static batching do its magic to reduce drawcalls.
        StaticBatchingUtility.Combine(rootObj);
    }

    public void ClearLevel()
    {
        var levelRootObject = GameObject.Find(LevelRootObjectName);

        if (levelRootObject is null)
        {
            return;
        }

        DestroyImmediate(levelRootObject);

    }

    void GenerateTile(ushort x, ushort y, GameObject rootObj)
    {
        TileMapping tileMapping = GetTileMappingAt(x, y);

        if (tileMapping is not null)
        {
            var position = new Vector3(x * gridSize, 0.0f, y * gridSize);
            var rotation = Quaternion.Euler(tileMapping.orientation);

            if (tileMapping.tileFlags.HasFlag(TileFlags.Start))
            {
                var player = GameObject.FindGameObjectWithTag("Player");
                player.transform.position = position;
                player.transform.rotation = rotation;
            }

            var _ = Instantiate(tileMapping.preFab, position, rotation, rootObj.transform);

        }
        else
        {
            // Unkown mapping or empty cell. Just move along, nothing to see here.
        }
    }

    public TileMapping GetTileMappingAt(ushort x, ushort y)
    {
        Color32 tile = map.GetPixel(x, y);

        if (tile != Color.black)
        {
            TileMapping mapping;
            if (lookupTable.TryGetValue(tile, out mapping))
            {
                return mapping;
            }
            else
            {
                Debug.Log("Missing tile mapping " + tile.ToString() + "! Skipping this tile. ");
            }
        }
        return null;
    }
}
