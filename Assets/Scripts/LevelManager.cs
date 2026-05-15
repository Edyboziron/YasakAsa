using UnityEngine;
using UnityEngine.SceneManagement; // Sahne yönetimi için gerekli

public class LevelManager : MonoBehaviour
{
    [Header("Sahne Ýsimleri")]
    public string winSceneName = "WinScene";      // Kazanma sahnesinin adý
    public string loseSceneName = "GameOverScene"; // Kaybetme sahnesinin adý

    [Header("Referanslar")]
    public PlayerStats player; // Oyuncunun canýný kontrol etmek için

    private bool isGameOver = false;

    void Update()
    {
        if (isGameOver) return; // Eðer oyun zaten bittiyse kontrol etmeyi býrak

        CheckWinCondition();
        CheckLoseCondition();
    }

    void CheckWinCondition()
    {
        // "Enemy" tag'ine sahip tüm objeleri bul
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // Eðer listede hiç obje kalmadýysa (ve oyun henüz bitmediyse)
        if (enemies.Length == 0)
        {
            WinGame();
        }
    }

    void CheckLoseCondition()
    {
        // Oyuncu referansý varsa ve caný 0 veya altýndaysa
        if (player != null && player.health <= 0)
        {
            LoseGame();
        }
    }

    void WinGame()
    {
        isGameOver = true;
        Debug.Log("Düþman kalmadý! Kazanma sahnesine gidiliyor...");
        SceneManager.LoadScene(winSceneName);
    }

    void LoseGame()
    {
        isGameOver = true;
        Debug.Log("Oyuncu öldü! Kaybetme sahnesine gidiliyor...");
        SceneManager.LoadScene(loseSceneName);
    }
}