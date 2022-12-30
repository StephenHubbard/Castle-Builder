using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using System;

public class BuildManager : Singleton<BuildManager>
{
    [SerializeField] private List<BuildingSO> buildingSOList;
    
    private BuildingSO buildingSO;

    public event EventHandler OnSelectedChanged;
    public event EventHandler OnObjectPlaced;

    protected override void Awake() {
        base.Awake();

        buildingSO = buildingSOList[0];
    }

    private void Update() {
        PlaceBuilding();
        ToggleWhichBuilding();

        // Debugging
        DebugShowGridLocation();
    }

    public Vector3 GetMouseWorldSnappedPosition() {
        Vector3 mousePosition = UtilsClass.GetMouseWorldPosition();
        GridGeneration.Instance.ReturnGrid().GetXY(mousePosition, out int x, out int y);

        if (buildingSO != null) {
            Vector3 placedObjectWorldPosition = GridGeneration.Instance.ReturnGrid().GetWorldPosition(x, y);
            return placedObjectWorldPosition;
        } else {
            return mousePosition;
        }
    }

    public BuildingSO GetBuildingSO() {
        return buildingSO;
    }

    public void ChangeBuildingSO(int value) {
        buildingSO = buildingSOList[value];
        RefreshSelectedObjectType();
    }

    private void DebugShowGridLocation() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            GridGeneration.Instance.ReturnGrid().GetXY(UtilsClass.GetMouseWorldPosition(), out int x, out int y);
            Debug.Log(x + " " + y);
        }
    }

    public void DeselectObjectType() {
        buildingSO = null; RefreshSelectedObjectType();
    }

    public void RefreshSelectedObjectType() {
        OnSelectedChanged?.Invoke(this, EventArgs.Empty);
    }


    private void ToggleWhichBuilding() {
        if (Input.GetKeyDown(KeyCode.Alpha1)) { buildingSO = buildingSOList[0]; RefreshSelectedObjectType(); }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { buildingSO = buildingSOList[1]; RefreshSelectedObjectType(); }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { buildingSO = buildingSOList[2]; RefreshSelectedObjectType(); }
        if (Input.GetKeyDown(KeyCode.Alpha4)) { buildingSO = buildingSOList[3]; RefreshSelectedObjectType(); }
        if (Input.GetKeyDown(KeyCode.Alpha5)) { buildingSO = buildingSOList[4]; RefreshSelectedObjectType(); }
        if (Input.GetKeyDown(KeyCode.Alpha6)) { buildingSO = buildingSOList[5]; RefreshSelectedObjectType(); }

        if (Input.GetMouseButtonDown(1)) { DeselectObjectType(); }
    }

    public bool CanGhostBuildingBePlaced() {

        bool canBuild = true;

        GridGeneration.Instance.ReturnGrid().GetXY(UtilsClass.GetMouseWorldPosition(), out int x, out int y);


        List<Vector2Int> gridPositionList = buildingSO.GetGridPositionList(new Vector2Int(x, y));
        List<Vector2Int> groundedList = buildingSO.GetGroundedList(new Vector2Int(x, y));

        // occupied tiles
        foreach (Vector2Int gridPosition in gridPositionList)
        {
            if (GridGeneration.Instance.ReturnGrid().GetGridObject(gridPosition.x, gridPosition.y) != null) {
                if (!GridGeneration.Instance.ReturnGrid().GetGridObject(gridPosition.x, gridPosition.y).CanBuild()) {
                    canBuild = false;
                    break;
                }
            } else {
                canBuild = false;
                break;
            }
        }

        // check if tiles below are grounded/occupied
        foreach (Vector2Int gridPosition in groundedList)
        {
            if (GridGeneration.Instance.ReturnGrid().GetGridObject(gridPosition.x, gridPosition.y) != null) { 
                if (GridGeneration.Instance.ReturnGrid().GetGridObject(gridPosition.x, gridPosition.y).GetPlacedBuilding() == null) {
                    canBuild = false;
                    break;
                } 
            }
            
        }

        return canBuild;
    }

    private void PlaceBuilding() {
        if (Input.GetMouseButtonDown(0) && !InputManager.Instance.ReturnIsOverUI()) {

            GridGeneration.Instance.ReturnGrid().GetXY(UtilsClass.GetMouseWorldPosition(), out int x, out int y);

            Vector2Int placedObjectOrigin = new Vector2Int(x, y);

            if (GridGeneration.Instance.ReturnGrid().GetGridObject(x, y) == null || buildingSO == null) { return; }
            
            List<Vector2Int> gridPositionList = buildingSO.GetGridPositionList(new Vector2Int(x, y));
            List<Vector2Int> groundedList = buildingSO.GetGroundedList(new Vector2Int(x, y));

            bool canBuild = true;

            // occupied tiles
            foreach (Vector2Int gridPosition in gridPositionList)
            {
                if (GridGeneration.Instance.ReturnGrid().GetGridObject(gridPosition.x, gridPosition.y) != null) {
                    if (!GridGeneration.Instance.ReturnGrid().GetGridObject(gridPosition.x, gridPosition.y).CanBuild()) {
                        canBuild = false;
                        break;
                    }
                } else {
                    UtilsClass.CreateWorldTextPopup("Out of bounds", UtilsClass.GetMouseWorldPosition());
                    canBuild = false;
                    break;
                }
            }

            // check if tiles below are grounded/occupied
            foreach (Vector2Int gridPosition in groundedList)
            {
                if (GridGeneration.Instance.ReturnGrid().GetGridObject(gridPosition.x, gridPosition.y) != null) {
                    if (GridGeneration.Instance.ReturnGrid().GetGridObject(gridPosition.x, gridPosition.y).GetPlacedBuilding() == null) {
                        canBuild = false;
                        UtilsClass.CreateWorldTextPopup("Must be grounded", UtilsClass.GetMouseWorldPosition());
                        break;
                    } 
                }
            }
            
            if (canBuild) {
                Vector3 placedObjectWorldPosition = GridGeneration.Instance.ReturnGrid().GetWorldPosition(x, y) + Vector3.zero * GridGeneration.Instance.ReturnGrid().GetCellSize();

                PlacedBuilding placedObject = PlacedBuilding.Create(placedObjectWorldPosition, placedObjectOrigin, buildingSO);

                foreach (Vector2Int gridPosition in gridPositionList)
                {
                    if (GridGeneration.Instance.ReturnGrid().GetGridObject(gridPosition.x, gridPosition.y) != null) { 
                        GridGeneration.Instance.ReturnGrid().GetGridObject(gridPosition.x, gridPosition.y).SetPlacedObject(placedObject);
                    }

                }
            } else {
                UtilsClass.CreateWorldTextPopup("Can't build here", UtilsClass.GetMouseWorldPosition());
            }
        }

        if (Input.GetMouseButtonDown(1) && Input.GetKey(KeyCode.LeftShift)) {
            var gridObject = GridGeneration.Instance.ReturnGrid().GetGridObject(UtilsClass.GetMouseWorldPosition());

            PlacedBuilding placedBuilding = gridObject.GetPlacedBuilding();

            if (placedBuilding != null) {
                placedBuilding.DestroySelf();

                List<Vector2Int> gridPositionList = placedBuilding.GetGridPositionList();

                foreach (Vector2Int gridPosition in gridPositionList) {
                    GridGeneration.Instance.ReturnGrid().GetGridObject(gridPosition.x, gridPosition.y).ClearPlacedBuilding();
                }
            }
        }
    }
}
