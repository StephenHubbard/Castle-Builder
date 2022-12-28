using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class BuildingSO : ScriptableObject
{
    public string nameString;
    public Transform prefab;
    public Transform visual;
    public int width;
    public int height;

     public List<Vector2Int> GetGridPositionList(Vector2Int offset) {
        List<Vector2Int> gridPositionList = new List<Vector2Int>();

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                gridPositionList.Add(offset + new Vector2Int(x, y));
            }
        }

        return gridPositionList;
    }
}
