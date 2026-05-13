using UnityEngine;
using System;

public class EnemyEntity : MonoBehaviour
{
    [SerializeField] private EnemySO _enemySO;
    private int _currentHealth;
    private PolygonCollider2D _polygonCollider2D;
    private BoxCollider2D _boxCollider2D;
    private EnemyAIProject _enemyAI;
    public event EventHandler OnTakingHit;
    public event EventHandler Death;
    private Knockback _knockback;


    private void Awake()
    {
        _polygonCollider2D = GetComponent<PolygonCollider2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _enemyAI = GetComponent<EnemyAIProject>();  
        _knockback = GetComponent<Knockback>();
    }

    private void Start()
    {
        _currentHealth = _enemySO.enemyHealth;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent(out Train player))
        {
            player.TakeDamage(_enemySO.enemyDamageAmount);
        }
    }


    public void TakeDamage(Transform damageSource, int damage)
    {
        _currentHealth -= damage;
        _knockback.GetKnockedBack(damageSource);
        OnTakingHit?.Invoke(this, EventArgs.Empty);
        DetectDeath();
    }

    public void PolygonColliderTurnOff()
    {
        _polygonCollider2D.enabled = false;
    }

    public void PolygonColliderTurnOn()
    {
        _polygonCollider2D.enabled = true;
    }

    

    private void DetectDeath()
    {
        if (_currentHealth <= 0)
        {
            _boxCollider2D.enabled = false;
            _polygonCollider2D.enabled = false;
            _enemyAI.SetDeathState();
            Death.Invoke(this, EventArgs.Empty);
        }
    }

    
}
