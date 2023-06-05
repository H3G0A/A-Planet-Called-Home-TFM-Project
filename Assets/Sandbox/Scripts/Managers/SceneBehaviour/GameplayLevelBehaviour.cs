using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayLevelBehaviour : MonoBehaviour
{
	[Header("Mouse Cursor Settings")]
	[SerializeField] CursorLockMode _cursorState = CursorLockMode.Locked;

	private void Awake()
    {
		Cursor.lockState = CursorLockMode.Locked;
    }
}
