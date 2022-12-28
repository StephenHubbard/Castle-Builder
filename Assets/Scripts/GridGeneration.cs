using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class GridGeneration : MonoBehaviour
{
    [SerializeField] private List<BuildingSO> buildingSOList;
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float cellSize;
    [SerializeField] private Vector3 offset;

    private Grid<GridObject> grid;
    private BuildingSO buildingSO;
    private WorldGeneration worldGeneration;

    private void Awake() {
        worldGeneration = GetComponent<WorldGeneration>();

        buildingSO = buildingSOList[0];
    }

    private void Start() {
        grid = new Grid<GridObject>(width, height, cellSize, offset, (Grid<GridObject> g, int x, int y) => new GridObject(g, x, y, false));

        // worldGeneration.SpawnGroundTiles(grid);
    }

    public class GridObject {
        private Grid<GridObject> grid;
        private int x;
        private int y;
        private bool isOccupied;
        private PlacedBuilding placedBuilding;

        public GridObject(Grid<GridObject> grid, int x, int y, bool isOccupied) {
            this.grid = grid;
            this.x = x;
            this.y = y;
            this.isOccupied = isOccupied;
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

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            grid.GetXY(UtilsClass.GetMouseWorldPosition(), out int x, out int y);

            Vector2Int placedObjectOrigin = new Vector2Int(x, y);

            List<Vector2Int> gridPositionList = buildingSO.GetGridPositionList(new Vector2Int(x, y));

            bool canBuild = true;

            foreach (Vector2Int gridPosition in gridPositionList)
            {
                if (!grid.GetGridObject(gridPosition.x, gridPosition.y).CanBuild()) {
                    canBuild = false;
                    break;
                }
            }
            
            if (canBuild) {
                Vector3 placedObjectWorldPosition = grid.GetWorldPosition(x, y) + Vector3.zero * grid.GetCellSize();

                PlacedBuilding placedObject = PlacedBuilding.Create(placedObjectWorldPosition, placedObjectOrigin, buildingSO);

                foreach (Vector2Int gridPosition in gridPositionList)
                {
                    grid.GetGridObject(gridPosition.x, gridPosition.y).SetPlacedObject(placedObject);
                }
            } else {
                UtilsClass.CreateWorldTextPopup("Can't build here", UtilsClass.GetMouseWorldPosition());
            }
        }

        if (Input.GetMouseButtonDown(1)) {
            GridObject gridObject = grid.GetGridObject(UtilsClass.GetMouseWorldPosition());

            PlacedBuilding placedBuilding = gridObject.GetPlacedBuilding();
            if (placedBuilding != null) {
                placedBuilding.DestroySelf();

                List<Vector2Int> gridPositionList = placedBuilding.GetGridPositionList();

                foreach (Vector2Int gridPosition in gridPositionList) {
                    grid.GetGridObject(gridPosition.x, gridPosition.y).ClearPlacedBuilding();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) { buildingSO = buildingSOList[0]; }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { buildingSO = buildingSOList[1]; }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { buildingSO = buildingSOList[2]; }
        if (Input.GetKeyDown(KeyCode.Alpha4)) { buildingSO = buildingSOList[3]; }
        if (Input.GetKeyDown(KeyCode.Alpha5)) { buildingSO = buildingSOList[4]; }
        if (Input.GetKeyDown(KeyCode.Alpha6)) { buildingSO = buildingSOList[5]; }
    }
}
