using UnityEngine;
using UnityEngine.AI;
using System;

public abstract class EnemyAIBase : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] protected float _chasingSpeed = 3f;           // Скорость преследования

    [Header("Attack Settings")]
    [SerializeField] protected bool _isAttackingEnemy = true;
    [SerializeField] protected float _attackRange = 6f;            // Дальность атаки
    [SerializeField] protected float _attackRate = 1.5f;           // Частота атаки

    [Header("Ranged Specific (для стреляющих)")]
    [SerializeField] protected float _stopDistance = 4f;           // Дистанция остановки (только для стрелков)

    protected NavMeshAgent _navMeshAgent;
    protected State _currentState;
    protected Collider2D _playerCollider;
    protected float _nextAttackTime;

    protected enum State
    {
        Chasing,    // Преследует поезд
        Attacking,  // Атакует
        Death       // Мёртв
    }

    public event EventHandler OnAttack;

    protected virtual void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;
        _navMeshAgent.speed = _chasingSpeed;
        _currentState = State.Chasing;  // Всегда начинаем с преследования
    }

    protected virtual void Start()
    {
        if (Train.Instance != null)
        {
            _playerCollider = Train.Instance.GetComponent<Collider2D>();
        }
    }

    protected virtual void Update()
    {
        if (_currentState == State.Death) return;

        StateHandler();
        UpdateFacingDirection();
    }

    protected virtual Vector3 GetTargetPoint()
    {
        if (Train.Instance == null) return transform.position;
        if (_playerCollider != null)
            return _playerCollider.ClosestPoint(transform.position);
        return Train.Instance.transform.position;
    }

    protected virtual float GetDistanceToPlayer()
    {
        if (Train.Instance == null) return Mathf.Infinity;
        if (_playerCollider != null)
        {
            Vector3 closestPoint = _playerCollider.ClosestPoint(transform.position);
            return Vector3.Distance(transform.position, closestPoint);
        }
        return Vector3.Distance(transform.position, Train.Instance.transform.position);
    }

    protected virtual void StateHandler()
    {
        float distance = GetDistanceToPlayer();

        // Определяем новое состояние
        State newState = State.Chasing;
        if (_isAttackingEnemy && distance <= _attackRange)
            newState = State.Attacking;

        // Переключаем состояние если нужно
        if (newState != _currentState)
        {
            _currentState = newState;

            if (_currentState == State.Attacking)
            {
                _navMeshAgent.ResetPath();
                OnEnterAttackingState();  // Виртуальный метод для наследников
            }
            else if (_currentState == State.Chasing)
            {
                _navMeshAgent.speed = _chasingSpeed;
                OnExitAttackingState();   // Виртуальный метод для наследников
            }
        }

        // Выполняем действия в текущем состоянии
        switch (_currentState)
        {
            case State.Chasing:
                ChasingBehaviour();
                break;
            case State.Attacking:
                AttackBehaviour();
                break;
        }
    }

    // Виртуальные методы для переопределения в наследниках
    protected virtual void OnEnterAttackingState() { }
    protected virtual void OnExitAttackingState() { }
    protected virtual void ChasingBehaviour() { }

    protected virtual void AttackBehaviour()
    {
        if (Time.time > _nextAttackTime)
        {
            PerformAttack();
            OnAttack?.Invoke(this, EventArgs.Empty);
            _nextAttackTime = Time.time + _attackRate;
        }
    }

    protected abstract void PerformAttack();

    protected virtual void UpdateFacingDirection()
    {
        Vector3 target = GetTargetPoint();
        if (transform.position.x > target.x)
            transform.rotation = Quaternion.Euler(0, -180, 0);
        else
            transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public virtual bool IsRunning()
    {
        return _navMeshAgent.velocity != Vector3.zero;
    }

    public virtual void SetDeathState()
    {
        _navMeshAgent.ResetPath();
        _currentState = State.Death;
    }
}