

using UnityEngine;

public class DamageObstacle : MonoBehaviour
{
    [SerializeField] private int damage;


    private void OnParticleCollision(GameObject other)
    {
        if (other.TryGetComponent(out IDamagable dest))
            dest.GetDamage(damage);
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.TryGetComponent(out IDamagable dest))
        {
            dest.GetDamage(damage);
        }
        else if (other.transform.root.TryGetComponent(out IDamagable destParent))
        {
            destParent.GetDamage(damage);
        }
    }
}
