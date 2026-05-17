using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool _pausegame;
    public GameObject _pausegamemenu;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (_pausegame) Resume();
            else Pause();
        }
    }

    public void Resume() {
        _pausegamemenu.SetActive(false);
        Time.timeScale = 1f;
        _pausegame = false;
    }

    public void Pause() {
        _pausegamemenu.SetActive(true);
        Time.timeScale = 0f;
        _pausegame = true;
    }

    public void LordMenu() {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }   
}
