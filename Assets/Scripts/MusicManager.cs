using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [Header("Audio Sources")]
    public AudioSource normalMusicSource;
    public AudioSource glitchMusicSource;

    [Header("Settings")]
    public float fadeSpeed = 0.5f; // Transition speed
    private float targetNormalVolume = 1f;
    private float targetGlitchVolume = 0f;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // Start with normal music playing and glitched track muted
        normalMusicSource.volume = 1f;
        glitchMusicSource.volume = 0f;

        normalMusicSource.Play();
        glitchMusicSource.Play();
    }

    void Update()
    {
        // Smoothly fade audio levels towards target volumes
        normalMusicSource.volume = Mathf.MoveTowards(normalMusicSource.volume, targetNormalVolume, fadeSpeed * Time.deltaTime);
        glitchMusicSource.volume = Mathf.MoveTowards(glitchMusicSource.volume, targetGlitchVolume, fadeSpeed * Time.deltaTime);
    }

    // Called by PlayerStats when stability drops below threshold or recovers
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
