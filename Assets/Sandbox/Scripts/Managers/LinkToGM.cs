using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkToGM : MonoBehaviour
{
    [SerializeField] List<GameObject> ObjectsToLink;

    void Start()
    {
        foreach (GameObject obj in ObjectsToLink)
        {
            GameManager.Instance.LinkObject(obj);
        }
    }
}
