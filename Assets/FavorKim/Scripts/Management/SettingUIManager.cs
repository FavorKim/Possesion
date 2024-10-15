using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SettingUIManager : MonoBehaviour
{
    [SerializeField] private GameObject setting;
    [SerializeField] private CanvasGroup retry;
    private static SettingUIManager instance;
    public static SettingUIManager Instance { get { return instance; } }
    bool isFull = false;

    public CanvasGroup Retry { get { return retry; } }

    private void Awake()
    {
        instance = this;
    }

    public void SetFullScreenMode(bool isToggle)
    {
        isFull = isToggle.isOn;
        Debug.Log(isFull);
        Screen.fullScreen = isFull;
    }

    public void SetResolution(int val)
    {
        switch (val)
        {
            case 0:
                Screen.SetResolution(1920, 1080, isFull);
                break;

            case 1:
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

    public void QuitApplication()
    {
        Application.Quit();
    }

    public void PopUpGameOver()
    {
        retry.gameObject.SetActive(true);
        Time.timeScale = 1f;
        retry.DOFade(1, 7);
    }

    public void OnRetry()
    {
        MySceneManager.Instance.Retry();
    }

    public void OnChangeScene(CanvasGroup popup)
    {
        popup.DOFade(0, 1).OnComplete(() => { popup.gameObject.SetActive(false); popup.alpha = 1; });
    }
}
