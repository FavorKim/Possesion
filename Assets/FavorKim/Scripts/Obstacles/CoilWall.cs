using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoilWall : MonoBehaviour
{

    [SerializeField] TeslaCoil[] teslaCoils;
    Animator openAnim;

    private void Awake()
    {
        openAnim = GetComponent<Animator>();
        foreach(TeslaCoil coil in teslaCoils)
        {
            coil.wall = this;
        }
    }



    public void CoilWallOpen()
    {
        foreach (TeslaCoil coil in teslaCoils)
        {
            if (!coil.TeslaIsOn()) return;
        }
        openAnim.SetTrigger(0);
    }
}
