using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles : MonoBehaviour
{
    [SerializeField] int damage;

    private void OnParticleTrigger()
    {
        GameManager.Instance.Player.GetDamage(damage);
    }
}
