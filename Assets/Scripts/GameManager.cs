using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    // Statik deðiþken: Oyun kapanýnca sýfýrlanýr, sahneler arasý geçiþte korunur.
    public static int currentSessionLevel = 0;

    [Header("Sistem Baðlantýsý")]
    public TypewriterManager typewriter;

    [Header("Arka Plan Resimleri")]
    public Image backgroundImage;
    public Sprite bgIntro, bgLevel1_2, bgLevel3_4, bgLevel5;

    [Header("Hikaye Dosyalarý")]
    public DialogueData introData, level1Data, level2Data, level3Data, level4Data, level5Data;

    [Header("Gidilecek Oyun Sahneleri")]
    public string sahne_Level1Sonrasi;
    public string sahne_Level2Sonrasi;
    public string sahne_Level3Sonrasi;
    public string sahne_Level4Sonrasi;
    public string sahne_Level5Sonrasi;

    void Start()
    {
        StartCoroutine(ResumeFromLastPoint(currentSessionLevel));
    }

    IEnumerator ResumeFromLastPoint(int levelIndex)
    {
        yield return new WaitForSeconds(0.5f);
        switch (levelIndex)
        {
            case 0: StartIntro(); break;
            case 1: StartLevel1(); break;
            case 2: StartLevel2(); break;
            case 3: StartLevel3(); break;
            case 4: StartLevel4(); break;
            case 5: StartLevel5(); break;
            default: StartIntro(); break;
        }
    }

    public void StartIntro()
    {
        SetBackground(bgIntro);
        typewriter.StartDialogue(introData, () => {
            // ÝSTEDÝÐÝN GÝBÝ: Intro bitince sahne deðiþmez, direkt Level 1 baþlar.
            StartLevel1();
        });
    }

    public void StartLevel1() { SetBackground(bgLevel1_2); typewriter.StartDialogue(level1Data, () => SessionSaveAndLoad(2, sahne_Level1Sonrasi)); }
    public void StartLevel2() { SetBackground(bgLevel1_2); typewriter.StartDialogue(level2Data, () => SessionSaveAndLoad(3, sahne_Level2Sonrasi)); }
    public void StartLevel3() { SetBackground(bgLevel3_4); typewriter.StartDialogue(level3Data, () => SessionSaveAndLoad(4, sahne_Level3Sonrasi)); }
    public void StartLevel4() { SetBackground(bgLevel3_4); typewriter.StartDialogue(level4Data, () => SessionSaveAndLoad(5, sahne_Level4Sonrasi)); }
    public void StartLevel5() { SetBackground(bgLevel5); typewriter.StartDialogue(level5Data, () => SessionSaveAndLoad(0, sahne_Level5Sonrasi)); }

    void SetBackground(Sprite sprite) { if (backgroundImage != null) backgroundImage.sprite = sprite; }

    void SessionSaveAndLoad(int nextLevelIndex, string sceneName)
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            currentSessionLevel = nextLevelIndex;
            SceneManager.LoadScene(sceneName);
        }
    }
}