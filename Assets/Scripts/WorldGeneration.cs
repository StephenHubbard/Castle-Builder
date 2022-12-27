using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGeneration : MonoBehaviour
{
    [SerializeField] private GameObject[] groundTiles;

    public void SpawnGroundTiles(Grid grid) {
        for (int x = 0; x < grid.width; x++)
        {
            for (int y = 0; y < grid.height; y++)
            {
                if (y < grid.height / 2) {
                    Instantiate(groundTiles[0], grid.GetWorldPosition(x, y) + new Vector3(grid.cellSize, grid.cellSize) * 0.5f, transform.rotation);
                }
            }
        }
    }
}
