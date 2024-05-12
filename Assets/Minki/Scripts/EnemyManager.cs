using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private int _healthPoints;

    private void Awake()
    {
        _healthPoints = 30;
    }

    public bool TakeHit()
    {
        _healthPoints -= 10;
        bool isDead = _healthPoints <= 0;
        if (isDead) _Die();
        return isDead;
    }

    private void _Die()
    {
        Destroy(gameObject);
    }
}
