using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    PlayerController player;
    [SerializeField] CinemachineFreeLook tpsCam;
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
                DontDestroyOnLoad(obj);
            }
        }
    }

    //public void GetDamage(Obstacles obs, GameObject dest)
    //{
    //    if (dest.CompareTag("Player") && obs.Damage != 0)
    //        player.GetDamage(obs.Damage);
    //    else if (dest.GetComponent<ITyped>() != null)
    //        SetTypeAttack(obs, dest.GetComponent<ITyped>());
    //}

    //public void GetDamage(int dmg)
    //{
    //    player.GetDamage(dmg);
    //}

    ///// <summary>
    ///// 몬스터가 아닌 구조물이 속성 공격을 받았을 때
    ///// </summary>
    ///// <param name="from">공격자</param>
    ///// <param name="to">피격자</param>
    //public void SetTypeAttack(Obstacles from, ITyped to)
    //{
    //    // 공격자가 공격대상 속성보다 우세일 경우 실행할 내용
    //    to.OnTypeAttacked(from);
    //}

    public void SetCameraFollow(Transform dest)
    {
        tpsCam.Follow = dest;
    }
    public void SetCameraLookAt(Transform dest)
    {
        tpsCam.LookAt = dest;
    }
}
