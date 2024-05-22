using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour
{
    [SerializeField]
    private Portal portal1;
    [SerializeField]
    private Portal portal2;


    private void Awake()
    {
        portal1.SetDestination(portal2.gameObject.transform);
        portal2.SetDestination(portal1.gameObject.transform);
    }
}
