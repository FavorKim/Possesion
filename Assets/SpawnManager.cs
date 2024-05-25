using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    [SerializeField]
    private Monsters[] m_Spawn;

    [SerializeField]
    public List<Queue<Monsters>> s_manager = new List<Queue<Monsters>>();

    private void Awake()
    {
        foreach(var i in m_Spawn)
        {
            Queue<Monsters> s1 = new Queue<Monsters>();

            for (int k = 0; k < 10; k++)
            {
                Monsters m1 = Instantiate(i);
                m1.gameObject.SetActive(false);
                s1.Enqueue(m1);
            }
            s_manager.Add(s1);
        }
    }
}
