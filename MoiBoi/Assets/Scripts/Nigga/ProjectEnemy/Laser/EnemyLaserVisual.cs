using UnityEngine;

public class EnemyLaserVisual : EnemyVisualBase
{
    [SerializeField] private EnemyLaserAI _enemyAI;
    [SerializeField] private EnemyLaserEntity _enemyEntity;

    protected override EnemyAIBase GetAI() => _enemyAI;
    protected override EnemyEntityBase GetEntity() => _enemyEntity;

    private void Start()
    {
        // Подписываемся на события
        _enemyAI.OnAttack += OnAttack;           // Когда атака
        _enemyEntity.OnTakingHit += OnTakingHit; // Когда получил урон
        _enemyEntity.Death += OnDeath;           // Когда умер
    }


    private void OnDestroy()
    {
        // Отписываемся (хороший тон)
        if (_enemyAI != null)
            _enemyAI.OnAttack -= OnAttack;
    }
}