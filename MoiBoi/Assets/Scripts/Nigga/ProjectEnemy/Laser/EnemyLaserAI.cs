using System.Collections;
using UnityEngine;

public class EnemyLaserAI : EnemyAIBase
{
    [Header("Visual")]
    [SerializeField] private LineRenderer _laserLine;
    [SerializeField] private Transform _firePoint;

    private float _attackStartTime = -999f;
    private bool _isLaserActive = false;

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

        // Начинаем новую атаку
        if (_attackStartTime < 0)
        {
            _attackStartTime = Time.time;
            StartCoroutine(LaserSequence());
        }
    }

    protected override void OnExitAttackingState()
    {
        base.OnExitAttackingState();

        // Сбрасываем атаку при выходе из состояния
        _attackStartTime = -999f;
        _isLaserActive = false;
        if (_laserLine != null)
            _laserLine.enabled = false;
    }

    protected override void ChasingBehaviour()
    {
        float distance = GetDistanceToPlayer();
        if (distance <= _enemy_entity_base._enemySO._stopDistance)
            _navMeshAgent.ResetPath();
        else
            _navMeshAgent.SetDestination(GetTargetPoint());
    }

    private IEnumerator LaserSequence()
    {
        _navMeshAgent.ResetPath();

        // Фаза зарядки (жёлтый лазер)
        if (_laserLine != null)
        {
            _laserLine.enabled = true;
            _laserLine.startColor = _enemy_entity_base._enemySO._chargeColor;
            _laserLine.endColor = _enemy_entity_base._enemySO._chargeColor;
            UpdateLaserLine();
        }

        InvokeOnAttack();
        yield return new WaitForSeconds(_enemy_entity_base._enemySO._chargeTime);

        // Фаза выстрела (красный лазер + урон)
        _isLaserActive = true;
        if (_laserLine != null)
        {
            _laserLine.startColor = _enemy_entity_base._enemySO._fireColor;
            _laserLine.endColor = _enemy_entity_base._enemySO._fireColor;
        }

        // Наносим урон
        if (Train.Instance != null)
        {
            float distance = Vector3.Distance(transform.position, Train.Instance.transform.position);
            if (distance <= _enemy_entity_base._enemySO._attackRange)
            {
                Train.Instance.TakeDamage(_enemy_entity_base._enemySO.enemyDamageAmount);
                Debug.Log($"LASER HIT! Damage: {_enemy_entity_base._enemySO.enemyDamageAmount}");
            }
        }

        yield return new WaitForSeconds(0.3f);

        // Завершаем атаку
        _isLaserActive = false;
        if (_laserLine != null)
            _laserLine.enabled = false;

        // Сбрасываем таймер, чтобы можно было атаковать снова
        _attackStartTime = -999f;

        // Принудительно выходим из состояния атаки на время перезарядки
        _currentState = State.Chasing;
    }

    private void Update()
    {
        // Обновляем линию лазера во время атаки
        if (_isLaserActive && _laserLine != null && _laserLine.enabled)
        {
            UpdateLaserLine();
        }

        // Вызываем базовый Update для обработки состояний
        if (_currentState != State.Death)
        {
            StateHandler();
            UpdateFacingDirection();
        }
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