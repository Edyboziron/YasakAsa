using UnityEngine;
using System.Collections;
using TMPro; // TextMeshPro kullanmak için bu satýr þart!

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance;

    [Header("Referanslar")]
    public PlayerStats player;
    public TextMeshProUGUI moveCountText; // Sahnendeki TMP objesini buraya sürükle

    [Header("Tur Ayarlarý")]
    public bool isPlayerTurn = true;
    public int stabilityRecovery = 5;
    public int cardsPlayedThisTurn = 0;
    public int maxCardsPerTurn = 3;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // Oyun baþýnda arayüzü güncelle
        UpdateMoveUI();
    }

    public void RecordCardPlayed()
    {
        cardsPlayedThisTurn++;
        UpdateMoveUI(); // Her kart oynandýðýnda yazýyý güncelle

        if (cardsPlayedThisTurn >= maxCardsPerTurn)
        {
            EndTurn();
        }
    }

    public void EndTurn()
    {
        if (!isPlayerTurn) return;
        StartCoroutine(EnemyTurnRoutine());
    }

    // Arayüzü güncelleyen yardýmcý fonksiyon
    public void UpdateMoveUI()
    {
        if (moveCountText != null)
        {
            // Örn: "HAMLE: 1 / 3"
            moveCountText.text = $"HAMLE: {cardsPlayedThisTurn} / {maxCardsPerTurn}";

            // Hamle hakký bittiyse görsel uyarý ver (Kýrmýzý yap)
            if (cardsPlayedThisTurn >= maxCardsPerTurn)
                moveCountText.color = Color.red;
            else
                moveCountText.color = Color.black;
        }
    }

    IEnumerator EnemyTurnRoutine()
    {
        isPlayerTurn = false;
        UpdateMoveUI(); // Yazýyý güncelle (Düþman turunda olduðunu belirtmek için opsiyonel)

        Debug.Log("<color=red>--- DÜÞMAN TURU ---</color>");

        // Sahnede o an yaþayan tüm düþmanlarý bul
        Enemy[] enemies = Object.FindObjectsByType<Enemy>(FindObjectsSortMode.None);

        foreach (Enemy e in enemies)
        {
            if (e == null || !e.gameObject.activeInHierarchy) continue;

            yield return StartCoroutine(e.VisualAttack(player));
            yield return new WaitForSeconds(0.3f);
        }

        // --- OYUNCU TURU BAÞLIYOR ---
        isPlayerTurn = true;
        cardsPlayedThisTurn = 0; // Hamleleri sýfýrla

        if (player != null)
            player.ChangeStability(stabilityRecovery);

        UpdateMoveUI(); // Yeni turda yazýyý tekrar beyaz ve 0/3 yap
        Debug.Log("<color=green>--- OYUNCU TURU ---</color>");
    }
}