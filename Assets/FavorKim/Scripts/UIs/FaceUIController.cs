using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class FaceUIController : Singleton<FaceUIController>
{
    //[SerializeField] List<GameObject> monsters = new List<GameObject>();
    
    Dictionary<Monsters, GameObject> facesDict = new Dictionary<Monsters, GameObject>();
    [SerializeField] Monsters[] _monsters;
    [SerializeField] GameObject playerFace;

    Monsters curMon;

    private void Awake()
    {
        _monsters = transform.GetComponentsInChildren<Monsters>();
        foreach (Monsters mons in _monsters)
        {
            facesDict.Add(mons, mons.gameObject);
            mons.gameObject.SetActive(false);
        }

    }

    public void GetPossessedMonster(Monsters dest)
    {
        if(playerFace.activeSelf) playerFace.SetActive(false);
        if (curMon != null) facesDict[curMon].SetActive(false);
        //curMon = dest;
        if (facesDict.ContainsKey(dest))
        {
            facesDict[dest].gameObject.SetActive(true);
            curMon = dest;
        }
        else
        {
            Debug.Log("¾ø¼ö");
        }
        

        ChangeFaceUI();
    }
    void ChangeFaceUI()
    {
        facesDict[curMon].SetActive(true);
    }
    public void ChangeFaceUIToPlayer()
    {
        facesDict[curMon].SetActive(false);
        playerFace.SetActive(true);
    }
}
