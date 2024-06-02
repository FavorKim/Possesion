using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapMarker : MonoBehaviour
{
    PlayerController player;
    private void Awake()
    {
        player = FindAnyObjectByType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = player.transform.eulerAngles;
    }
}
