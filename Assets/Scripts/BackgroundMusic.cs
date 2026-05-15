using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    // Bu statik deðiþken, sahnede sadece bir tane müzikçalar olmasýný garantiler.
    public static BackgroundMusic Instance;

    private void Awake()
    {
        // Eðer daha önce yaratýlmýþ bir müzik kutusu yoksa...
        if (Instance == null)
        {
            Instance = this;
           
        }
        else
        {
            // Eðer zaten çalan bir müzik kutusu varsa (örneðin ana menüye geri döndün),
            // bu yeni oluþan fazlalýðý hemen yok et.
            Destroy(gameObject);
        }
    }
}