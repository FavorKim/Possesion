using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour
{
    private Portal[] portals;

    private void Awake()
    {
        portals = GetComponentsInChildren<Portal>();
    }
}
