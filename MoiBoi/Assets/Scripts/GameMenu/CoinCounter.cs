using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CoinCounter : MonoBehaviour
{
    public TextMeshProUGUI coinText;
    public Image coinIcon;
    public CanvasGroup canvasGroup; // для плавного появления/исчезновения (опционально)
    
    private void Start()
    {
        if (coinText == null)
            coinText = GetComponent<TextMeshProUGUI>();
        
        UpdateCoinDisplay();
    }
    
    private void Update()
    {
        UpdateCoinDisplay();
    }
    
    public void UpdateCoinDisplay()
    {
        if (coinText != null)
        {
            coinText.text = $"{GameData.Currency}";
        }
    }
    
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    
    public void Show()
    {
        gameObject.SetActive(true);
    }
}