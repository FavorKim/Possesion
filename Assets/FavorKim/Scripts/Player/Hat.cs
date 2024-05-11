using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hat : MonoBehaviour
{
    
    DOTweenAnimation dO;
    [SerializeField]Transform hatParent;
    [SerializeField] Transform shootPos;


    [SerializeField] float speed;
    float time;
    void Awake()
    {
        hatParent = transform.parent;
        dO = GetComponentInChildren<DOTweenAnimation>();
    }

    public void ThrowHat()
    {
        transform.parent = null;
        transform.rotation = Quaternion.LookRotation(shootPos.position, Vector3.up);
        shootPos.parent = null;
        StartCoroutine(CorShoot());
        dO.DORestartAllById("Shoot");
    }
    public void ReturnAnim()
    {
        transform.parent = hatParent;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }
    IEnumerator CorShoot()
    {
        while (true)
        {
            yield return null;
            transform.Translate(Time.deltaTime * speed * transform.forward);
            time += Time.deltaTime;
            if (time > 1)
                break;
        }
        time = 0;
        ReturnAnim();
        StopCoroutine(CorShoot());
    }

}
