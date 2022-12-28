using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using System;

public class GridGeneration : Singleton<GridGeneration>
{
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float cellSize;
    [SerializeField] private Vector3 offset;

 

    private Grid<GridObject> grid;
    private WorldGeneration worldGeneration;

    protected override void Awake() {
        base.Awake();

        worldGeneration = GetComponent<WorldGeneration>();
    }

    private void Start() {
        grid = new Grid<GridObject>(width, height, cellSize, offset, (Grid<GridObject> g, int x, int y) => new GridObject(g, x, y));

        worldGeneration.SpawnGroundTiles();
    }

    public Grid<GridObject> ReturnGrid() {
        return grid;
    }

    
    public class GridObject {
        private Grid<GridObject> grid;
        private int x;
        private int y;
        private PlacedBuilding placedBuilding;

        public GridObject(Grid<GridObject> grid, int x, int y) {
            this.grid = grid;
            this.x = x;
            this.y = y;
        }

        public void SetPlacedObject(PlacedBuilding placedBuilding) {
            this.placedBuilding = placedBuilding;
            grid.TriggerGridObjectChanged(x, y);
        }

        public PlacedBuilding GetPlacedBuilding() {
            return placedBuilding;
        }

        public bool CanBuild() {
            return placedBuilding == null;
        }

        public void ClearPlacedBuilding() {
            placedBuilding = null;
        }

        public override string ToString()
        {
            return x + ", " + y + "\n" + placedBuilding;
        }

        

    }

    
}
