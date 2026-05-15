using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class CardMovement : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public CardSlot mySlot;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private CardDisplay cardDisplay;
    private Transform originalParent;
    private int originalSiblingIndex;
    private Vector3 originalLocalPosition;
    private PlayerStats player;
    private AudioSource audioSource;
    private LayoutElement layoutElement;
    private bool hasLayoutGroup; // Parent'ta Layout Group var mý?

    [Header("Prefablar ve Sesler")]
    public GameObject fireballProjectilePrefab;
    public AudioClip grabSFX; public AudioClip invalidSFX; public AudioClip attackSFX;
    public AudioClip healSFX; public AudioClip stabilitySFX; public AudioClip shieldSFX; public AudioClip stunSFX;

    private bool isAnimating = false;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        cardDisplay = GetComponent<CardDisplay>();
        audioSource = GetComponent<AudioSource>();
        layoutElement = GetComponent<LayoutElement>();
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.GetComponent<PlayerStats>();
        GetCanvas();
    }

    private void GetCanvas() { if (canvas == null) canvas = GetComponentInParent<Canvas>() ?? FindObjectOfType<Canvas>(); }

    public void OnBeginDrag(PointerEventData e)
    {
        if (isAnimating || TurnManager.Instance != null && (!TurnManager.Instance.isPlayerTurn || TurnManager.Instance.cardsPlayedThisTurn >= TurnManager.Instance.maxCardsPerTurn)) { e.pointerDrag = null; return; }
        GetCanvas(); PlayLocalSound(grabSFX);

        originalParent = transform.parent;
        originalSiblingIndex = transform.GetSiblingIndex();
        originalLocalPosition = transform.localPosition;

        // Parent'ta Layout Group var mý kontrol et
        hasLayoutGroup = (originalParent != null &&
                         (originalParent.GetComponent<HorizontalLayoutGroup>() != null ||
                          originalParent.GetComponent<VerticalLayoutGroup>() != null ||
                          originalParent.GetComponent<GridLayoutGroup>() != null));

        // Layout Group varsa ignore layout aktif et
        if (hasLayoutGroup && layoutElement != null)
        {
            layoutElement.ignoreLayout = true;
        }

        if (canvas != null)
        {
            transform.SetParent(canvas.transform);
            transform.SetAsLastSibling();
        }
        canvasGroup.blocksRaycasts = false; canvasGroup.alpha = 0.6f;
    }

    public void OnDrag(PointerEventData e) => transform.position = Input.mousePosition;

    public void OnEndDrag(PointerEventData e)
    {
        GameObject target = GetObjectUnderMouse();
        canvasGroup.blocksRaycasts = true; canvasGroup.alpha = 1f;
        if (target != null && player != null && cardDisplay?.cardData != null)
        {
            Enemy enemy = target.GetComponentInParent<Enemy>();
            PlayerTargetZone pZone = target.GetComponentInParent<PlayerTargetZone>();
            CardType type = cardDisplay.cardData.cardType;

            if ((type == CardType.Attack || type == CardType.Stun) && enemy != null) ApplyEffect(enemy, null);
            else if ((type == CardType.Heal || type == CardType.RestoreStability || type == CardType.Shield) && pZone != null) ApplyEffect(null, player);
            else ReturnToHand();
        }
        else ReturnToHand();
    }

    private void ApplyEffect(Enemy enemyTarget, PlayerStats playerTarget)
    {
        if (player == null || cardDisplay == null || cardDisplay.cardData == null) return;

        CardType type = cardDisplay.cardData.cardType;
        int power = cardDisplay.cardData.value;
        int manaCost = cardDisplay.cardData.manaCost;

        PlayEffectSound(type);

        bool willGlitch = (player.stability < 50 && type != CardType.RestoreStability);

        player.ChangeStability(-manaCost);

        if (willGlitch)
        {
            Debug.Log("<color=red>GLITCH TETÝKLENDÝ!</color>");
            ApplyGlitchEffect(enemyTarget, playerTarget);
        }
        else
        {
            if (type == CardType.Attack && enemyTarget != null)
            {
                LaunchFireball(enemyTarget, power);
            }
            else if (type == CardType.Stun && enemyTarget != null)
            {
                enemyTarget.ApplyStunVisuals();
                FinishCardAction();
            }
            else
            {
                ApplyNormalEffect(enemyTarget, playerTarget);
                FinishCardAction();
            }
        }
    }

    private void LaunchFireball(Enemy target, int damage)
    {
        GameObject proj = Instantiate(fireballProjectilePrefab, player.transform.position, Quaternion.identity);
        proj.transform.SetParent(canvas.transform, true);
        proj.transform.SetAsLastSibling();
        Vector3 p = proj.transform.position; p.z = 0; proj.transform.position = p;
        proj.transform.localScale = Vector3.one;
        proj.GetComponent<FireballProjectile>()?.Setup(target.transform, (player.stability < 50 ? damage / 2 : damage));
        FinishCardAction();
    }

    private void ApplyNormalEffect(Enemy enemy, PlayerStats self)
    {
        int p = cardDisplay.cardData.value; CardType t = cardDisplay.cardData.cardType;
        if (self != null)
        {
            if (t == CardType.Heal) { self.health += p; self.TriggerHealEffect(); }
            else if (t == CardType.RestoreStability) self.ChangeStability(p);
            else if (t == CardType.Shield) self.armor += p;
        }
    }

    private void ApplyGlitchEffect(Enemy enemy, PlayerStats self)
    {
        int p = cardDisplay.cardData.value; int r = Random.Range(0, 3);
        if (self != null)
        {
            if (r == 0) StartCoroutine(VisualCardBackfire(player.transform, p / 2));
            else if (r == 1)
            {
                Enemy[] ens = Object.FindObjectsByType<Enemy>(FindObjectsSortMode.None);
                if (ens.Length > 0) StartCoroutine(VisualCardTransfer(ens[Random.Range(0, ens.Length)].transform, p));
                else FinishCardAction();
            }
            else { player.ChangeStability(-15); FinishCardAction(); }
        }
        else if (enemy != null) { enemy.TakeDamage(p / 2); player.TakeDamage(2); FinishCardAction(); }
    }

    IEnumerator VisualCardBackfire(Transform t, int d)
    {
        isAnimating = true;
        while (t != null && Vector3.Distance(transform.position, t.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, t.position, 1500f * Time.deltaTime);
            yield return null;
        }
        player.TakeDamage(d);
        FinishCardAction();
    }

    IEnumerator VisualCardTransfer(Transform t, int a)
    {
        isAnimating = true;
        while (t != null && Vector3.Distance(transform.position, t.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, t.position, 1200f * Time.deltaTime);
            yield return null;
        }
        Enemy e = t.GetComponentInParent<Enemy>();
        if (e != null)
        {
            e.health += a;
            e.UpdateHealthUI();
            e.TriggerHealEffect();
        }
        FinishCardAction();
    }

    private void FinishCardAction()
    {
        if (TurnManager.Instance != null) TurnManager.Instance.RecordCardPlayed();
        if (mySlot != null) mySlot.SpawnCard();
        Destroy(gameObject);
    }

    private void ReturnToHand()
    {
        PlayLocalSound(invalidSFX);
        if (originalParent != null)
        {
            transform.SetParent(originalParent);
            transform.SetSiblingIndex(originalSiblingIndex);

            if (hasLayoutGroup)
            {
                // Layout Group varsa: ignore layout kapat ve layout'u yenile
                if (layoutElement != null) layoutElement.ignoreLayout = false;
                LayoutRebuilder.ForceRebuildLayoutImmediate(originalParent.GetComponent<RectTransform>());
            }
            else
            {
                // Layout Group yoksa: manuel pozisyona dön
                transform.localPosition = originalLocalPosition;
            }
        }
    }

    private void PlayLocalSound(AudioClip c) { if (c != null && audioSource != null) audioSource.PlayOneShot(c); }
    private void PlayEffectSound(CardType t) { AudioClip c = t switch { CardType.Attack => attackSFX, CardType.Heal => healSFX, CardType.RestoreStability => stabilitySFX, CardType.Shield => shieldSFX, CardType.Stun => stunSFX, _ => null }; if (c != null) AudioSource.PlayClipAtPoint(c, Camera.main.transform.position, 1f); }
    private GameObject GetObjectUnderMouse() { PointerEventData pd = new PointerEventData(EventSystem.current) { position = Input.mousePosition }; List<RaycastResult> res = new List<RaycastResult>(); EventSystem.current.RaycastAll(pd, res); foreach (var r in res) if (r.gameObject != gameObject && !r.gameObject.transform.IsChildOf(transform)) if (r.gameObject.GetComponentInParent<Enemy>() != null || r.gameObject.GetComponentInParent<PlayerTargetZone>() != null) return r.gameObject; return null; }
}