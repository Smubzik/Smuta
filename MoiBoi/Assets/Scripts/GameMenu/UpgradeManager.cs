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
    
    [Header("Turret Tabs (кнопки вкладки)")]
    public Button basicTab;
    public Button laserTab;
    public Button rapidFireTab;
    public Button sniperTab;
    public Button shotgunTab;
    public TextMeshProUGUI selectedTurretNameText;
    
    private Train player;
    private Turret currentTurret;
    private static bool isFirstRun = false;
    
    // Текущая выбранная турель в магазине
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
        
        if (!isFirstRun)
        {
            isFirstRun = true;
            GameData.ResetAll();
            Debug.Log("Новая игра");
        }
        
        // Настройка кнопок-вкладок
        SetupTurretTabs();
        
        // Загружаем улучшения для начальной турели
        if (currentTurret != null)
        {
            TurretType currentType = GetCurrentTurretType();
            currentTurret.damage = GameData.GetTurretDamage(currentType);
            currentTurret._startTime = GameData.GetTurretFireDelay(currentType);
            currentTurret.extraProjectiles = GameData.GetTurretExtraProjectiles(currentType);
        }
        
        // Загружаем дальность для LightningTurret
        LightningTurret lightning = FindObjectOfType<LightningTurret>();
        if (lightning != null)
        {
            TurretType currentType = GetCurrentTurretType();
            lightning.UpdateRange(GameData.GetTurretRange(currentType));
        }
        
        if (player != null)
        {
            if (GameData.BonusMaxHealth > 0)
                player.UpgradeMaxHealth(GameData.BonusMaxHealth);
            if (GameData.BonusSpeed > 0)
                player.UpgradeSpeed(GameData.BonusSpeed);
            if (GameData.BonusArmor > 0)
                player.UpgradeArmor(GameData.BonusArmor);
            if (GameData.BonusRegeneration > 0)
                player.UpgradeRegeneration(GameData.BonusRegeneration);
        }
        
        UpdateCurrencyUI();
        
        if (upgradeMenuPanel != null)
            upgradeMenuPanel.SetActive(false);
    }
    
    void SetupTurretTabs()
    {
        if (basicTab != null)
            basicTab.onClick.AddListener(ShowBasicUpgrades);
        if (laserTab != null)
            laserTab.onClick.AddListener(ShowLaserUpgrades);
        if (rapidFireTab != null)
            rapidFireTab.onClick.AddListener(ShowRapidFireUpgrades);
        if (sniperTab != null)
            sniperTab.onClick.AddListener(ShowSniperUpgrades);
        if (shotgunTab != null)
            shotgunTab.onClick.AddListener(ShowShotgunUpgrades);
        
        ShowBasicUpgrades();
    }
    
    // ===== МЕТОДЫ ДЛЯ КНОПОК-ВКЛАДОК =====
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
        // Закрытие магазина по Escape или по U
        if (upgradeMenuPanel != null && upgradeMenuPanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.U))
            {
                ToggleUpgradeMenu();
                return;
            }
        }
        
        // Открытие магазина по U
        if (Time.timeScale != 0f && Input.GetKeyDown(KeyCode.U))
        {
            ToggleUpgradeMenu();
        }
        
        // Переключение турелей по цифрам
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
        
        // Выключаем все турели
        foreach (Transform child in activeWeaponParent.transform)
            child.gameObject.SetActive(false);
        
        // Включаем выбранную
        activeWeaponParent.transform.GetChild(index).gameObject.SetActive(true);
        currentTurret = activeWeaponParent.transform.GetChild(index).GetComponent<Turret>();
        selectedTurretIndex = index;
        
        // Загружаем улучшения для этой турели
        if (currentTurret != null)
        {
            currentTurret.damage = GameData.GetTurretDamage(type);
            currentTurret._startTime = GameData.GetTurretFireDelay(type);
            currentTurret.extraProjectiles = GameData.GetTurretExtraProjectiles(type);
            Debug.Log($"Активирована {type}: урон={currentTurret.damage}, задержка={currentTurret._startTime}");
        }
        
        // Загружаем дальность для LightningTurret
        LightningTurret lightning = FindObjectOfType<LightningTurret>();
        if (lightning != null)
        {
            lightning.UpdateRange(GameData.GetTurretRange(type));
        }
        
        // Если магазин открыт — обновляем отображаемые улучшения
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
        
        // Очищаем старые
        foreach (Transform child in turretUpgradesContainer)
            Destroy(child.gameObject);
        foreach (Transform child in playerUpgradesContainer)
            Destroy(child.gameObject);
        
        // Показываем улучшения для выбранной вкладки
        foreach (var upgrade in availableTurretUpgrades)
        {
            if (upgrade == null) continue;
            
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
        if (PurchaseUpgrade(upgrade.upgradePrice))
        {
            switch (upgrade.upgradeType)
            {
                case UpgradeType.Damage:
                    float newDamage = GameData.GetTurretDamage(currentShopTurretType) + upgrade.damageBonus;
                    GameData.SetTurretDamage(currentShopTurretType, newDamage);
                    
                    if (GetCurrentTurretType() == currentShopTurretType && currentTurret != null)
                        currentTurret.damage = newDamage;
                    break;
                    
                case UpgradeType.FireRate:
                    float newDelay = GameData.GetTurretFireDelay(currentShopTurretType) - upgrade.fireRateBonus;
                    if (newDelay < 0.1f) newDelay = 0.1f;
                    GameData.SetTurretFireDelay(currentShopTurretType, newDelay);
                    
                    if (GetCurrentTurretType() == currentShopTurretType && currentTurret != null)
                        currentTurret._startTime = newDelay;
                    break;
                    
                case UpgradeType.MultiShot:
                    int newProjectiles = GameData.GetTurretExtraProjectiles(currentShopTurretType) + upgrade.extraProjectiles;
                    GameData.SetTurretExtraProjectiles(currentShopTurretType, newProjectiles);
                    
                    if (GetCurrentTurretType() == currentShopTurretType && currentTurret != null)
                        currentTurret.extraProjectiles = newProjectiles;
                    break;
                    
                case UpgradeType.Range:
                    float newRange = GameData.GetTurretRange(currentShopTurretType) + upgrade.rangeBonus;
                    GameData.SetTurretRange(currentShopTurretType, newRange);
                    Debug.Log($"Дальность для {currentShopTurretType} увеличена до {newRange}");
                    
                    LightningTurret lightning = FindObjectOfType<LightningTurret>();
                    if (lightning != null)
                    {
                        lightning.UpdateRange(newRange);
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
    
    public void ApplyPlayerUpgrade(PlayerUpgradeData upgrade)
    {
        if (player == null) return;
        
        if (PurchaseUpgrade(upgrade.upgradePrice))
        {
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