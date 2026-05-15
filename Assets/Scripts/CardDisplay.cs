using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardDisplay : MonoBehaviour
{
    public CardData cardData; // ScriptableObject dosyanýz

    [Header("UI Bileþenleri")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI stabilityCostText; // Eskiden manaCost idi
    public TextMeshProUGUI valueText;
    public Image artworkImage;

    void Start()
    {
        UpdateCardVisuals();
    }

    // Public olmalý ki CardSlot içinden çaðrýlabilsin
    public void UpdateCardVisuals()
    {
        if (cardData == null) return;

        if (nameText) nameText.text = cardData.cardName;
        if (descriptionText) descriptionText.text = cardData.description;
        if (valueText) valueText.text = cardData.value.ToString();
        if (artworkImage) artworkImage.sprite = cardData.artwork;

        // CardData içindeki ismi hala manaCost olabilir, sorun deðil
        if (stabilityCostText) stabilityCostText.text = cardData.manaCost.ToString();

        // Eðer stabilite düþükse kart ismi titreyebilir (Opsiyonel)
        // Bu efekti CardMovement içine de ekleyebiliriz.
    }
}