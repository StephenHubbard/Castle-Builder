using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    private bool isOverUI = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
    }

    public void ToggleIsOverUI(bool value) {
        isOverUI = value;
    }

    public bool ReturnIsOverUI() {
        return isOverUI;
    }
}
