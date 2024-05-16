using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{
    /*
    public GameObject slash;
    Rigidbody rb;

    GameObject gb;
    private void Awake()
    {
        rb = slash.GetComponent<Rigidbody>();
    }

    void Slashed()
    {
        rb.transform.position = gameObject.transform.localPosition;
        
        StartCoroutine(Destroyed());
        //Debug.Log("º£¿´À½");
    }
    IEnumerator Destroyed()
    {
        gb = Instantiate(rb, rb.transform.localPosition, rb.transform.localRotation).GetComponent<GameObject>();
        yield return new WaitForSeconds(0.1f);
        Destroy(gb);
    }
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.Player.GetDamage(10);
        }
    }*/
}
