using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Citizen : MonoBehaviour
{
    private enum State {
        Roaming, 
        GoingBackToStart,
    }

    private State state;
    private Vector3 startingPosition;
    private Vector3 roamPosition;
    private float timeRoaming = 0f;
    
    private Pathfinding pathfinding;

    private void Awake() {
        pathfinding = GetComponent<Pathfinding>();
        state = State.Roaming;
    }

    private void Start() {
        startingPosition = transform.position;
        roamPosition = GetRoamingPosition();
        StartCoroutine(RoamingCo());
    }
   

    private void Update() {
        MovementStateControl();
    }


    private void MovementStateControl() {
        switch (state)
        {
        default:
        case State.Roaming: 

            timeRoaming += Time.deltaTime;

            pathfinding.MoveTo(roamPosition);

            float reachedPositionDistance = 1f;
            if ((Vector3.Distance(transform.position, roamPosition) < reachedPositionDistance) || timeRoaming > 4f) {
                roamPosition = GetRoamingPosition();
            }
            break;

        case State.GoingBackToStart:

            pathfinding.MoveTo(startingPosition);

            reachedPositionDistance = 1f;
            if (Vector3.Distance(transform.position, startingPosition) < reachedPositionDistance) {
                state = State.Roaming;
                StartCoroutine(RoamingCo());

            }
            break;
        }        
    }

    private IEnumerator RoamingCo() {
        while (state == State.Roaming)
        {
            yield return new WaitForSeconds(Random.Range(3f, 4f));
            roamPosition = GetRoamingPosition();
        }
    }

    private Vector3 GetRoamingPosition() {
        return startingPosition + GetRandomDir() * Random.Range(1f, 5f);
    }

    private static Vector3 GetRandomDir() {
        return new Vector3(UnityEngine.Random.Range(-1f, 1f), 0).normalized;
    }

}
