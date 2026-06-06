using UnityEngine;

[CreateAssetMenu(fileName = "New Player Upgrade", menuName = "Upgrades/Player")]
public class PlayerUpgradeData : ScriptableObject
{
    public string upgradeName;
    public string description;
    public int upgradePrice;
    public Sprite upgradeIcon;
    
    public PlayerUpgradeType upgradeType;
    public int healthBonus;
    public float speedBonus;
    public float armorBonus;
    
    public int maxPurchases = 3;  
}

public enum PlayerUpgradeType
{
    MaxHealth,
    Speed,
    Armor,
    Regeneration,
    Heal
}