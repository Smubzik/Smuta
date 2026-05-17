using UnityEngine;

public class EnemyRangedEntity : EnemyEntityBase
{
    private EnemyRangedAI _enemyAI;

    protected override void Awake()
    {
        base.Awake();
        _enemyAI = GetComponent<EnemyRangedAI>();
    }

    // Нет OnTriggerStay2D — урон только от пуль

    protected override void OnDeath()
    {
        _enemyAI.SetDeathState();
    }
}