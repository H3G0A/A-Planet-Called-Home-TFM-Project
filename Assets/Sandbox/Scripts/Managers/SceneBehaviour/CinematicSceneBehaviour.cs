using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using static GlobalParameters;

public class CinematicSceneBehaviour : MonoBehaviour
{
    [Header("Mouse Cursor Settings")]
    [SerializeField] CursorLockMode _cursorState = CursorLockMode.Locked;

    [Header("Manage Scenes")]
    [SerializeField] Scenes _nextScene;
    [SerializeField] VideoPlayer _videoPlayer;

    bool _videoEnded = false;

    private void Awake()
    {
        Cursor.lockState = _cursorState;

        _videoPlayer.loopPointReached += ctx => _videoEnded = true;
    }

    void Start()
    {
        StartCoroutine(ManageCinematic());
    }

    private IEnumerator ManageCinematic()
    {
        while (!_videoEnded)
        {
            yield return null;
        }

        yield return new WaitForSeconds(1);

        SceneLoader.Instance.LoadScene(_nextScene);
    }
}
