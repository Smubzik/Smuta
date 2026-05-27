using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool _pausegame;
    public GameObject _pausegamemenu;
    public GameObject settingsButton;
    private UpgradeManager upgradeManager;

    void Start()
    {
        upgradeManager = FindObjectOfType<UpgradeManager>();
    }

    void Update()
    {
        // Если магазин открыт — не реагируем на Escape (не открываем паузу)
        if (upgradeManager != null && upgradeManager.IsUpgradeMenuOpen())
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_pausegame) Resume();
            else Pause();
        }
    }

    public void Resume()
    {
        _pausegamemenu.SetActive(false);
        if (settingsButton != null) settingsButton.SetActive(true);
        Time.timeScale = 1f;
        _pausegame = false;
    }

    public void Pause()
    {
        _pausegamemenu.SetActive(true);
        if (settingsButton != null) settingsButton.SetActive(false);
        Time.timeScale = 0f;
        _pausegame = true;
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
}