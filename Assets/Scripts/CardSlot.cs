using UnityEngine;

public class CardSlot : MonoBehaviour
{
    [Header("Bu Yuvaya Özel Kart")]
    public GameObject cardPrefab;

    void Start()
    {
        SpawnCard();
    }

    public void SpawnCard()
    {
        if (cardPrefab == null) return;

        // Kartý oluþtur
        GameObject newCard = Instantiate(cardPrefab, transform);

        // UI Boyutlarýný sýfýrla
        RectTransform rt = newCard.GetComponent<RectTransform>();
        if (rt != null)
        {
            rt.localScale = Vector3.one;
            rt.anchoredPosition = Vector2.zero;
            rt.localPosition = Vector3.zero;
        }

        // Görselleri güncelle
        CardDisplay display = newCard.GetComponent<CardDisplay>();
        if (display != null)
        {
            display.UpdateCardVisuals();
        }

        // Karta bu yuvayý tanýt
        CardMovement movement = newCard.GetComponent<CardMovement>();
        if (movement != null)
        {
            movement.mySlot = this;
        }
    }
}