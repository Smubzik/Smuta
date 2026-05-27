using UnityEngine;

public class EnemyLaserEntity : EnemyEntityBase
{
    private EnemyLaserAI _enemyAI;

    protected override void Awake()
    {
        base.Awake();
        _enemyAI = GetComponent<EnemyLaserAI>();
    }

    // У лазерного врага НЕТ OnTriggerStay2D
    // Урон наносится только лазером

    protected override void OnDeath()
    {
        _enemyAI.SetDeathState();  // Говорим AI: я умер
    }
}