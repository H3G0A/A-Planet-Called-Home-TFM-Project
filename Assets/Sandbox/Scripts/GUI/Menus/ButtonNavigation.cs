using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonNavigation : MonoBehaviour
{

    private void Awake()
    {
        foreach(Transform child in transform)
        {
            child.gameObject.AddComponent<NavigableButton>();
        }
    }
}
