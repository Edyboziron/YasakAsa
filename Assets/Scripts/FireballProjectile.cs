using UnityEngine;
using System.Collections;

public class FireballProjectile : MonoBehaviour
{
    private Transform target;
    private int damage;
    private float travelDuration = 0.4f;
    private Vector3 startPos;
    private bool isMoving = false;

    public void Setup(Transform targetEnemy, int damageAmount)
    {
        target = targetEnemy;
        damage = damageAmount;

        // Mermi fareyi engellemesin
        CanvasGroup cg = GetComponent<CanvasGroup>();
        if (cg == null) cg = gameObject.AddComponent<CanvasGroup>();
        cg.blocksRaycasts = false;
        cg.interactable = false;

        // Baþlangýç pozisyonunu HEMEN kaydet
        startPos = transform.position;

        // Hareketi baþlat
        isMoving = true;
        StartCoroutine(MoveRoutine());
    }

    IEnumerator MoveRoutine()
    {
        float elapsed = 0f;

        // Hedef kaybolana veya süre dolana kadar hareket et
        while (elapsed < travelDuration && target != null && isMoving)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / travelDuration;

            // Lerp ile hareket (pozisyon her frame güncelleniyor)
            transform.position = Vector3.Lerp(startPos, target.position, t);

            yield return null;
        }

        // Hedefe ulaþtýk mý?
        if (target != null)
        {
            Enemy enemy = target.GetComponentInParent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }

        // Kendini yok et
        Destroy(gameObject);
    }

    // Güvenlik için - obje devre dýþý býrakýlýrsa coroutine'i durdur
    private void OnDisable()
    {
        isMoving = false;
        StopAllCoroutines();
    }
}