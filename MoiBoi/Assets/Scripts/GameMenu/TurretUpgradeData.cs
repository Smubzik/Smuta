using UnityEngine;

[CreateAssetMenu(fileName = "New Turret Upgrade", menuName = "Upgrades/Turret")]
public class TurretUpgradeData : ScriptableObject
{
    public string upgradeName;
    public string description;
    public int upgradePrice;
    public Sprite upgradeIcon;
    
    public TurretType turretType;
    public UpgradeType upgradeType;
    
    public float damageBonus;
    public float fireRateBonus;
    public int extraProjectiles;
    public float rangeBonus;
}

public enum TurretType
{
    Basic,     // 0
    Laser,     // 1
    RapidFire, // 2
    Sniper,    // 3
    Shotgun    // 4
}

public enum UpgradeType
{
    Damage,
    FireRate,
    MultiShot,
    SwitchTurret,
    Range
}