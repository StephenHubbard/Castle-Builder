using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGeneration : MonoBehaviour
{
    [SerializeField] private BuildingSO[] groundTiles;


    public void SpawnGroundTiles() {
        for (int x = 0; x < GridGeneration.Instance.ReturnGrid().width; x++)
        {
            for (int y = 0; y < GridGeneration.Instance.ReturnGrid().height; y++)
            {
                if (y < GridGeneration.Instance.ReturnGrid().height / 2) {
                    PlacedBuilding placedBuilding = Instantiate(groundTiles[0].prefab, GridGeneration.Instance.ReturnGrid().GetWorldPosition(x, y), transform.rotation).GetComponent<PlacedBuilding>();

                    GridGeneration.Instance.ReturnGrid().GetGridObject(x, y).SetPlacedObject(placedBuilding);

                    Vector2Int placedObjectOrigin = new Vector2Int(x, y);

                    placedBuilding.Setup(groundTiles[0], placedObjectOrigin);
                }
            }
        }
    }
}
