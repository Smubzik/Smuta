using UnityEngine;

public static class GameData
{
    // ===== МОНЕТЫ =====
    public static int Currency
    {
        get => PlayerPrefs.GetInt("Currency", 1000);
        set => PlayerPrefs.SetInt("Currency", value);
    }
    
    // ===== УЛУЧШЕНИЯ ДЛЯ КАЖДОЙ ТУРЕЛИ =====
    public static float GetTurretDamage(TurretType type)
    {
        return PlayerPrefs.GetFloat($"TurretDamage_{type}", GetBaseDamage(type));
    }
    
    public static void SetTurretDamage(TurretType type, float damage)
    {
        PlayerPrefs.SetFloat($"TurretDamage_{type}", damage);
        PlayerPrefs.Save();
    }
    
    public static float GetTurretFireDelay(TurretType type)
    {
        return PlayerPrefs.GetFloat($"TurretFireDelay_{type}", GetBaseFireDelay(type));
    }
    
    public static void SetTurretFireDelay(TurretType type, float delay)
    {
        PlayerPrefs.SetFloat($"TurretFireDelay_{type}", delay);
        PlayerPrefs.Save();
    }
    
    public static int GetTurretExtraProjectiles(TurretType type)
    {
        return PlayerPrefs.GetInt($"TurretExtraProjectiles_{type}", 0);
    }
    
    public static void SetTurretExtraProjectiles(TurretType type, int count)
    {
        PlayerPrefs.SetInt($"TurretExtraProjectiles_{type}", count);
        PlayerPrefs.Save();
    }
    
    public static float GetTurretRange(TurretType type)
    {
        return PlayerPrefs.GetFloat($"TurretRange_{type}", GetBaseRange(type));
    }
    
    public static void SetTurretRange(TurretType type, float range)
    {
        PlayerPrefs.SetFloat($"TurretRange_{type}", range);
        PlayerPrefs.Save();
    }
    
    // ===== БАЗОВЫЕ ЗНАЧЕНИЯ =====
    static float GetBaseDamage(TurretType type)
    {
        switch (type)
        {
            case TurretType.Basic: return 10f;
            case TurretType.Laser: return 15f;
            case TurretType.RapidFire: return 5f;
            case TurretType.Sniper: return 30f;
            case TurretType.Shotgun: return 8f;
            default: return 10f;
        }
    }
    
    static float GetBaseFireDelay(TurretType type)
    {
        switch (type)
        {
            case TurretType.Basic: return 0.5f;
            case TurretType.Laser: return 0.2f;
            case TurretType.RapidFire: return 0.15f;
            case TurretType.Sniper: return 1f;
            case TurretType.Shotgun: return 0.7f;
            default: return 0.5f;
        }
    }
    
    static float GetBaseRange(TurretType type)
    {
        switch (type)
        {
            case TurretType.Basic: return 10f;
            case TurretType.Laser: return 12f;
            case TurretType.RapidFire: return 8f;
            case TurretType.Sniper: return 15f;
            case TurretType.Shotgun: return 7f;
            default: return 10f;
        }
    }
    
    // ===== РАЗБЛОКИРОВКА ТУРЕЛЕЙ =====
    public static bool IsTurretUnlocked(TurretType type)
    {
        if (type == TurretType.Basic) return true;
        return PlayerPrefs.GetInt($"TurretUnlocked_{type}", 0) == 1;
    }
    
    public static void UnlockTurret(TurretType type)
    {
        PlayerPrefs.SetInt($"TurretUnlocked_{type}", 1);
        PlayerPrefs.Save();
    }
    
    // ===== УЛУЧШЕНИЯ ИГРОКА =====
    public static int BonusMaxHealth
    {
        get => PlayerPrefs.GetInt("BonusMaxHealth", 0);
        set => PlayerPrefs.SetInt("BonusMaxHealth", value);
    }
    
    public static float BonusSpeed
    {
        get => PlayerPrefs.GetFloat("BonusSpeed", 0f);
        set => PlayerPrefs.SetFloat("BonusSpeed", value);
    }
    
    public static float BonusArmor
    {
        get => PlayerPrefs.GetFloat("BonusArmor", 0f);
        set => PlayerPrefs.SetFloat("BonusArmor", value);
    }
    
    public static float BonusRegeneration
    {
        get => PlayerPrefs.GetFloat("BonusRegeneration", 0f);
        set => PlayerPrefs.SetFloat("BonusRegeneration", value);
    }
    
    public static int SavedHealth
    {
        get => PlayerPrefs.GetInt("SavedHealth", 100);
        set => PlayerPrefs.SetInt("SavedHealth", value);
    }
    
    // ===== СБРОС =====
    public static void ResetAll()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("Все данные сброшены!");
    }
}