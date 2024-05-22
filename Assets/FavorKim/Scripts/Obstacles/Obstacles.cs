using UnityEngine;

public class Obstacles : MonoBehaviour, ITyped
{

    /*
        몬스터의 스킬 내지 공격이 타입을 갖고있어서
        이걸로 플랫포머의 장애물 등등을 극복하는 형태
        덤불 장애물은 CUTTER나 FIRE로 극복
        FIRE, CUTTER >> LEAF
        몬스터 타입은 일단 추후에.
    */

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
        GameManager.Instance.GetDamage(this, other);
    }

    // OnTriggerEnter()
    private void OnTriggerEnter(Collider other)
    {
        GameManager.Instance.GetDamage(this, other.gameObject);
    }
    #endregion Unity Events

    #region Custom Methods
    public virtual void OnTypeAttacked(Obstacles attackedType) { }
    #endregion Custom Methods
}
