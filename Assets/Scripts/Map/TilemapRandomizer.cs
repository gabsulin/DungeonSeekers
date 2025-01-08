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

        FillTilemapWithRandomTiles();
    }

    void FillTilemapWithRandomTiles()
    {
        BoundsInt bounds = tilemap.cellBounds;

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);

                if (tilemap.HasTile(position))
                {
                    TileBase randomTile = tiles[Random.Range(0, tiles.Length)];
                    tilemap.SetTile(position, randomTile);
                }
            }
        }
    }
}
