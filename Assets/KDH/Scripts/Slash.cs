using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{

    public GameObject slash;
    Rigidbody rb;

    GameObject gb;
    private void Awake()
    {
        rb = slash.GetComponent<Rigidbody>();
    }

    public void StartSlash()
    {
        StartCoroutine(Slashed());
    }
    IEnumerator Slashed()
    {
        slash.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        slash.SetActive(false);
    }
    // Start is called before the first frame update
    
}

