using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bedroom : MonoBehaviour
{
   [SerializeField] private GameObject citizenPrefab;
   [SerializeField] private Transform citizenSpawnPos;

   private void Start() {
        Instantiate(citizenPrefab, citizenSpawnPos.position, Quaternion.identity);    
   }
}
