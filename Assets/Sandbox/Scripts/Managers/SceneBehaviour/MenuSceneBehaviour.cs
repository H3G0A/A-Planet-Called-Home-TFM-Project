using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSceneBehaviour : MonoBehaviour
{
    [Header("Mouse Cursor Settings")]
	[SerializeField] CursorLockMode _cursorState = CursorLockMode.None;

    private void Awake()
    {
		Cursor.lockState = _cursorState;
    }
}
