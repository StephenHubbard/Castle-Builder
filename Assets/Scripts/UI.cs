using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class UI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        InputManager.Instance.ToggleIsOverUI(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InputManager.Instance.ToggleIsOverUI(false);
    }
}
