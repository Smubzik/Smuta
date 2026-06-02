using UnityEngine;

public class EnemyBombEntity : EnemyEntityBase
{
    private EnemyBombAI _enemyAI;

    protected override void Awake()
    {
        base.Awake();
        _enemyAI = GetComponent<EnemyBombAI>();
    }

    protected override void OnDeath()
    {
        // Если умирает от пули — тоже взрывается
        _enemyAI.SetDeathState();
        // Можно вызвать взрыв
    }
}