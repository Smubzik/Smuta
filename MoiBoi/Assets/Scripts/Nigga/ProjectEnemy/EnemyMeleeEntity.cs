using UnityEngine;

public class EnemyMeleeEntity : EnemyEntityBase
{
    private EnemyMeleeAI _enemyAI;

    protected override void Awake()
    {
        base.Awake();
        _enemyAI = GetComponent<EnemyMeleeAI>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Train player))
        {
            player.TakeDamage(_enemySO.enemyDamageAmount);
        }
    }

    protected override void OnDeath()
    {
        _enemyAI.SetDeathState();
    }
}