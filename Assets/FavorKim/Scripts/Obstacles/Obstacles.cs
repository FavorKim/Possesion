using UnityEngine;

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
        other.GetComponent<Obstacles>()?.OnTypeAttacked(type);

        // 데미지를 계산할 필요가 있다면? 데미지 계산까지.
        other.GetComponent<IDamagable>()?.GetDamage(Damage);
    }

    // OnTriggerEnter()
    private void OnTriggerStay(Collider other)
    {
        other.GetComponent<Obstacles>()?.OnTypeAttacked(type);
        other.GetComponent<IDamagable>()?.GetDamage(Damage);
    }

    #endregion Unity Events

    #region Custom Methods
    public virtual void OnTypeAttacked(ITyped.Type attackedType) { }
    #endregion Custom Methods


    /*
    obs to obs
    피격자의 속성은 중요하지 않아. 공격자에 따른 행동.
    심지어 공격자가 어떤 속성인지 정확히 들어맞아야만 기능이 수행.
    그렇다면 Obs는 공격자의 속성을 매개로 받아서 내부적으로 기능을 수행시키면 됨.
    언제? 맞았을 때.



    몬스터와 플레이어는 속성은 있지만 관련한 기능은 없으며, 사실 이름뿐.
    몬스터의 공격은 물체와의 상호작용. 즉, obs to obs에 밀접한 게임의 핵심이기 때문에
    Obs속성은 필요하다
    공격은? -> 데미지 계산은 Obs에서 담당할 필요 없음.
    몬스터가 Obs를 사용하지 않고 자체적으로 공격과 관련한 기능을 수행하려면 무엇이 필요할까
    Obs는 달아주되, 데미지는 기재하지 않는다. 오직 속성만 담는 기능을 하는
    Obs가 아닌 Type이라는 Component를 만들자.
    속성공격이 아닌 일반 공격은 Type을 부착할 필요가 없다.
    
    
    충돌 시에는 무엇을 검사해야할까?
    상호작용을 일으킬지 데미지를 입힐지를 검사해야할 것.
    데미지를 입을 수 있는 녀석들(IDamagable)과 Obstacle 사이를 검사하자.
    if(other.GetComponent<Obstacles>() !=null)
    else if (other.GetComponenet<IDamagable>()!=null)


     */
}
