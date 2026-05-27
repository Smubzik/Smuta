using UnityEngine;

public class EnemyMeleeEntity : EnemyEntityBase
{
    private EnemyMeleeAI _enemyAI;
    
    [Header("Reward")]
    public int coinReward = 50;  // ← сколько монет даёт враг

    protected override void Awake()
    {
        base.Awake();
        _enemyAI = GetComponent<EnemyMeleeAI>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Train player))
        {
            player.TakeDamage(_enemySO.enemyDamageAmount);
        }
    }

    protected override void OnDeath()
    {
        // Добавляем монеты перед смертью
        UpgradeManager upgradeManager = FindFirstObjectByType<UpgradeManager>();
        if (upgradeManager != null)
        {
            upgradeManager.AddCurrency(coinReward);
            Debug.Log($"+{coinReward} монет с врага!");
        }
        
        _enemyAI.SetDeathState();
    }
}