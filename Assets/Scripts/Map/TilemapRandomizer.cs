using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapRandomizer : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    public TileBase[] tiles;


    void Start()
    {
        if (tilemap == null)
        {
            Debug.LogError("Tilemap is not assigned in the Inspector.");
            return;
        }

        if (tiles == null || tiles.Length == 0)
        {
            Debug.LogError("Tiles array is empty or not assigned in the Inspector.");
            return;
        }

        FillTilemapWithRandomTiles();
    }

    void FillTilemapWithRandomTiles()
    {
        // Get bounds of the Tilemap
        BoundsInt bounds = tilemap.cellBounds;

        // Loop through each position in the bounds
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);

                // Check if there's already a tile at this position
                if (tilemap.HasTile(position))
                {
                    // Randomly choose a tile from the array
                    TileBase randomTile = tiles[Random.Range(0, tiles.Length)];
                    tilemap.SetTile(position, randomTile);
                }
            }
        }
    }
}
