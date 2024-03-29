using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using static GlobalParameters;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject _menuCanvas;
    [SerializeField] GameObject _firstSelected;
    [SerializeField] CursorLockMode _cursorState;
    AudioSource _audioSource;

    PlayerControls _input;
    bool _isActive = false;
    CursorLockMode _lastState = CursorLockMode.None;

    private void Awake()
    {
        _input = new PlayerControls();
        _audioSource = GetComponent<AudioSource>();
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
        _input.Menus.Enable();

        // Save current mouse state and unlock it to navigate menu
        _lastState = Cursor.lockState;
        Cursor.lockState = _cursorState;

        // Show pause menu
        _menuCanvas.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_firstSelected);
    }

    private void DisableMenu()
    {
        // Resume game
        _isActive = false;
        GameManager.Instance.ResumeGame();
        _input.Menus.Disable();

        // Return to previous mouse state
        Cursor.lockState = _lastState;

        // Hide pause menu
        _menuCanvas.SetActive(false);
    }

    //////////////////////////////////////////////// MENU BUTTONS //////////////////////////////////
    public void OnContinue()
    {
        _audioSource.Play();

        DisableMenu();
    }

    public void OnMainMenu()
    {
        _audioSource.Play();

        DisableMenu();
        SceneLoader.Instance.LoadScene(Scenes.MainMenu);
    }

    public void OnExit()
    {
        _audioSource.Play();

        GameManager.Instance.ExitGame();
    }
    /////////////////////////////////////////////////////////////////////////////////////////////////

    private void EnableInput()
    {
        // Without PlayerInput component, every action map has to be explicitly enabled and disabled
        _input.Ground.Enable();

        _input.Ground.PauseMenu.performed += ToggleMenu;

        _input.Menus.ResumeGame.performed += ToggleMenu;
    }

    private void DisableInput()
    {
        _input.Ground.PauseMenu.performed -= ToggleMenu;

        _input.Menus.ResumeGame.performed -= ToggleMenu;
    }


}
