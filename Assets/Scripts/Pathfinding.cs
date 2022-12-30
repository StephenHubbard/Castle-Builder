using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour {

    [SerializeField] private float moveSpeed = 20f;

    private List<Vector3> pathVectorList;
    private Vector3 moveDir;
    private Vector2 lastMoveDir;
    private int currentPathIndex;
    private bool allowedToMove = true;
    private Rigidbody2D myRb;
    private bool isGrounded = false;

    private void Awake() {
        myRb = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        HandleMovement();
    }

    private void FixedUpdate() {

        myRb.velocity = new Vector2(moveDir.x * moveSpeed, myRb.velocity.y); 

        if (lastMoveDir.x < 0) {
            GetComponent<SpriteRenderer>().flipX = true;
        } else {
            GetComponent<SpriteRenderer>().flipX = false;
        }
    }

    private void OnCollisionStay(Collision other) {
        isGrounded = true;
    }

    private void OnCollisionExit(Collision other) {
        isGrounded = false;
    }


    public void StopMoving() {
        moveDir = Vector3.zero;
    }

    public void MoveTo(Vector3 targetPosition) {
        SetTargetPosition(targetPosition);
    }

    public void SetTargetPosition(Vector3 targetPosition) {
        currentPathIndex = 0;

        pathVectorList = new List<Vector3> { targetPosition };

        if (pathVectorList != null && pathVectorList.Count > 1) {
            pathVectorList.RemoveAt(0);
        }
    }

    public Vector3 GetPosition() {
        return transform.position;
    }

    private void HandleMovement() {
        if (!allowedToMove) { return; }

        if (moveDir.x < 0) {
            lastMoveDir.x = -1;
        } else {
            lastMoveDir.x = 1;
        }

        if (pathVectorList != null && pathVectorList.Count > 0) {
            Vector3 targetPosition = pathVectorList[currentPathIndex];
            float reachedTargetDistance = .05f;
            if (Vector3.Distance(GetPosition(), targetPosition) > reachedTargetDistance) {
                moveDir = (targetPosition - GetPosition()).normalized;
            } else {
                currentPathIndex++;
                if (currentPathIndex >= pathVectorList.Count) {
                    StopMoving();
                }
            }
        } 
    }
}
