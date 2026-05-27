using UnityEngine;
using UnityEngine.UI;
using TMPro;  // ← для TextMeshPro

public class UpgradeItemUI : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI nameText;        // ← TMP
    public TextMeshProUGUI descriptionText; // ← TMP
    public TextMeshProUGUI priceText;       // ← TMP
    public Button buyButton;
    
    private TurretUpgradeData turretUpgrade;
    private PlayerUpgradeData playerUpgrade;
    private UpgradeManager manager;
    
    public void SetupTurretUpgrade(TurretUpgradeData upgrade, UpgradeManager upgradeManager)
    {
        turretUpgrade = upgrade;
        manager = upgradeManager;
        
        if (icon != null && upgrade.upgradeIcon != null)
            icon.sprite = upgrade.upgradeIcon;
        
        if (nameText != null)
            nameText.text = upgrade.upgradeName;
        
        if (descriptionText != null)
            descriptionText.text = upgrade.description;
        
        if (priceText != null)
            priceText.text = $"{upgrade.upgradePrice} монет";
        
        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(() => manager.ApplyTurretUpgrade(turretUpgrade));
        
        Debug.Log($"Турель улучшение настроено: {upgrade.upgradeName}");
    }
    
    public void SetupPlayerUpgrade(PlayerUpgradeData upgrade, UpgradeManager upgradeManager)
    {
        playerUpgrade = upgrade;
        manager = upgradeManager;
        
        if (icon != null && upgrade.upgradeIcon != null)
            icon.sprite = upgrade.upgradeIcon;
        
        if (nameText != null)
            nameText.text = upgrade.upgradeName;
        
        if (descriptionText != null)
            descriptionText.text = upgrade.description;
        
        if (priceText != null)
            priceText.text = $"{upgrade.upgradePrice} монет";
        
        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(() => manager.ApplyPlayerUpgrade(playerUpgrade));
        
        Debug.Log($"Игрок улучшение настроено: {upgrade.upgradeName}");
    }
}