using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PauseMenu : MonoBehaviour
{
    public bool _pausegame;
    public GameObject _pausegamemenu;
    public GameObject settingsButton;
    private UpgradeManager upgradeManager;

    // Статический флаг, который блокирует паузу на один кадр
    private static bool blockPauseThisFrame = false;

    [System.Obsolete]
    void Start()
    {
        upgradeManager = FindObjectOfType<UpgradeManager>();
    }

    void Update()
    {
        // Сбрасываем флаг в начале каждого кадра
        if (blockPauseThisFrame)
        {
            blockPauseThisFrame = false;
            return;
        }

        // Если магазин открыт — не реагируем на Escape
        if (upgradeManager != null && upgradeManager.IsUpgradeMenuOpen())
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_pausegame) Resume();
            else Pause();
        }
    }

    // Статический метод для блокировки паузы
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