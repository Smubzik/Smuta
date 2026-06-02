using UnityEngine;

public class EnemyLaserEntity : EnemyEntityBase
{
    private EnemyLaserAI _enemyAI;
    [Header("Reward")]
    public int coinReward = 50;

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