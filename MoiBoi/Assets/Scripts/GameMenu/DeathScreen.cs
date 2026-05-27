using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class DeathScreen : MonoBehaviour
{
    [Header("UI Components")]
    public GameObject deathScreenPanel;
    public TextMeshProUGUI deathText;
    public Button restartButton;
    public Button menuButton;
    
    private Train train;
    private static bool isDeathScreenActive = false;
    
    public static bool IsDeathScreenActive => isDeathScreenActive;
    
    private void Start()
    {
        train = FindObjectOfType<Train>();
        
        if (train != null)
        {
            train.OnPlayerDeath += ShowDeathScreen;
        }
        
        if (deathScreenPanel != null)
            deathScreenPanel.SetActive(false);
        
        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);
        
        if (menuButton != null)
            menuButton.onClick.AddListener(GoToMenu);
        
        isDeathScreenActive = false;
    }
    
    private void ShowDeathScreen(object sender, System.EventArgs e)
    {
        if (deathScreenPanel != null)
        {
            deathScreenPanel.SetActive(true);
            isDeathScreenActive = true;
            
            if (deathText != null)
            {
                deathText.text = "ПОЕЗД УНИЧТОЖЕН\n\nВы проиграли!";
            }
            
            Time.timeScale = 0f;
        }
    }
    
    private void RestartGame()
    {
        isDeathScreenActive = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    private void GoToMenu()
    {
        isDeathScreenActive = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}