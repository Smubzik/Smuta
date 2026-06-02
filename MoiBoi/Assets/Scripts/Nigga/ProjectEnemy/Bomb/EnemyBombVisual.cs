using UnityEngine;

public class EnemyBombVisual : EnemyVisualBase
{
    [SerializeField] private EnemyBombAI _enemyAI;
    [SerializeField] private EnemyBombEntity _enemyEntity;

    protected override EnemyAIBase GetAI() => _enemyAI;
    protected override EnemyEntityBase GetEntity() => _enemyEntity;

    private void Start()
    {
        _enemyAI.OnAttack += OnAttack;
        _enemyEntity.OnTakingHit += OnTakingHit;
        _enemyEntity.Death += OnDeath;
    }

    private void OnDestroy()
    {
        if (_enemyAI != null)
            _enemyAI.OnAttack -= OnAttack;
    }
}