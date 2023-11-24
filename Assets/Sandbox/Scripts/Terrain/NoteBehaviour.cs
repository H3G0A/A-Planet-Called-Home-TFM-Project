using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class NoteBehaviour : MonoBehaviour, IInteractable
{
    [SerializeField] TextAsset _note;

    public void TriggerInteraction()
    {
        GameManager.Instance.ReadNote(_note.text);
    }
}
