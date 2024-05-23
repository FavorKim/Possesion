using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class SpawnMonster : MonoBehaviour
{
    [SerializeField]
    SpawnManager spawnManagerInstance;

    private void Awake()
    {
        spawnManagerInstance = SpawnManager.Instance;
    }
    private void OnCollisionEnter(Collision collision)
    {
        int rand = Random.Range(0, spawnManagerInstance.s_manager.Count);
        BaseMonster m1 = spawnManagerInstance.s_manager[rand].Dequeue();
        m1.transform.position = gameObject.transform.position;
        m1.gameObject.SetActive(true);
    }
}
