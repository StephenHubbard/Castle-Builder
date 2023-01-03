using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ramp : MonoBehaviour
{
    [SerializeField] private GameObject[] rampPrefabs;

    private int width;

    private void Awake() {
    }

    private void Start() {
        this.width = GetComponent<PlacedBuilding>().placedObjectTypeSO.width;

        CheckWhichRampToDisplay();
    }

    private void CheckWhichRampToDisplay() {
        Vector2Int origin = GetComponent<PlacedBuilding>().ReturnOrigin();
        List<Vector2Int> leftRightList = GetRampLeftRightList(origin);

        bool bothSidesOccupied = true;

        if (GridGeneration.Instance.ReturnGrid().GetGridObject(leftRightList[1].x, leftRightList[1].y).GetPlacedBuilding() != null) {
            Destroy(this.transform.GetChild(0).gameObject);
            GameObject newRamp = Instantiate(rampPrefabs[0], transform.position, Quaternion.identity);
            newRamp.transform.parent = this.transform;
        } else {
            bothSidesOccupied = false;
        }

        if (GridGeneration.Instance.ReturnGrid().GetGridObject(leftRightList[0].x, leftRightList[0].y).GetPlacedBuilding() != null) {
            Destroy(this.transform.GetChild(0).gameObject);
            GameObject newRamp = Instantiate(rampPrefabs[1], transform.position, Quaternion.identity);
            newRamp.transform.parent = this.transform;
        } else {
            bothSidesOccupied = false;
        }

        if (bothSidesOccupied) {
            Destroy(this.transform.GetChild(0).gameObject);
            GameObject newRamp = Instantiate(rampPrefabs[2], transform.position, Quaternion.identity);
            newRamp.transform.parent = this.transform;
        }
    }

    public List<Vector2Int> GetRampLeftRightList(Vector2Int origin) {

        List<Vector2Int> leftRightList = new List<Vector2Int>();

        leftRightList.Add(origin + new Vector2Int(1, 0));
        leftRightList.Add(origin + new Vector2Int(-1, 0));

        return leftRightList;
    }
}
