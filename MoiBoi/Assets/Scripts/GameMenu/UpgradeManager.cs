using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;
    
    [Header("UI Components")]
    public GameObject upgradeMenuPanel;
    public Transform turretUpgradesContainer;
    public Transform playerUpgradesContainer;
    public GameObject upgradeItemPrefab;
    
    [Header("Upgrades Lists")]
    public List<TurretUpgradeData> availableTurretUpgrades;
    public List<PlayerUpgradeData> availablePlayerUpgrades;
    
    [Header("Player Stats")]
    public int playerCurrency = 1000;
    public TextMeshProUGUI currencyText;
    
    [Header("Settings Button")]
    public GameObject settingsButton;
    
    [Header("Turret Switching")]
    public GameObject activeWeaponParent;
    public int selectedTurretIndex = 0;
    
    [Header("Turret Tabs")]
    public Button basicTab;
    public Button laserTab;
    public Button rapidFireTab;
    public Button sniperTab;
    public Button shotgunTab;
    public TextMeshProUGUI selectedTurretNameText;
    
    private Train player;
    private Turret currentTurret;
    private RapidTurret currentRapidTurret;
    private LightningTurret currentLightningTurret;
    private static bool isFirstRun = false;
    private TurretType currentShopTurretType = TurretType.Basic;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    
    private void Start()
    {
        player = FindObjectOfType<Train>();
        currentTurret = FindObjectOfType<Turret>();
        currentRapidTurret = FindObjectOfType<RapidTurret>();
        currentLightningTurret = FindObjectOfType<LightningTurret>();
        
        if (!isFirstRun)
        {
            isFirstRun = true;
            GameData.ResetAll();
            Debug.Log("Новая игра");
            ResetLightningTurretRange();
        }
        else
        {
            LoadLightningTurretRange();
        }
        
        SetupTurretTabs();
        
        LoadCurrentTurretStats();
        
        if (player != null)
        {
            if (GameData.BonusMaxHealth > 0) player.UpgradeMaxHealth(GameData.BonusMaxHealth);
            if (GameData.BonusSpeed > 0) player.UpgradeSpeed(GameData.BonusSpeed);
            if (GameData.BonusArmor > 0) player.UpgradeArmor(GameData.BonusArmor);
            if (GameData.BonusRegeneration > 0) player.UpgradeRegeneration(GameData.BonusRegeneration);
        }
        
        UpdateCurrencyUI();
        
        if (upgradeMenuPanel != null)
            upgradeMenuPanel.SetActive(false);
    }
    
    void LoadCurrentTurretStats()
    {
        TurretType currentType = GetCurrentTurretType();
        
        if (currentTurret != null && currentType != TurretType.Laser && currentType != TurretType.RapidFire)
        {
            currentTurret.damage = GameData.GetTurretDamage(currentType);
            currentTurret._startTime = GameData.GetTurretFireDelay(currentType);
            currentTurret.extraProjectiles = GameData.GetTurretExtraProjectiles(currentType);
        }
        
        if (currentLightningTurret != null && currentType == TurretType.Laser)
        {
            currentLightningTurret.damage = GameData.GetTurretDamage(currentType);
            currentLightningTurret.attackRate = GameData.GetTurretFireDelay(currentType);
        }
        
        if (currentRapidTurret != null && currentType == TurretType.RapidFire)
        {
            currentRapidTurret.damage = GameData.GetTurretDamage(currentType);
            currentRapidTurret._startTime = GameData.GetTurretFireDelay(currentType);
            currentRapidTurret.extraProjectiles = GameData.GetTurretExtraProjectiles(currentType);
        }
    }
    
    void ResetLightningTurretRange()
    {
        if (currentLightningTurret != null)
        {
            currentLightningTurret.ResetRange();
        }
    }
    
    void LoadLightningTurretRange()
    {
        if (currentLightningTurret != null)
        {
            TurretType currentType = GetCurrentTurretType();
            currentLightningTurret.UpdateRange(GameData.GetTurretRange(currentType));
        }
    }
    
    void SetupTurretTabs()
    {
        if (basicTab != null) basicTab.onClick.AddListener(ShowBasicUpgrades);
        if (laserTab != null) laserTab.onClick.AddListener(ShowLaserUpgrades);
        if (rapidFireTab != null) rapidFireTab.onClick.AddListener(ShowRapidFireUpgrades);
        if (sniperTab != null) sniperTab.onClick.AddListener(ShowSniperUpgrades);
        if (shotgunTab != null) shotgunTab.onClick.AddListener(ShowShotgunUpgrades);
        
        ShowBasicUpgrades();
    }
    
    public void ShowBasicUpgrades() => ShowTurretUpgrades(TurretType.Basic);
    public void ShowLaserUpgrades() => ShowTurretUpgrades(TurretType.Laser);
    public void ShowRapidFireUpgrades() => ShowTurretUpgrades(TurretType.RapidFire);
    public void ShowSniperUpgrades() => ShowTurretUpgrades(TurretType.Sniper);
    public void ShowShotgunUpgrades() => ShowTurretUpgrades(TurretType.Shotgun);
    
    public void ShowTurretUpgrades(TurretType type)
    {
        currentShopTurretType = type;
        if (selectedTurretNameText != null)
            selectedTurretNameText.text = $"Улучшения для: {type}";
        LoadUpgrades();
        Debug.Log($"Показываем улучшения для турели: {type}");
    }
    
    private void Update()
    {
        if (upgradeMenuPanel != null && upgradeMenuPanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.U))
            {
                ToggleUpgradeMenu();
                return;
            }
        }
        
        if (Time.timeScale != 0f && Input.GetKeyDown(KeyCode.U))
        {
            ToggleUpgradeMenu();
        }
        
        if (!IsUpgradeMenuOpen())
        {
            CheckTurretSwitchInput();
        }
    }
    
    void CheckTurretSwitchInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
            TryActivateTurret(0, TurretType.Basic);
        else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
            TryActivateTurret(1, TurretType.Laser);
        else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
            TryActivateTurret(2, TurretType.RapidFire);
        else if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
            TryActivateTurret(3, TurretType.Sniper);
        else if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5))
            TryActivateTurret(4, TurretType.Shotgun);
    }
    
    void TryActivateTurret(int index, TurretType type)
    {
        if (!GameData.IsTurretUnlocked(type))
        {
            Debug.Log($"Турель {type} ещё не разблокирована! Купи улучшение в магазине.");
            return;
        }
        
        if (activeWeaponParent == null)
        {
            Debug.LogError("activeWeaponParent не назначен!");
            return;
        }
        
        if (index >= activeWeaponParent.transform.childCount)
        {
            Debug.LogError($"Индекс {index} выходит за пределы!");
            return;
        }
        
        if (selectedTurretIndex == index) return;
        
        foreach (Transform child in activeWeaponParent.transform)
            child.gameObject.SetActive(false);
        
        activeWeaponParent.transform.GetChild(index).gameObject.SetActive(true);
        
        // Обновляем ссылки на текущие турели
        currentTurret = activeWeaponParent.transform.GetChild(index).GetComponent<Turret>();
        currentRapidTurret = activeWeaponParent.transform.GetChild(index).GetComponent<RapidTurret>();
        currentLightningTurret = activeWeaponParent.transform.GetChild(index).GetComponent<LightningTurret>();
        selectedTurretIndex = index;
        
        LoadCurrentTurretStats();
        
        // Обновляем дальность для LightningTurret
        if (currentLightningTurret != null && type == TurretType.Laser)
        {
            currentLightningTurret.UpdateRange(GameData.GetTurretRange(type));
        }
        
        if (IsUpgradeMenuOpen())
        {
            ShowTurretUpgrades(type);
        }
    }
    
    public void ToggleUpgradeMenu()
    {
        if (upgradeMenuPanel == null) return;
        
        bool isOpen = !upgradeMenuPanel.activeSelf;
        upgradeMenuPanel.SetActive(isOpen);
        
        if (settingsButton != null)
            settingsButton.SetActive(!isOpen);
        
        if (isOpen)
        {
            Time.timeScale = 0f;
            ShowTurretUpgrades(GetCurrentTurretType());
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
    
    void LoadUpgrades()
    {
        if (turretUpgradesContainer == null || playerUpgradesContainer == null) return;
        
        foreach (Transform child in turretUpgradesContainer)
            Destroy(child.gameObject);
        foreach (Transform child in playerUpgradesContainer)
            Destroy(child.gameObject);
        
        // Улучшения турелей
        foreach (var upgrade in availableTurretUpgrades)
        {
            if (upgrade == null) continue;
            
            int purchaseCount = GameData.GetUpgradePurchaseCount(upgrade.name);
            if (purchaseCount >= upgrade.maxPurchases)
                continue;
            
            if (upgrade.upgradeType == UpgradeType.SwitchTurret)
            {
                if (!GameData.IsTurretUnlocked(upgrade.turretType))
                {
                    GameObject item = Instantiate(upgradeItemPrefab, turretUpgradesContainer);
                    var ui = item.GetComponent<UpgradeItemUI>();
                    if (ui != null) ui.SetupTurretUpgrade(upgrade, this);
                }
            }
            else if (upgrade.upgradeType == UpgradeType.Damage ||
                     upgrade.upgradeType == UpgradeType.FireRate ||
                     upgrade.upgradeType == UpgradeType.MultiShot ||
                     upgrade.upgradeType == UpgradeType.Range)
            {
                if (upgrade.turretType == currentShopTurretType)
                {
                    GameObject item = Instantiate(upgradeItemPrefab, turretUpgradesContainer);
                    var ui = item.GetComponent<UpgradeItemUI>();
                    if (ui != null) ui.SetupTurretUpgrade(upgrade, this);
                }
            }
        }
        
        // Улучшения игрока
        foreach (var upgrade in availablePlayerUpgrades)
        {
            if (upgrade == null) continue;
            
            int purchaseCount = GameData.GetUpgradePurchaseCount(upgrade.name);
            if (purchaseCount >= upgrade.maxPurchases)
                continue;
            
            GameObject item = Instantiate(upgradeItemPrefab, playerUpgradesContainer);
            var ui = item.GetComponent<UpgradeItemUI>();
            if (ui != null) ui.SetupPlayerUpgrade(upgrade, this);
        }
    }
    
    TurretType GetCurrentTurretType()
    {
        switch (selectedTurretIndex)
        {
            case 0: return TurretType.Basic;
            case 1: return TurretType.Laser;
            case 2: return TurretType.RapidFire;
            case 3: return TurretType.Sniper;
            case 4: return TurretType.Shotgun;
            default: return TurretType.Basic;
        }
    }
    
    public bool PurchaseUpgrade(int price)
    {
        if (GameData.Currency >= price)
        {
            GameData.Currency -= price;
            UpdateCurrencyUI();
            return true;
        }
        return false;
    }
    
    public void ApplyTurretUpgrade(TurretUpgradeData upgrade)
    {
        int purchaseCount = GameData.GetUpgradePurchaseCount(upgrade.name);
        if (purchaseCount >= upgrade.maxPurchases)
        {
            Debug.Log($"❌ {upgrade.upgradeName} уже куплено максимальное количество раз ({upgrade.maxPurchases})!");
            return;
        }
        
        if (PurchaseUpgrade(upgrade.upgradePrice))
        {
            GameData.SetUpgradePurchaseCount(upgrade.name, purchaseCount + 1);
            Debug.Log($"✅ Куплено {upgrade.upgradeName} ({purchaseCount + 1}/{upgrade.maxPurchases})");
            
            TurretType targetType = upgrade.turretType;
            
            switch (upgrade.upgradeType)
            {
                case UpgradeType.Damage:
                    float newDamage = GameData.GetTurretDamage(targetType) + upgrade.damageBonus;
                    GameData.SetTurretDamage(targetType, newDamage);
                    if (GetCurrentTurretType() == targetType)
                        UpdateCurrentTurretDamage(newDamage);
                    break;
                    
                case UpgradeType.FireRate:
                    float newDelay = GameData.GetTurretFireDelay(targetType) - upgrade.fireRateBonus;
                    if (newDelay < 0.1f) newDelay = 0.1f;
                    GameData.SetTurretFireDelay(targetType, newDelay);
                    if (GetCurrentTurretType() == targetType)
                        UpdateCurrentTurretFireDelay(newDelay);
                    break;
                    
                case UpgradeType.MultiShot:
                    int newProjectiles = GameData.GetTurretExtraProjectiles(targetType) + upgrade.extraProjectiles;
                    GameData.SetTurretExtraProjectiles(targetType, newProjectiles);
                    if (GetCurrentTurretType() == targetType)
                        UpdateCurrentTurretExtraProjectiles(newProjectiles);
                    break;
                    
                case UpgradeType.Range:
                    float newRange = GameData.GetTurretRange(targetType) + upgrade.rangeBonus;
                    GameData.SetTurretRange(targetType, newRange);
                    Debug.Log($"Дальность для {targetType} увеличена до {newRange}");
                    
                    if (currentLightningTurret != null && targetType == TurretType.Laser)
                    {
                        currentLightningTurret.UpdateRange(newRange);
                    }
                    break;
                    
                case UpgradeType.SwitchTurret:
                    GameData.UnlockTurret(upgrade.turretType);
                    Debug.Log($"Разблокирована турель: {upgrade.turretType}");
                    break;
            }
            
            if (IsUpgradeMenuOpen())
                LoadUpgrades();
        }
    }
    
    void UpdateCurrentTurretDamage(float newDamage)
    {
        if (currentTurret != null) currentTurret.damage = newDamage;
        if (currentRapidTurret != null) currentRapidTurret.damage = newDamage;
        if (currentLightningTurret != null) currentLightningTurret.damage = newDamage;
    }
    
    void UpdateCurrentTurretFireDelay(float newDelay)
    {
        if (currentTurret != null) currentTurret._startTime = newDelay;
        if (currentRapidTurret != null) currentRapidTurret._startTime = newDelay;
        if (currentLightningTurret != null) currentLightningTurret.attackRate = newDelay;
    }
    
    void UpdateCurrentTurretExtraProjectiles(int newCount)
    {
        if (currentTurret != null) currentTurret.extraProjectiles = newCount;
        if (currentRapidTurret != null) currentRapidTurret.extraProjectiles = newCount;
        // LightningTurret не использует extraProjectiles
    }
    
    public void ApplyPlayerUpgrade(PlayerUpgradeData upgrade)
    {
        if (player == null) return;
        
        int purchaseCount = GameData.GetUpgradePurchaseCount(upgrade.name);
        if (purchaseCount >= upgrade.maxPurchases)
        {
            Debug.Log($"❌ {upgrade.upgradeName} уже куплено максимальное количество раз ({upgrade.maxPurchases})!");
            return;
        }
        
        if (PurchaseUpgrade(upgrade.upgradePrice))
        {
            GameData.SetUpgradePurchaseCount(upgrade.name, purchaseCount + 1);
            Debug.Log($"✅ Куплено {upgrade.upgradeName} ({purchaseCount + 1}/{upgrade.maxPurchases})");
            
            switch (upgrade.upgradeType)
            {
                case PlayerUpgradeType.MaxHealth:
                    player.UpgradeMaxHealth(upgrade.healthBonus);
                    GameData.BonusMaxHealth += upgrade.healthBonus;
                    GameData.SavedHealth = player.CurrentHealth;
                    break;
                case PlayerUpgradeType.Speed:
                    player.UpgradeSpeed(upgrade.speedBonus);
                    GameData.BonusSpeed += upgrade.speedBonus;
                    break;
                case PlayerUpgradeType.Armor:
                    player.UpgradeArmor(upgrade.armorBonus);
                    GameData.BonusArmor += upgrade.armorBonus;
                    break;
                case PlayerUpgradeType.Regeneration:
                    player.UpgradeRegeneration(upgrade.healthBonus);
                    GameData.BonusRegeneration += upgrade.healthBonus;
                    break;
                case PlayerUpgradeType.Heal:
                    player.Heal(upgrade.healthBonus);
                    GameData.SavedHealth = player.CurrentHealth;
                    break;
            }
            
            if (IsUpgradeMenuOpen())
                LoadUpgrades();
        }
    }
    
    private void UpdateCurrencyUI()
    {
        if (currencyText != null)
            currencyText.text = $"Монеты: {GameData.Currency}";
    }
    
    public bool IsUpgradeMenuOpen() => upgradeMenuPanel != null && upgradeMenuPanel.activeSelf;
    
    public void AddCurrency(int amount)
    {
        GameData.Currency += amount;
        UpdateCurrencyUI();
    }
}