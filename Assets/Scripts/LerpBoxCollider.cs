using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpBoxCollider : MonoBehaviour
{
    private BoxCollider2D boxCollider2D;

    private void Awake() {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Start() {
        boxCollider2D.offset = new Vector2(.5f, .1f);

        boxCollider2D.size = new Vector2(1f, .2f);

        StartCoroutine(StartColliderLerpUp());
    }

    private void Update() {
        
    }

    private IEnumerator StartColliderLerpUp() {
        float timePassed = 0f;
        float duration = .1f;

        while (timePassed < duration)
        {
            timePassed += Time.deltaTime;
            float linearT = timePassed / duration;

            boxCollider2D.offset = new Vector2(.5f, Mathf.Lerp(.1f, .5f, linearT));  
            boxCollider2D.size = new Vector2(1f, Mathf.Lerp(.2f, 1, linearT));

            yield return null;
        }

    }

}
