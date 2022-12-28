using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacedBuilding : MonoBehaviour {

    private BuildingSO placedObjectTypeSO;
    private Vector2Int origin;

    public static PlacedBuilding Create(Vector3 worldPosition, Vector2Int origin, BuildingSO placedObjectTypeSO) {
        Transform placedObjectTransform = Instantiate(placedObjectTypeSO.prefab, worldPosition, Quaternion.identity);

        PlacedBuilding placedObject = placedObjectTransform.GetComponent<PlacedBuilding>();

        placedObject.Setup(placedObjectTypeSO, origin);

        return placedObject;
    }

    public void Setup(BuildingSO placedObjectTypeSO, Vector2Int origin) {
        this.placedObjectTypeSO = placedObjectTypeSO;
        this.origin = origin;
    }

    public List<Vector2Int> GetGridPositionList() {
        return placedObjectTypeSO.GetGridPositionList(origin);
    }

    public void DestroySelf() {
        Destroy(gameObject);
    }

    public override string ToString() {
        return placedObjectTypeSO.nameString;
    }

}
