using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static GlobalParameters;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject _menuCanvas;

    PlayerControls _input;
    bool _isActive = false;
    CursorLockMode _lastState = CursorLockMode.None;

    private void Awake()
    {
        _input = new PlayerControls();
    }

    private void OnEnable()
    {
        EnableInput();
    }

    private void OnDisable()
    {
        DisableInput();
    }

    private void ToggleMenu(InputAction.CallbackContext ctx)
    {
        if (!_isActive)
        {
            EnableMenu();
        }
        else
        {
            DisableMenu();
        }
    }

    private void EnableMenu()
    {
        // Pause game
        _isActive = true;
        GameManager.Instance.PauseGame();

        // Save current mouse state and unlock it to navigate menu
        _lastState = Cursor.lockState;
        Cursor.lockState = CursorLockMode.None;

        // Show pause menu
        _menuCanvas.SetActive(true);
    }

    private void DisableMenu()
    {
        // Resume game
        _isActive = false;
        GameManager.Instance.ResumeGame();

        // Return to previous mouse state
        Cursor.lockState = _lastState;

        // Hide pause menu
        _menuCanvas.SetActive(false);
    }

    //////////////////////////////////////////////// MENU BUTTONS //////////////////////////////////
    public void OnContinue()
    {
        DisableMenu();
    }

    public void OnMainMenu()
    {
        SceneLoader.Instance.LoadScene(Scenes.MainMenu);
    }

    public void OnExit()
    {
        GameManager.Instance.ExitGame();
    }
    /////////////////////////////////////////////////////////////////////////////////////////////////

    private void EnableInput()
    {
        // Without PlayerInput component, every action map has to be explicitly enabled and disabled
        _input.Menus.Enable();

        _input.Menus.Escape.performed += ToggleMenu;
    }

    private void DisableInput()
    {
        _input.Menus.Disable();

        _input.Menus.Escape.performed -= ToggleMenu;
    }
}
