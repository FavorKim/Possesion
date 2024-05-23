using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    [SerializeField]
    private Monsters[] m_Spawn;
    [SerializeField]
    public List<Queue<BaseMonster>> s_manager = new List<Queue<BaseMonster>>();

    private void Awake()
    {
        foreach(var i in m_Spawn)
        {
            Queue<BaseMonster> s1 = new Queue<BaseMonster>();

            for (int k = 0; k < 10; k++)
            {
                BaseMonster m1 = Instantiate(i) as BaseMonster;
                m1.gameObject.SetActive(false);
                s1.Enqueue(m1);
            }
            s_manager.Add(s1);
        }
    }
}
