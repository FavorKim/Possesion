using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatManager : MonoBehaviour
{
    [SerializeField]GameObject hatPref;
    [SerializeField] Transform hatTransform;
    GameObject hat;
    [SerializeField] GameObject hatImg;
    public GameObject GetHatImg() { return hatImg; }

    private void Awake()
    {
        hat = Instantiate(hatPref);
        hat.SetActive(false);
    }

    public void ShootHat()
    {
        if (hat.activeSelf) return;

        hat.transform.position = hatTransform.position;
        hat.transform.rotation = hatTransform.parent.rotation;
        hatImg.SetActive(false);
        hat.SetActive(true);
    }
}
