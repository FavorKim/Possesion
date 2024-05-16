using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance {  get { return instance; } }

    PlayerController player;
    public PlayerController Player { get { return player; } }



    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        player = FindAnyObjectByType<PlayerController>();
    }

    private void Start()
    {
        if (instance == null)
        {
            instance = FindObjectOfType(typeof(GameManager)).GetComponent<GameManager>();
            if(instance == null)
            {
                GameObject obj = new GameObject("GameManager");
                obj.AddComponent<GameManager>();
                instance = obj.GetComponent<GameManager>();
            }
        }
    }

}
