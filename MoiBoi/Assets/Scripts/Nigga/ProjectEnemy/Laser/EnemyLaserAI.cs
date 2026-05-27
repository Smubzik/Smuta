using System.Collections;
using UnityEngine;

public class EnemyLaserAI : EnemyAIBase
{
    [Header("Laser Settings")]
    [SerializeField] private float _chargeTime = 0.8f;
    [SerializeField] private int _laserDamage = 20;

    [Header("Visual")]
    [SerializeField] private LineRenderer _laserLine;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private Color _chargeColor = Color.yellow;
    [SerializeField] private Color _fireColor = Color.red;

    private bool _isAttacking = false;

    protected override void Awake()
    {
        base.Awake();
        if (_laserLine != null)
            _laserLine.enabled = false;
    }

    protected override void PerformAttack() { }

    protected override void OnEnterAttackingState()
    {
        base.OnEnterAttackingState();
        if (!_isAttacking)
            StartCoroutine(LaserAttackCoroutine());
    }

    protected override void OnExitAttackingState()
    {
        base.OnExitAttackingState();
        if (_laserLine != null)
            _laserLine.enabled = false;
        _isAttacking = false;
    }

    protected override void ChasingBehaviour()
    {
        float distance = GetDistanceToPlayer();
        if (distance <= _stopDistance)
            _navMeshAgent.ResetPath();
        else
            _navMeshAgent.SetDestination(GetTargetPoint());
    }

    private IEnumerator LaserAttackCoroutine()
    {
        _isAttacking = true;
        _navMeshAgent.ResetPath();

        // Зарядка
        if (_laserLine != null)
        {
            _laserLine.enabled = true;
            _laserLine.startColor = _chargeColor;
            _laserLine.endColor = _chargeColor;
            UpdateLaserLine();
        }

        InvokeOnAttack();
        yield return new WaitForSeconds(_chargeTime);

        // Выстрел
        if (_laserLine != null)
        {
            _laserLine.startColor = _fireColor;
            _laserLine.endColor = _fireColor;
        }

        // ===== УРОН БЕЗ РЕЙКАСТА =====
        if (Train.Instance != null)
        {
            Train.Instance.TakeDamage(_laserDamage);
            Debug.Log($"LASER DAMAGE: {_laserDamage}");
        }
        // =============================

        yield return new WaitForSeconds(0.2f);

        if (_laserLine != null)
            _laserLine.enabled = false;

        _isAttacking = false;
    }

    private void UpdateLaserLine()
    {
        if (_laserLine == null) return;
        Vector3 startPoint = _firePoint != null ? _firePoint.position : transform.position;
        Vector3 targetPoint = GetTargetPoint();
        _laserLine.SetPosition(0, startPoint);
        _laserLine.SetPosition(1, targetPoint);
    }
}