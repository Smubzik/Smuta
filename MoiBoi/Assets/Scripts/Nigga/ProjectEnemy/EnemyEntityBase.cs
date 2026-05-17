using UnityEngine;
using System;

public abstract class EnemyEntityBase : MonoBehaviour
{
    [SerializeField] protected EnemySO _enemySO;
    protected int _currentHealth;
    protected PolygonCollider2D _polygonCollider2D;
    protected BoxCollider2D _boxCollider2D;
    protected Knockback _knockback;

    public event EventHandler OnTakingHit;
    public event EventHandler Death;

    protected virtual void Awake()
    {
        _polygonCollider2D = GetComponent<PolygonCollider2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _knockback = GetComponent<Knockback>();
    }

    protected virtual void Start()
    {
        _currentHealth = _enemySO.enemyHealth;
    }

    public virtual void TakeDamage(Transform damageSource, int damage)
    {
        _currentHealth -= damage;
        _knockback?.GetKnockedBack(damageSource);
        OnTakingHit?.Invoke(this, EventArgs.Empty);
        DetectDeath();
    }

    protected virtual void DetectDeath()
    {
        if (_currentHealth <= 0)
        {
            _boxCollider2D.enabled = false;
            _polygonCollider2D.enabled = false;
            OnDeath();
            Death?.Invoke(this, EventArgs.Empty);
        }
    }

    protected abstract void OnDeath();

    public void PolygonColliderTurnOff() => _polygonCollider2D.enabled = false;
    public void PolygonColliderTurnOn() => _polygonCollider2D.enabled = true;
}