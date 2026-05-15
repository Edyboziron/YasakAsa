using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [Header("Audio Sources")]
    public AudioSource normalMusicSource;
    public AudioSource glitchMusicSource;

    [Header("Ayarlar")]
    public float fadeSpeed = 0.5f; // Geçiþ hýzý
    private float targetNormalVolume = 1f;
    private float targetGlitchVolume = 0f;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // Baþlangýçta normal müzik çalsýn, diðeri sessiz olsun
        normalMusicSource.volume = 1f;
        glitchMusicSource.volume = 0f;

        normalMusicSource.Play();
        glitchMusicSource.Play();
    }

    void Update()
    {
        // Ses seviyelerini hedef deðerlere doðru yumuþakça kaydýr (Fade Effect)
        normalMusicSource.volume = Mathf.MoveTowards(normalMusicSource.volume, targetNormalVolume, fadeSpeed * Time.deltaTime);
        glitchMusicSource.volume = Mathf.MoveTowards(glitchMusicSource.volume, targetGlitchVolume, fadeSpeed * Time.deltaTime);
    }

    // PlayerStats üzerinden bu fonksiyon çaðrýlacak
    public void SwitchMusic(bool isGlitched)
    {
        if (isGlitched)
        {
            targetNormalVolume = 0f;
            targetGlitchVolume = 1f;
        }
        else
        {
            targetNormalVolume = 1f;
            targetGlitchVolume = 0f;
        }
    }
}