using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class Enemy : MonoBehaviour
{
    [Header("Ýstatistikler")]
    public string enemyName = "Sincap";
    public int health = 5;
    public int attackDamage = 1;
    public bool isStunned = false;

    [Header("UI Referanslarý")]
    public TextMeshProUGUI healthText;
    public Image enemyImage;

    [Header("Ölüm ve Yeni Faz (Animasyonlu)")]
    public GameObject objectToActivate; // Doðuacak yeni karakter
    public float shakeDuration = 0.2f;  // Ekran sallanma süresi
    public float shakeMagnitude = 10f; // Ekran sallanma þiddeti

    [Header("Ses ve Pozisyon")]
    public AudioClip attackSFX;
    private AudioSource audioSource;
    private Vector3 originalPos;
    private Vector3 originalScale;
    private Color originalColor = Color.white;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        originalPos = transform.position;

        if (enemyImage == null) enemyImage = GetComponent<Image>();

        if (enemyImage != null)
        {
            originalColor = enemyImage.color;
            originalScale = enemyImage.rectTransform.localScale;
        }
    }

    void Start() => UpdateHealthUI();

    public void UpdateHealthUI() { if (healthText != null) healthText.text = health.ToString(); }

    public void TakeDamage(int amount)
    {
        health -= amount;
        UpdateHealthUI();

        if (health <= 0)
        {
            Die();
        }
        else
        {
            // Sadece yaþýyorsak parlama efekti yap
            if (gameObject.activeInHierarchy)
            {
                StopCoroutine("FlashColor");
                StartCoroutine(FlashColor(Color.red));
            }
        }
    }

    private void Die()
    {
        Debug.Log(enemyName + " yok edildi, yeni faz baþlýyor!");

        if (objectToActivate != null)
        {
            // Yeni objeyi açmadan önce ekraný salla (Kamera objesi silinmediði için güvenli)
            StartCoroutine(SimpleScreenShake());
            objectToActivate.SetActive(true);
        }

        // Objeyi hiyerarþiden hemen pasif yap ve yok et
        gameObject.SetActive(false);
        Destroy(gameObject);
    }

    IEnumerator SimpleScreenShake()
    {
        Transform cam = Camera.main.transform;
        Vector3 camOriginalPos = cam.localPosition;
        float elapsed = 0.0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            cam.localPosition = new Vector3(camOriginalPos.x + x, camOriginalPos.y + y, camOriginalPos.z);
            elapsed += Time.deltaTime;
            yield return null;
        }
        cam.localPosition = camOriginalPos;
    }

    public void ApplyStunVisuals()
    {
        isStunned = true;
        if (enemyImage != null)
        {
            enemyImage.rectTransform.localScale = new Vector3(originalScale.x, -originalScale.y, originalScale.z);
            enemyImage.color = new Color(0.5f, 0.5f, 0.5f);
        }
    }

    public void ResetVisuals()
    {
        // Güvenlik: Obje silinmiþse çalýþma
        if (this == null || enemyImage == null) return;

        enemyImage.rectTransform.localScale = originalScale;
        enemyImage.color = originalColor;
    }

    public void TriggerHealEffect()
    {
        if (gameObject.activeInHierarchy)
        {
            StopCoroutine("FlashColor");
            StartCoroutine(FlashColor(Color.green));
        }
    }

    IEnumerator FlashColor(Color c)
    {
        if (enemyImage != null)
        {
            enemyImage.color = c;
            yield return new WaitForSeconds(0.4f);

            // KRÝTÝK: Bekleme süresinden sonra obje hala var mý kontrol et
            if (this != null && enemyImage != null)
            {
                enemyImage.color = isStunned ? new Color(0.5f, 0.5f, 0.5f) : originalColor;
            }
        }
    }

    public IEnumerator VisualAttack(PlayerStats player)
    {
        // 1. Kontrol: Baþlangýçta yaþýyor muyuz?
        if (this == null || !gameObject.activeInHierarchy) yield break;

        if (isStunned)
        {
            yield return new WaitForSeconds(0.5f);
            if (this != null) // Beklemeden sonra yaþýyor muyuz?
            {
                isStunned = false;
                ResetVisuals();
            }
            yield break;
        }

        Vector3 targetPos = player.transform.position;
        float elapsed = 0f;

        // Ýleri Hareket
        while (elapsed < 0.2f)
        {
            if (this == null) yield break; // Silindiysek coroutine'i durdur
            transform.position = Vector3.Lerp(originalPos, targetPos, elapsed / 0.2f);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Vuruþ Aný
        if (this == null) yield break;
        if (audioSource != null && attackSFX != null) audioSource.PlayOneShot(attackSFX);
        player.TakeDamage(attackDamage);

        // Geri Dönüþ
        elapsed = 0f;
        while (elapsed < 0.2f)
        {
            if (this == null) yield break; // Silindiysek coroutine'i durdur
            transform.position = Vector3.Lerp(targetPos, originalPos, elapsed / 0.2f);
            elapsed += Time.deltaTime;
            yield return null;
        }

        if (this != null) transform.position = originalPos;
    }
}