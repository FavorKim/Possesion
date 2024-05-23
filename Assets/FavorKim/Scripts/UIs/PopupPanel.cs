using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupPanel : MonoBehaviour
{
    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
    }
}
