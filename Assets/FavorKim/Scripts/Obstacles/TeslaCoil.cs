using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeslaCoil : Obstacles
{
    ParticleSystem onFX;
    public CoilWall wall;
    
    public bool TeslaIsOn() {  return onFX.isPlaying; }

    private void Awake()
    {
        onFX = GetComponentInChildren<ParticleSystem>();
    }

    public override void OnTypeAttacked(Obstacles attackedType)
    {
        if (attackedType.type == ITyped.Type.THUNDER)
        {
            onFX.Play();
            wall.CoilWallOpen();
        }
    }
}