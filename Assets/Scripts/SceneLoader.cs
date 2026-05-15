using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Statik değişken: Nereden geldiğimizi hafızada tutar
    public static string sonOynananSahne;

    public void SahneDegistir(string sahneAdi)
    {
        SceneManager.LoadScene(sahneAdi);
    }

    // Öldüğün an bunu çağır
    public void KaybetmeEkraninaGit()
    {
        sonOynananSahne = SceneManager.GetActiveScene().name; // Geldiğin sahneyi kaydet
        SceneManager.LoadScene("KaybetmeSahnesi_Adini_Yaz"); // Kaybetme ekranına git
    }

    // "Tekrar Dene" butonuna bunu bağla
    public void TekrarDene()
    {
        if (!string.IsNullOrEmpty(sonOynananSahne))
            SceneManager.LoadScene(sonOynananSahne); // Kaydettiğimiz sahneye geri dön
        else
            SceneManager.LoadScene("AnaOyunSahnesi"); // Hata olursa başa dön
    }

    public void OyundanCik()
    {
        Application.Quit();
    }
}