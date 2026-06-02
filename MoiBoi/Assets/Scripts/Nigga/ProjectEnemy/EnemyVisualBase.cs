using UnityEngine;

public abstract class EnemyVisualBase : MonoBehaviour
{
    protected Animator _animator;
    protected SpriteRenderer _spriteRenderer;

    protected const string ISRUNNING = "IsRunning";
    protected const string ATTACK = "Attack";
    protected const string TAKEHIT = "TakeHit";
    protected const string DEATH = "IsDead";

    protected virtual void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void Update()
    {
        EnemyAIBase ai = GetAI();
        if (ai != null)
        {
            _animator.SetBool(ISRUNNING, ai.IsRunning());
        }
    }

    protected abstract EnemyAIBase GetAI();
    protected abstract EnemyEntityBase GetEntity();

    protected virtual void OnAttack(object sender, System.EventArgs e)
    {
        _animator?.SetTrigger(ATTACK);
    }

    protected virtual void OnTakingHit(object sender, System.EventArgs e)
    {
        _animator?.SetTrigger(TAKEHIT);
    }

    protected virtual void OnDeath(object sender, System.EventArgs e)
    {
        if (_animator != null)
            _animator.SetBool(DEATH, true);
        if (_spriteRenderer != null)
            _spriteRenderer.sortingOrder = -1;
    }

    public virtual void TriggerAttackAnimationTurnOff() => GetEntity().PolygonColliderTurnOff();
    public virtual void TriggerAttackAnimationTurnOn() => GetEntity().PolygonColliderTurnOn();
    public virtual void EnemyDestroy() => Destroy(transform.parent.gameObject);
}