using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXController : MonoBehaviour
{
    ParticleSystem pS;
    [SerializeField] float duration;
    [SerializeField] float coolTime;
    AudioSource sfx;
    float playTime;
    float targetDur;


    private void Awake()
    {
        sfx = GetComponent<AudioSource>();
        pS = GetComponent<ParticleSystem>();
        targetDur = duration;
    }
    void Start()
    {
        pS.Stop();
    }

    void Update()
    {
        playTime += Time.deltaTime;

        if (playTime > duration)
        {
            pS.Stop();
            sfx.Stop();
            playTime = 0;
            duration = float.MaxValue;
        }

        if (playTime > coolTime && !pS.isPlaying)
        {
            pS.Play();
            sfx.Play();
            playTime = 0;
            duration = targetDur;
        }
    }
}
