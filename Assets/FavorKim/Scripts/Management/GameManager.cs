using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    PlayerController player;
    [SerializeField] CinemachineFreeLook tpsCam;
    public PlayerController Player { get { return player; } }
    public CinemachineFreeLook TpsCam { get { return tpsCam; } }


    private void Start()
    {
        instance= this;
    }


    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        player = FindAnyObjectByType<PlayerController>();
    }

    public void SetCameraFollow(Transform dest)
    {
        tpsCam.Follow = dest;
    }
    public void SetCameraLookAt(Transform dest)
    {
        tpsCam.LookAt = dest;
    }

   
}
