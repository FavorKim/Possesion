using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles : MonoBehaviour
{
    [SerializeField] int damage;
    ParticleSystem ps;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        ParticleSystem.TriggerModule triggerModule = ps.trigger;

        triggerModule.enabled = true;
        triggerModule.SetCollider(0, FindAnyObjectByType<PlayerController>());

        triggerModule.enter = ParticleSystemOverlapAction.Callback;
    }

    private void OnParticleTrigger()
    {
        GameManager.Instance.Player.GetDamage(damage);
    }
}
