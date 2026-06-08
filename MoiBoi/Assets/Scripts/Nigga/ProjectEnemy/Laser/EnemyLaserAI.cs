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

        // ===== ФАЗА ЗАРЯДКИ (анимация) =====
        if (_laserLine != null)
        {
            _laserLine.enabled = true;
            _laserLine.startColor = _enemy_entity_base._enemySO._chargeColor;
            _laserLine.endColor = _enemy_entity_base._enemySO._chargeColor;
            UpdateLaserLine();
        }

        // Запускаем анимацию зарядки
        InvokeOnAttack();

        // Ждём длину анимации зарядки (например, 0.8 сек)
        // Если длина анимации другая — поменяй значение
        float chargeAnimationLength = 2.0f;
        yield return new WaitForSeconds(chargeAnimationLength);

        // ===== ФАЗА ВЫСТРЕЛА =====
        _isLaserActive = true;
        if (_laserLine != null)
        {
            _laserLine.startColor = _enemy_entity_base._enemySO._fireColor;
            _laserLine.endColor = _enemy_entity_base._enemySO._fireColor;
        }

        // Наносим урон
        if (Train.Instance != null)
        {
            float distance = GetDistanceToPlayer();
            if (distance <= _enemy_entity_base._enemySO._attackRange)
            {
                Train.Instance.TakeDamage(_enemy_entity_base._enemySO.enemyDamageAmount);
                Debug.Log($"LASER HIT! Damage: {_enemy_entity_base._enemySO.enemyDamageAmount}");
            }
        }

        // Держим красный лазер 0.3 сек
        yield return new WaitForSeconds(0.3f);

        // ===== ЗАВЕРШЕНИЕ =====
        _isLaserActive = false;
        if (_laserLine != null)
            _laserLine.enabled = false;

        // Сбрасываем таймер
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