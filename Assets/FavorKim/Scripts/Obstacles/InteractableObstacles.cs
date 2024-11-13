using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class InteractableObstacles : MonoBehaviour
{

    #region Components
    // 컴포넌트(Components)
    private ParticleSystem ps;
    [SerializeField] protected ITypeInteractable.Type myType;
    #endregion Components

    #region Get/Set Methods
    // Get/Set 함수들
    public ITypeInteractable.Type type { get { return myType; } }

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
        if (other.GetComponent<ParticleSystem>() != null || other.GetComponentInParent<IDamagable>() == null)
            other.GetComponent<InteractableObstacles>()?.Interact(type);
    }

    // OnTriggerEnter()
    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out InteractableObstacles obs))
        {
            obs.Interact(type);
        }
    }
    #endregion Unity Events

    #region Custom Methods
    public virtual void Interact(ITypeInteractable.Type attackedType) { }
    #endregion Custom Methods

}
