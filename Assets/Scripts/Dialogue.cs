using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class Dialogue : MonoBehaviour
{
    [Header("UI Bileþenleri")]
    public TextMeshProUGUI textComponent; // Ana Diyalog Yazýsý
    public TextMeshProUGUI nameComponent; // Ýsim Yazýsý (Örn: Anlatýcý)
    public GameObject dialogueWindow;     // Kapanacak Panel

    [Header("Ayarlar")]
    [TextArea(2, 5)]
    public string[] lines;    // Cümlelerin
    public float textSpeed = 0.05f;
    public string sceneToLoad; // Gidilecek Sahne (AnaEkran)

    private int index;

    void Start()
    {
        textComponent.text = string.Empty;
        if (nameComponent != null) nameComponent.text = string.Empty;
        StartDialogue();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Eðer yazý hala yazýlýyorsa tamamla, bittiyse sonrakine geç
            if (textComponent.text == GetContentOnly(lines[index]))
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = GetContentOnly(lines[index]);
            }
        }
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        // 1. Satýrý Analiz Et (Ýsim var mý?)
        string fullLine = lines[index];
        string name = "";
        string content = "";

        if (fullLine.Contains(":"))
        {
            // "Anlatýcý: Merhaba" -> Ýkiye böl
            string[] parts = fullLine.Split(new char[] { ':' }, 2);
            name = parts[0].Trim();      // "Anlatýcý"
            content = parts[1].Trim();   // "Merhaba"
        }
        else
        {
            // Ýsim yoksa direkt yaz
            content = fullLine;
        }

        // 2. Ýsmi Ekrana Yaz (Varsa)
        if (nameComponent != null)
            nameComponent.text = name;

        // 3. Ýçeriði Daktilo Gibi Yaz
        textComponent.text = "";
        foreach (char c in content.ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    // Yardýmcý fonksiyon: Sadece konuþma metnini almak için (týklayýnca tamamlamak için lazým)
    string GetContentOnly(string line)
    {
        if (line.Contains(":"))
        {
            return line.Split(new char[] { ':' }, 2)[1].Trim();
        }
        return line;
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        if (dialogueWindow != null) dialogueWindow.SetActive(false);
        else gameObject.SetActive(false);

        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}