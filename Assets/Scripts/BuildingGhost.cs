using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingGhost : MonoBehaviour {
    [SerializeField] private Color redColor = new Color();
    [SerializeField] private Color greenColor = new Color();

    private Transform visual;
    private BuildingSO buildingSO;

    private SpriteRenderer visualSpriteRenderer;

    private void Awake() {
    }

    private void Start() {
        BuildManager.Instance.OnSelectedChanged += Instance_OnSelectedChanged;
    }

    private void Update() {
        CheckGhostPlacement();
    }

    private void CheckGhostPlacement() {
        if (visual == null) { return; }

        if (BuildManager.Instance.CanGhostBuildingBePlaced()) {
            visualSpriteRenderer.color = new Color (greenColor.r, greenColor.g, greenColor.b, 0.7f);
        } else {
            visualSpriteRenderer.color = new Color (redColor.r, redColor.g, redColor.b, 0.7f);
        }
    }

    private void Instance_OnSelectedChanged(object sender, System.EventArgs e) {
        RefreshVisual();
    }


    private void LateUpdate() {
        Vector3 targetPosition = BuildManager.Instance.GetMouseWorldSnappedPosition();
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 15f);
    }

    private void RefreshVisual() {
        if (visual != null) {
            Destroy(visual.gameObject);
            visual = null;
        }

        BuildingSO buildingSO = BuildManager.Instance.GetBuildingSO();

        if (buildingSO != null) {
            visual = Instantiate(buildingSO.visual, Vector3.zero, Quaternion.identity);
            visual.parent = transform;
            visual.localPosition = Vector3.zero;
            visual.localEulerAngles = Vector3.zero;
            visualSpriteRenderer = visual.GetComponent<SpriteRenderer>();
        }

    }

}
