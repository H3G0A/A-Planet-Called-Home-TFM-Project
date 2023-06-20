using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogManager : MonoBehaviour
{
    [SerializeField] GameObject _dialogHUD;
    [SerializeField]  TMP_Text _dialogText;
    // Start is called before the first frame update
    void Start()
    {
        _dialogHUD.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playDialogs(List<string> dialogList){
        StartCoroutine(showDialogs(dialogList));
    }

    IEnumerator showDialogs(List<string> dialogList){
        _dialogHUD.SetActive(true);
        foreach(string dialogo in dialogList){
            _dialogText.text = dialogo;
            yield return new WaitForSeconds(5f);
        }
        _dialogHUD.SetActive(false);
        
    }
}
