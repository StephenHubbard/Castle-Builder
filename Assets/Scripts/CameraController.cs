using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{

    [SerializeField] private float MIN_FOLLOW_Z_OFFSET;
    [SerializeField] private float MAX_FOLLOW_Z_OFFSET;

    [SerializeField] private float moveSpeed = 20f;
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    // [SerializeField] private Collider2D cameraConfiner;
    // [SerializeField] private float cameraBorderBuffer = 7f;
    [SerializeField] private Camera mainCam;
    [SerializeField] private float edgeSize = 30f;
    [SerializeField] public bool edgeScrollingEnabled = true;

    // private CinemachineTransposer cinemachineTransposer;

    // private Vector3 bottomLeftLimit;
    // private Vector3 topRightLimit;

    private void Start()
    {
        // cinemachineTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();

        cinemachineVirtualCamera.m_Lens.OrthographicSize = 13f;

        // bottomLeftLimit = cameraConfiner.bounds.min;
        // topRightLimit = cameraConfiner.bounds.max;
    }

    private void Update()
    {
        HandleMovement();
        HandleZoom();
    }

    private void HandleMovement()
    {
        Vector2 inputMoveDir = GetCameraMoveVector();

        if (edgeScrollingEnabled) {
            if (Input.mousePosition.x > Screen.width - edgeSize) {
                inputMoveDir.x = +1;
            }
            if (Input.mousePosition.x < edgeSize) {
                inputMoveDir.x = -1;
            }
            if (Input.mousePosition.y > Screen.height - edgeSize) {
                inputMoveDir.y = +1;
            }
            if (Input.mousePosition.y < edgeSize) {
                inputMoveDir.y = -1;
            }
        }

        Vector3 moveVector = transform.up * inputMoveDir.y + transform.right * inputMoveDir.x;
        transform.position += moveVector * moveSpeed * Time.deltaTime;

    }

    private void HandleZoom()
    {
        float zoomIncreaseAmount = 1f;
        float currentZoom = cinemachineVirtualCamera.m_Lens.OrthographicSize;
        float targetZoom = currentZoom;
        targetZoom += GetCameraZoomAmount() * zoomIncreaseAmount;
        targetZoom = Mathf.Clamp(targetZoom, MIN_FOLLOW_Z_OFFSET, MAX_FOLLOW_Z_OFFSET);

        // float zoomSpeed = 5f;

        // cameraBorderBuffer = FindObjectOfType<CinemachineVirtualCamera>().m_Lens.OrthographicSize * 1.65f;

        // cinemachineVirtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(currentZoom, targetZoom, Time.deltaTime * zoomSpeed);
        cinemachineVirtualCamera.m_Lens.OrthographicSize = targetZoom;

    }

     public Vector2 GetCameraMoveVector()
    {
        Vector2 inputMoveDir = new Vector2(0, 0);
        
        if (Input.GetKey(KeyCode.W))
        {
            inputMoveDir.y = +1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputMoveDir.y = -1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputMoveDir.x = -1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputMoveDir.x = +1f;
        }

        return inputMoveDir;
    }

    public float GetCameraZoomAmount()
    {
        float zoomAmount = 0f;

        if (Input.mouseScrollDelta.y > 0 && !Input.GetKey(KeyCode.LeftShift))
        {
            zoomAmount = -1f;
        }
        if (Input.mouseScrollDelta.y < 0 && !Input.GetKey(KeyCode.LeftShift))
        {
            zoomAmount = +1f;
        }

        return zoomAmount;
    }

}