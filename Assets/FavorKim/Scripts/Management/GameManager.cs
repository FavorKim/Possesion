using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    PlayerController player;
    //public PlayerController Player { get { return player; } }



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
            if (instance == null)
            {
                GameObject obj = new GameObject("GameManager");
                obj.AddComponent<GameManager>();
                instance = obj.GetComponent<GameManager>();
            }
        }
    }

    public void GetDamage(Obstacles obs, GameObject dest)
    {
        if (dest.CompareTag("Player"))
            player.GetDamage(obs.Damage);
        else if (dest.GetComponent<Obstacles>() != null)
            SetTypeAttack(obs, dest.GetComponent<Obstacles>());
        else
            return;
    }
    public void GetDamage(int dmg)
    {
        player.GetDamage(dmg);
    }


    public void SetTypeAttack(Obstacles from, Obstacles to)
    {
        if ((int)from.GetObsType() > (int)to.GetObsType())
        {
            // 공격자가 공격대상 속성보다 우세일 경우 실행할 내용
            to.OnTypeAttacked(from.GetObsType());
            //to.gameObject.SetActive(false);
        }
        else
        {
            // 공격자가 공격대상 속성보다 열세일 경우 실행할 내용
            return;
        }
    }


}
