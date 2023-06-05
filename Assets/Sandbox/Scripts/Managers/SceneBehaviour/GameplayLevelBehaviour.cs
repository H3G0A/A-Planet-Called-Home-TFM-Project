using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayLevelBehaviour : MonoBehaviour
{
	[Header("Mouse Cursor Settings")]
	[SerializeField] bool cursorLocked = true;

    private void Awake()
    {
		Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnApplicationFocus(bool hasFocus)
	{
		SetCursorState(cursorLocked);
	}

	private void SetCursorState(bool newState)
	{
		Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
	}
}
