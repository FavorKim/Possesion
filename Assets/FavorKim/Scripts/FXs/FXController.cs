using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXController : MonoBehaviour
{
    ParticleSystem pS;
    [SerializeField] float duration;
    [SerializeField] float coolTime;
    float playTime;
    float targetDur;

    void Start()
    {
        targetDur = duration;
        pS = GetComponent<ParticleSystem>();
        pS.Stop();
    }

    void Update()
    {
        playTime += Time.deltaTime;

        if (playTime > duration)
        {
            pS.Stop();
            playTime = 0;
            duration = float.MaxValue;
        }

        if (playTime > coolTime && !pS.isPlaying)
        {
            pS.Play();
            playTime = 0;
            duration = targetDur;
        }
    }
}
