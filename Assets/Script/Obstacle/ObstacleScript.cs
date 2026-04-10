using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
    [Header("Obstacle Component")]
    [SerializeField] int damageValue;
    [SerializeField] float knockPower;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            ContactPoint2D contact = collision.GetContact(0);
            Vector2 contactPoint = contact.normal;
            Vector2 contactPointWorld = contact.point;

            if (collision.collider.TryGetComponent<PlayerCarActive>(out var playerCarActive))
            {
                playerCarActive.TakeDamage(damageValue);
                playerCarActive.TakeKnockBack(-contactPoint.x * knockPower);
            }
            VisualEffectManager.Instance.BumpEffect(contactPointWorld);
            SoundManager.Instance.PlayCrashSFX(null);
            //Debug.Log("Tabrakan " + contactPoint);
        }
    }
}
