using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ResetParticleOnEnable : MonoBehaviour
{
    ParticleSystem _particleSystem;
    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _particleSystem.Stop();
    }
    private void OnEnable()
    {
        _particleSystem.Clear();
        _particleSystem.Play();
    }
    private void OnDisable()
    {
        _particleSystem.Stop();
    }
}
