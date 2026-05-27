using UnityEngine;

public static class GameData
{
    // Монеты
    public static int Currency
    {
        get => PlayerPrefs.GetInt("Currency", 200);
        set => PlayerPrefs.SetInt("Currency", value);
    }
    
    // Улучшения турели
    public static float TurretDamage
    {
        get => PlayerPrefs.GetFloat("TurretDamage", 10f);
        set => PlayerPrefs.SetFloat("TurretDamage", value);
    }   
    
    public static float TurretFireDelay
    {
        get => PlayerPrefs.GetFloat("TurretFireDelay", 0.5f);
        set => PlayerPrefs.SetFloat("TurretFireDelay", value);
    }
    
    public static int TurretExtraProjectiles
    {
        get => PlayerPrefs.GetInt("TurretExtraProjectiles", 0);
        set => PlayerPrefs.SetInt("TurretExtraProjectiles", value);
    }
    
    // Бонусы игрока
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
    
    // Сохранённое здоровье (последнее перед выходом)
    public static int SavedHealth
    {
        get => PlayerPrefs.GetInt("SavedHealth", 100);
        set => PlayerPrefs.SetInt("SavedHealth", value);
    }
    
    // Сброс всех данных (при новой игре)
    public static void ResetAll()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}