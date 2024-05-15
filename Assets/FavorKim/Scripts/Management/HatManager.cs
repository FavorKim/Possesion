using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatManager : MonoBehaviour
{
    [SerializeField]GameObject hatPref;
    [SerializeField] Transform hatTransform;
    GameObject hat;
    [SerializeField] GameObject hatImg;
    [SerializeField] GameObject hitParticlePref;
    //GameObject hitParticle;
    ParticleSystem hitParticle;
    [SerializeField]AudioSource sfx;

    public GameObject GetHatImg() { return hatImg; }


    private void Awake()
    {
        hat = Instantiate(hatPref);
        hat.SetActive(false);
        hitParticle = Instantiate(hitParticlePref).GetComponent<ParticleSystem>();
        sfx = hitParticle.gameObject.GetComponent<AudioSource>();
        hitParticle.Stop();
    }

    public void ShootHat()
    {
        if (hat.activeSelf) return;

        hat.transform.position = hatTransform.position;
        hat.transform.rotation = hatTransform.parent.rotation;
        hatImg.SetActive(false);
        hat.SetActive(true);
    }

    public void SetHitParticle(Vector3 pos)
    {
        hitParticle.transform.position = pos;
        sfx.Play();
        hitParticle.Play();
    }
}
