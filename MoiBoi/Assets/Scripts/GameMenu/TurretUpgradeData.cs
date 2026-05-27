using UnityEngine;

// ========== ДОБАВЬТЕ ЭТОТ ENUM ==========
public enum TurretType
{
    Basic,
    RapidFire,
    Sniper,
    Shotgun,
    Laser
}
// ========================================

[CreateAssetMenu(fileName = "New Turret Upgrade", menuName = "Upgrades/Turret")]
public class TurretUpgradeData : ScriptableObject
{
    public string upgradeName;
    public string description;
    public int upgradePrice;
    public Sprite upgradeIcon;
    
    public TurretType turretType;      // ← теперь будет работать
    public UpgradeType upgradeType;
    
    public float damageBonus;
    public float fireRateBonus;
    public int extraProjectiles;
}

public enum UpgradeType
{
    Damage,
    FireRate,
    MultiShot,
    SwitchTurret
}