using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class Obstacles : MonoBehaviour, ITyped
{

    #region Components
    // 컴포넌트(Components)
    private ParticleSystem ps;
    [SerializeField] private int damage;
    [SerializeField] protected ITyped.Type myType;
    #endregion Components

    #region Get/Set Methods
    // Get/Set 함수들
    public ITyped.Type type { get { return myType; } }

    public int Damage { get { return damage; } }
    #endregion Get/Set Methods

    #region Awake()
    private void Awake()
    {
        // 파티클 시스템(Particle System)을 초기화한다.
        ps = GetComponent<ParticleSystem>();
        if (ps == null) return;

        ParticleSystem.CollisionModule col = ps.collision;
        col.enabled = true;
        col.type = ParticleSystemCollisionType.World;
        col.sendCollisionMessages = true;
    }
    #endregion Awake()

    #region Unity Events
    // OnParticleCollision()
    private void OnParticleCollision(GameObject other)
    {
        // 데미지 계산 x 상호작용만.
        if (other.GetComponent<Obstacles>() != null)
            other.GetComponent<Obstacles>()?.OnTypeAttacked(type);

        // 데미지를 계산할 필요가 있다면? 데미지 계산까지.
        else if (other.transform.root.GetComponent<PlayerController>() != null || transform.root.GetComponent<PlayerController>() != null)
        {
            if (other.transform.root != transform.root)
                other.GetComponentInParent<IDamagable>()?.GetDamage(Damage);
        }
    }

    // OnTriggerEnter()
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Obstacles>() != null)
            other.gameObject.GetComponent<Obstacles>()?.OnTypeAttacked(type);

        else if (other.transform.root.GetComponent<PlayerController>() != null || transform.root.GetComponent<PlayerController>() != null)
        {
            if (other.transform.root != transform.root)
                other.GetComponentInParent<IDamagable>()?.GetDamage(Damage);
        }
    }
    #endregion Unity Events

    #region Custom Methods
    public virtual void OnTypeAttacked(ITyped.Type attackedType) { }
    #endregion Custom Methods

}
