using UnityEngine;

public class EnemyRangedAI : EnemyAIBase
{

    [Header("Ranged")]
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private Transform _firePoint;

    private bool _isInPosition = false;

    protected override void ChasingBehaviour()
    {
        float distance = GetDistanceToPlayer();

        if (distance <= _enemy_entity_base._enemySO._stopDistance)
        {
            if (!_isInPosition)
            {
                _navMeshAgent.ResetPath();
                _isInPosition = true;
            }
        }
        else
        {
            _isInPosition = false;
            _navMeshAgent.SetDestination(GetTargetPoint());
        }
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
        }
    }
}