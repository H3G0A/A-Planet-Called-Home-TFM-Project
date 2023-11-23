using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalParameters;

public class InteractionHUD : MonoBehaviour
{
    [SerializeField] GameObject _promptTxt;
    [SerializeField] GameObject _keyboardPrompt;
    [SerializeField] GameObject _controllerPrompt;

    void Start()
    {
        GameManager.Instance.InteractionHUD_ = this;

        HidePrompt();
    }

    public void SetPromptActive(bool active)
    {
        if (active)
        {
            ShowPrompt();
        }
        else
        {
            HidePrompt();
        }
    }

    private void HidePrompt()
    {
        _promptTxt.SetActive(false);
        _keyboardPrompt.SetActive(false);
        _controllerPrompt.SetActive(false);
    }

    private void ShowPrompt()
    {
        _promptTxt.SetActive(true);

        switch (GameManager.Instance.CurrentControlScheme)
        {
            case KEYBOARD_SCHEME:
                _keyboardPrompt.SetActive(true);
                _controllerPrompt.SetActive(false);
                break;

            case CONTROLLER_SCHEME:
                _controllerPrompt.SetActive(true);
                _keyboardPrompt.SetActive(false);
                break;
        }
    }
}
