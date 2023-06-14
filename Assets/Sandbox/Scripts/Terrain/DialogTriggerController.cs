using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalParameters;

public class DialogTriggerController : MonoBehaviour
{
    [SerializeField] List<string> _dialogs;
    [SerializeField] GameObject _dialogController;

    void OnTriggerEnter(Collider other)
    {
        DialogManager _dialogManager = _dialogController.GetComponent<DialogManager>();
        if(other.CompareTag(PLAYER_TAG))
        {
            Debug.Log("Play Dialogs");
            _dialogManager.playDialogs(_dialogs);
            Destroy(gameObject);
        }
    }
}
