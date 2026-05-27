using UnityEngine;

public class EnemyRangedEntity : EnemyEntityBase
{
    private EnemyRangedAI _enemyAI;
    
    [Header("Reward")]
    public int coinReward = 50;

    protected override void Awake()
    {
        base.Awake();
        _enemyAI = GetComponent<EnemyRangedAI>();
    }

    protected override void OnDeath()
    {
        UpgradeManager upgradeManager = FindObjectOfType<UpgradeManager>();
        if (upgradeManager != null)
        {
            upgradeManager.AddCurrency(coinReward);
            Debug.Log($"+{coinReward} монет с врага!");
        }
        
        _enemyAI.SetDeathState();
    }
}