using UnityEngine;
using System.Collections;

public class EnemyBombAI : EnemyAIBase
{

    private bool _isExploding = false;

    protected override void PerformAttack() { }

    protected override void OnEnterAttackingState()
    {
        base.OnEnterAttackingState();
        if (!_isExploding)
        {
            StartCoroutine(ExplodeCoroutine());
        }
    }

    protected override void ChasingBehaviour()
    {
        if (_isExploding) return;

        float distance = GetDistanceToPlayer();
        if (distance <= _enemy_entity_base._enemySO._stopDistance)
        {
            _navMeshAgent.ResetPath();
        }
        else
        {
            _navMeshAgent.SetDestination(GetTargetPoint());
        }
    }

    private IEnumerator ExplodeCoroutine()
    {
        _isExploding = true;
        _navMeshAgent.ResetPath();

        // Мигаем красным
        StartCoroutine(BlinkRed());

        yield return new WaitForSeconds(_enemy_entity_base._enemySO._explosionDelay);

        // Взрыв
        Explode();
    }

    private void Explode()
    {
        // Эффект взрыва
        if (_enemy_entity_base._enemySO._explosionEffect != null)
        {
            Instantiate(_enemy_entity_base._enemySO._explosionEffect, transform.position, Quaternion.identity);
        }

        // Урон всем в радиусе (ИГРОКУ И ВРАГАМ)
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, _enemy_entity_base._enemySO._explosionRadius);

        foreach (Collider2D hit in hitColliders)
        {
            // Урон игроку
            if (hit.TryGetComponent(out Train player))
            {
                player.TakeDamage(_enemy_entity_base._enemySO.enemyDamageAmount);
                Debug.Log($"Bomb hit player! Damage: {_enemy_entity_base._enemySO.enemyDamageAmount}");
            }

            // Урон другим врагам (ближний бой)
            if (hit.TryGetComponent(out EnemyMeleeEntity enemy))
            {
                enemy.TakeDamage(transform, _enemy_entity_base._enemySO.enemyDamageAmount);
                Debug.Log($"Bomb hit enemy! Damage: {_enemy_entity_base._enemySO.enemyDamageAmount}");
            }

            // Урон другим врагам (стреляющий)
            if (hit.TryGetComponent(out EnemyRangedEntity rangedEnemy))
            {
                rangedEnemy.TakeDamage(transform, _enemy_entity_base._enemySO.enemyDamageAmount);
                Debug.Log($"Bomb hit ranged enemy! Damage: {_enemy_entity_base._enemySO.enemyDamageAmount}");
            }

            if (hit.TryGetComponent(out EnemyLaserEntity laserEnemy))
            {
                laserEnemy.TakeDamage(transform, _enemy_entity_base._enemySO.enemyDamageAmount);
                Debug.Log($"Bomb hit laser enemy! Damage: {_enemy_entity_base._enemySO.enemyDamageAmount}");
            }
        }

        // Уничтожаем бомбу
        Destroy(gameObject);
    }

    private IEnumerator BlinkRed()
    {
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        if (sr == null) yield break;

        float elapsed = 0f;
        while (elapsed < _enemy_entity_base._enemySO._explosionDelay)
        {
            sr.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            sr.color = Color.white;
            yield return new WaitForSeconds(0.1f);
            elapsed += 0.2f;
        }

        sr.color = Color.white;
    }
}