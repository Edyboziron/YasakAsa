using UnityEngine;
using UnityEngine.EventSystems;

public class InfoPanelStatic : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Panel Ayarý")]
    public GameObject infoPanel; // Açýlacak olan sabit panel

    void Start()
    {
        // Baþlangýçta panelin kapalý olduðundan emin ol
        if (infoPanel != null)
            infoPanel.SetActive(false);
    }

    // Mouse üzerine geldiðinde
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (infoPanel != null)
        {
            infoPanel.SetActive(true);

            // Eðer panel baþka UI elemanlarýnýn arkasýnda kalýyorsa en öne getirir
            infoPanel.transform.SetAsLastSibling();
        }
    }

    // Mouse üzerinden çekildiðinde
    public void OnPointerExit(PointerEventData eventData)
    {
        if (infoPanel != null)
        {
            infoPanel.SetActive(false);
        }
    }

    // Obje yok edilirse (Kart oynanýrsa veya düþman ölürse) panel açýk kalmasýn
    void OnDisable()
    {
        if (infoPanel != null)
            infoPanel.SetActive(false);
    }
}