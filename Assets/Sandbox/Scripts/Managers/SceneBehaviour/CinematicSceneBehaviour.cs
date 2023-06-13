using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalParameters;

public class CinematicSceneBehaviour : MonoBehaviour
{
    [Header("Mouse Cursor Settings")]
    [SerializeField] CursorLockMode _cursorState = CursorLockMode.Locked;

    [Header("Manage Scenes")]
    [SerializeField] Scenes _nextScene;

    private void Awake()
    {
        Cursor.lockState = _cursorState;
    }

    void Start()
    {
        StartCoroutine(ManageCinematic());
    }

    private IEnumerator ManageCinematic()
    {
        yield return new WaitForSeconds(3);

        SceneLoader.Instance.LoadScene(_nextScene);
    }
}
