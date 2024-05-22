using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraTracker : MonoBehaviour
{
    [SerializeField] Transform player;

    private void Update()
    {
        transform.rotation = player.rotation;
    }
}
