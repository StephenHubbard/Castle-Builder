using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bedroom : MonoBehaviour
{
   [SerializeField] private GameObject citizenPrefab;

   private void Start() {
        Instantiate(citizenPrefab, transform.position, Quaternion.identity);    
   }
}
