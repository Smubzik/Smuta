using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    public GameObject upgradeMenuPanel;
    public Transform turretUpgradesContainer;
    public Transform playerUpgradesContainer;
    public GameObject upgradeItemPrefab;
    public List<TurretUpgradeData> availableTurretUpgrades;
    public List<PlayerUpgradeData> availablePlayerUpgrades;
    public TextMeshProUGUI currencyText;
    public GameObject settingsButton;

    public GameObject basicTurretPrefab;
    public GameObject rapidFireTurretPrefab;
    public GameObject sniperTurretPrefab;
    public GameObject shotgunTurretPrefab;
    public GameObject laserTurretPrefab;

    private Train player;
    private Turret currentTurret;
    private static bool isFirstRun = false;
    private bool isUpdatingMenu = false; // Защита от рекурсивных вызовов

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
        if (currentTurret != null)
        {
            currentTurret.damage = GameData.TurretDamage;
            currentTurret._startTime = GameData.TurretFireDelay;
            currentTurret.extraProjectiles = GameData.TurretExtraProjectiles;
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
        LoadUpgrades();

        if (upgradeMenuPanel != null)
            upgradeMenuPanel.SetActive(false);
    }

    private void Update()
    {
        // Только закрытие магазина по Escape
        if (Input.GetKeyDown(KeyCode.Escape) && upgradeMenuPanel != null && upgradeMenuPanel.activeSelf)
        {
            ToggleUpgradeMenu();
            return;
        }

        // Если игра на паузе — не открываем магазин
        if (Time.timeScale == 0f) return;

        // Открытие магазина по U
        if (Input.GetKeyDown(KeyCode.U))
        {
            ToggleUpgradeMenu();
        }
    }

    public void ToggleUpgradeMenu()
    {
        if (upgradeMenuPanel == null) return;

        bool isOpen = !upgradeMenuPanel.activeSelf;

        if (isOpen)
        {
            // Открываем магазин
            upgradeMenuPanel.SetActive(true);
            if (settingsButton != null)
                settingsButton.SetActive(false);
            Time.timeScale = 0f;
            LoadUpgrades();
        }
        else
        {
            // Закрываем магазин и блокируем паузу на этот кадр
            upgradeMenuPanel.SetActive(false);
            if (settingsButton != null)
                settingsButton.SetActive(true);
            Time.timeScale = 1f;

            // ВАЖНО: блокируем открытие паузы в этом же кадре
            PauseMenu.BlockPauseForThisFrame();
        }
    }

    private void LoadUpgrades()
    {
        if (turretUpgradesContainer == null || playerUpgradesContainer == null) return;

        foreach (Transform child in turretUpgradesContainer)
            Destroy(child.gameObject);

        foreach (Transform child in playerUpgradesContainer)
            Destroy(child.gameObject);

        foreach (var upgrade in availableTurretUpgrades)
        {
            if (upgrade == null) continue;
            GameObject item = Instantiate(upgradeItemPrefab, turretUpgradesContainer);
            var ui = item.GetComponent<UpgradeItemUI>();
            if (ui != null) ui.SetupTurretUpgrade(upgrade, this);
        }

        foreach (var upgrade in availablePlayerUpgrades)
        {
            if (upgrade == null) continue;
            GameObject item = Instantiate(upgradeItemPrefab, playerUpgradesContainer);
            var ui = item.GetComponent<UpgradeItemUI>();
            if (ui != null) ui.SetupPlayerUpgrade(upgrade, this);
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
        if (currentTurret == null) return;

        if (PurchaseUpgrade(upgrade.upgradePrice))
        {
            switch (upgrade.upgradeType)
            {
                case UpgradeType.Damage:
                    currentTurret.damage += upgrade.damageBonus;
                    GameData.TurretDamage = currentTurret.damage;
                    break;
                case UpgradeType.FireRate:
                    currentTurret._startTime -= upgrade.fireRateBonus;
                    if (currentTurret._startTime < 0.1f)
                        currentTurret._startTime = 0.1f;
                    GameData.TurretFireDelay = currentTurret._startTime;
                    break;
                case UpgradeType.MultiShot:
                    currentTurret.extraProjectiles += upgrade.extraProjectiles;
                    GameData.TurretExtraProjectiles = currentTurret.extraProjectiles;
                    break;
                case UpgradeType.SwitchTurret:
                    SwitchTurret(upgrade.turretType);
                    break;
            }
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
            }
            LoadUpgrades();
        }
    }

    private void SwitchTurret(TurretType type)
    {
        if (currentTurret == null) return;

        Vector3 pos = currentTurret.transform.position;
        Quaternion rot = currentTurret.transform.rotation;
        Destroy(currentTurret.gameObject);

        GameObject prefab = type switch
        {
            TurretType.RapidFire => rapidFireTurretPrefab,
            TurretType.Sniper => sniperTurretPrefab,
            TurretType.Shotgun => shotgunTurretPrefab,
            TurretType.Laser => laserTurretPrefab,
            _ => basicTurretPrefab
        };

        if (prefab != null)
        {
            GameObject obj = Instantiate(prefab, pos, rot);
            currentTurret = obj.GetComponent<Turret>();
            currentTurret.damage = GameData.TurretDamage;
            currentTurret._startTime = GameData.TurretFireDelay;
            currentTurret.extraProjectiles = GameData.TurretExtraProjectiles;
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