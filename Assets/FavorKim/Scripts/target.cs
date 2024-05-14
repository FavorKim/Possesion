using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class target : MonoBehaviour
{
    public Transform playertran;
    public Transform camertrans;

    private void Update()
    {
        transform.position = playertran.position + (playertran.position - camertrans.position);
    }
}
