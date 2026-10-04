using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    [Header("Statistics")]
    public int health = 100;
    public int stability = 100;
    public int armor = 0;

    [Header("UI Texts")]
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI stabilityText;

    [Header("Visual Settings")]
    public Image targetZoneImage;
    public Sprite normalSprite;          // Stability >= 50
    public Sprite stabilityGlitchSprite; // Stability < 50
    public Sprite damagedSprite;         // Hit impact sprite
    public float effectDuration = 0.3f;

    private Vector3 originalStabilityPos;
    private bool wasGlitched = false;
    private bool isEffectActive = false; // Pauses base sprite updates during damage/heal flashes

    void Start()
    {
        if (stabilityText != null)
            originalStabilityPos = stabilityText.rectTransform.localPosition;

        if (targetZoneImage == null)
            targetZoneImage = GetComponent<Image>();

        UpdateUI();
        UpdatePlayerBaseImage();
    }

    void Update()
    {
        stability = Mathf.Clamp(stability, 0, 100);

        // 1. Text shake and color checks
        UpdateUI();

        // 2. Music corruption transition check
        CheckMusicTransition();

        // 3. Player base portrait update (unless flashing)
        if (!isEffectActive)
        {
            UpdatePlayerBaseImage();
        }
    }

    private void UpdatePlayerBaseImage()
    {
        if (targetZoneImage == null) return;

        if (stability < 50 && stabilityGlitchSprite != null)
            targetZoneImage.sprite = stabilityGlitchSprite;
        else if (normalSprite != null)
            targetZoneImage.sprite = normalSprite;
    }

    public void UpdateUI()
    {
        if (healthText != null)
            healthText.text = $"CAN: {health} | ZIRH: {armor}";

        if (stabilityText != null)
        {
            stabilityText.text = "STABILITE: %" + stability;

            if (stability < 50)
            {
                stabilityText.color = Color.red;
                float s = (50 - stability) * 0.2f;
                stabilityText.rectTransform.localPosition = originalStabilityPos +
                    new Vector3(Random.Range(-s, s), Random.Range(-s, s), 0);
            }
            else
            {
                stabilityText.color = Color.white;
                stabilityText.rectTransform.localPosition = originalStabilityPos;
            }
        }
    }

    private void CheckMusicTransition()
    {
        if (MusicManager.Instance == null) return;

        if (stability < 50 && !wasGlitched)
        {
            wasGlitched = true;
            Debug.Log("Müzik Bozuluyor...");
            MusicManager.Instance.SwitchMusic(true);
        }
        else if (stability >= 50 && wasGlitched)
        {
            wasGlitched = false;
            Debug.Log("Müzik Düzeliyor...");
            MusicManager.Instance.SwitchMusic(false);
        }
    }

    public void TakeDamage(int amount)
    {
        if (amount <= 0) return;

        if (armor > 0)
        {
            if (armor >= amount) { armor -= amount; amount = 0; }
            else { amount -= armor; armor = 0; }
        }

        health -= amount;
        if (health < 0) health = 0;

        StopCoroutine("FlashEffect");
        StartCoroutine(FlashEffect(Color.red, damagedSprite));
    }

    public void TriggerHealEffect()
    {
        StopCoroutine("FlashEffect");
        StartCoroutine(FlashEffect(Color.green, null));
    }

    IEnumerator FlashEffect(Color flashColor, Sprite tempSprite)
    {
        isEffectActive = true;

        if (tempSprite != null) targetZoneImage.sprite = tempSprite;
        targetZoneImage.color = flashColor;

        yield return new WaitForSeconds(effectDuration);

        targetZoneImage.color = Color.white;
        isEffectActive = false;

        UpdatePlayerBaseImage();
    }

    public void ChangeStability(int amount)
    {
        stability += amount;
        stability = Mathf.Clamp(stability, 0, 100);
    }
}
