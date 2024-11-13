using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeslaCoil : InteractableObstacles
{
    ParticleSystem onFX;
    public CoilWall wall;
    
    public bool TeslaIsOn() {  return onFX.isPlaying; }

    private void Awake()
    {
        onFX = GetComponentInChildren<ParticleSystem>();
    }

    public override void Interact(ITypeInteractable.Type type)
    {
        if (type == ITypeInteractable.Type.THUNDER)
        {
            onFX.Play();
            wall.CoilWallOpen();
        }
    }
}
