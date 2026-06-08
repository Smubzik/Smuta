using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PauseMenu : MonoBehaviour
{
    public bool _pausegame;
    public GameObject _pausegamemenu;
    public GameObject settingsButton;
    private UpgradeManager upgradeManager;
    
    // ДОБАВЬ ЭТИ ПОЛЯ
    public GameObject healthBarObject;
    public GameObject coinCounterObject;

    private static bool blockPauseThisFrame = false;

    void Start()
    {
        upgradeManager = FindObjectOfType<UpgradeManager>();
    }

    void Update()
    {
        if (blockPauseThisFrame)
        {
            blockPauseThisFrame = false;
            return;
        }

        if (upgradeManager != null && upgradeManager.IsUpgradeMenuOpen())
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_pausegame) Resume();
            else Pause();
        }
    }

    public static void BlockPauseForThisFrame()
    {
        blockPauseThisFrame = true;
    }

    public void Resume()
    {
        _pausegamemenu.SetActive(false);
        if (settingsButton != null) settingsButton.SetActive(true);
        Time.timeScale = 1f;
        _pausegame = false;
        
        ShowGameUI(true);
    }

    public void Pause()
    {
        _pausegamemenu.SetActive(true);
        if (settingsButton != null) settingsButton.SetActive(false);
        Time.timeScale = 0f;
        _pausegame = true;
        
        ShowGameUI(false);
    }

    public void LordMenu()
    {
        if (upgradeManager != null && upgradeManager.IsUpgradeMenuOpen())
            upgradeManager.ToggleUpgradeMenu();

        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void NewGame()
    {
        GameData.ResetAll();
        SceneManager.LoadScene("Project");
        Time.timeScale = 1f;
    }
    
    private void ShowGameUI(bool show)
    {
        if (healthBarObject != null)
            healthBarObject.SetActive(show);
        
        if (coinCounterObject != null)
            coinCounterObject.SetActive(show);
    }
}