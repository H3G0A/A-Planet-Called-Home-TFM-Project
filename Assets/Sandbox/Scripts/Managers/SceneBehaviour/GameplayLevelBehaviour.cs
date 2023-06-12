using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalParameters;

public class GameplayLevelBehaviour : MonoBehaviour
{
	[Header("Mouse Cursor Settings")]
	[SerializeField] CursorLockMode _cursorState = CursorLockMode.Locked;
	[SerializeField] Scenes _currentLevel;

	private void Awake()
    {
		Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start()
    {
        GameManager.Instance.CurrentLevel = _currentLevel;
    }
}
