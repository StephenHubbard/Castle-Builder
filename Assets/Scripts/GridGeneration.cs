using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class GridGeneration : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float cellSize;
    [SerializeField] private Vector3 offset;

    private Grid grid;

    private WorldGeneration worldGeneration;

    private void Awake() {
        worldGeneration = GetComponent<WorldGeneration>();
    }

    private void Start() {
        grid = new Grid (width, height, cellSize, offset);

        worldGeneration.SpawnGroundTiles(grid);
    }
}
