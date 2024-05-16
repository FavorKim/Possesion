using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXManager : Singleton<FXManager>
{
    [SerializeField] GameObject poExitFXPref;
    GameObject poExitFX;
    Dictionary<string, GameObject> FXs = new Dictionary<string, GameObject>();

    void Awake()
    {
        poExitFX = Instantiate(poExitFXPref, transform);
        FXs.Add("PoExit", poExitFX);
        
    }

    public void PlayFX(string name, Vector3 pos)
    {
        poExitFX.transform.position = pos;
        FXs[name].GetComponent<ParticleSystem>().Play();
        FXs[name].GetComponent<AudioSource>().Play();
    }
}
