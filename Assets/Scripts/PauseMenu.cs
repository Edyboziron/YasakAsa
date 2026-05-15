using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Tooltip("Esc ekranýndaki Canvas'ý (veya paneli) buraya sürükle")]
    public GameObject pauseMenuUI;

    private bool isPaused = false;

    void Update()
    {
        // ESC tuþuna basýlýnca
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false); // Menüyü gizle
        Time.timeScale = 1f;          // Zamaný normal akýþýna getir
        isPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);  // Menüyü göster
        Time.timeScale = 0f;          // Zamaný DURDUR (Her þey donar)
        isPaused = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f; // Ana menüye dönerken zamaný tekrar açmayý unutma!
        SceneManager.LoadScene("AnaEkran");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}