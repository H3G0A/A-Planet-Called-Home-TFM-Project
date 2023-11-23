using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NavigableButton : MonoBehaviour, IPointerEnterHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(this.gameObject);
    }
}
