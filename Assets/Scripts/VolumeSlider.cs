using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private string volumeParameter = "MasterVolume";

    public void SetVolume(float sliderValue)
    {
        // Konsola yazdýrýp kontrol edelim:
        Debug.Log("Slider Deðeri: " + sliderValue);

        float volume = Mathf.Log10(sliderValue) * 20;

        if (sliderValue <= 0.0001f) volume = -80f;

        myMixer.SetFloat(volumeParameter, volume);
    }
}