using UnityEngine;

public class EnemyRangedAI : EnemyAIBase
{
    [Header("Ranged")]
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private Transform _firePoint;

    private float _nextAttackTime;

    private bool _isAttacking = false;

    protected override void ChasingBehaviour()
    {
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

    protected override void AttackBehaviour()
    {
        // Стреляем только если не атакуем в данный момент
        if (!_isAttacking && Time.time >= _nextAttackTime)
        {
            _isAttacking = true;
            PerformAttack();
            InvokeOnAttack();
            _nextAttackTime = Time.time + _enemy_entity_base._enemySO._attackRate;
            // Сбрасываем флаг после задержки (как перезарядка)
            Invoke(nameof(ResetAttackFlag), 0.5f);
        }
    }

    private void ResetAttackFlag()
    {
        _isAttacking = false;
    }

    protected override void PerformAttack()
    {
        if (_firePoint == null)
        {
            _firePoint = GetComponentInChildren<Transform>().Find("FirePoint");
            if (_firePoint == null) return;
        }

        if (_projectilePrefab == null) return;

        GameObject projectile = Instantiate(_projectilePrefab, _firePoint.position, Quaternion.identity);
        Vector2 direction = (GetTargetPoint() - _firePoint.position).normalized;

        EnemyBullet bullet = projectile.GetComponent<EnemyBullet>();
        if (bullet != null)
        {
            bullet.Initialize(direction, _enemy_entity_base._enemySO._projectileSpeed, _enemy_entity_base._enemySO.enemyDamageAmount);
            Debug.Log($"Ranged enemy shot! Damage: {_enemy_entity_base._enemySO.enemyDamageAmount}");
        }
    }
}