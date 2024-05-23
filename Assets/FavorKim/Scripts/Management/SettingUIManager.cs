using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SettingUIManager : MonoBehaviour
{
    [SerializeField] private GameObject setting;
    private static SettingUIManager instance;
    bool isFull = false;


    private void Start()
    {
        if(instance == null)
        {
            instance = FindAnyObjectByType<SettingUIManager>();
            if (instance == null)
            {
                instance = this;
            }
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SetFullScreenMode(Toggle isToggle)
    {
        isFull = isToggle;
        Screen.fullScreen = isFull;
    }

    public void SetResolution(TMP_Dropdown val)
    {
        switch (val.value)
        {
            case 1:
                Screen.SetResolution(1920, 1080, isFull);
                break;

            case 2:
                Screen.SetResolution(1280, 720, isFull);
                break;

        }
    }

    public void SetSound(Slider val)
    {
        AudioListener.volume = val.value;
    }

    public void SetSensitivity(Slider val)
    {
        GameManager.Instance.TpsCam.m_XAxis.m_MaxSpeed = 150 + val.value * 300;
        GameManager.Instance.TpsCam.m_YAxis.m_MaxSpeed = 1 + val.value * 2;
    }

    void OnESC(InputValue inputValue)
    {
        if (inputValue.isPressed)
        {
            if (setting.activeSelf)
                setting.SetActive(false);
            else
                setting.SetActive(true);
        }
    }
}
