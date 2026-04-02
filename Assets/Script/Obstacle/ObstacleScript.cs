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
            Vector2 contactPoint = collision.GetContact(0).normal;

            if (collision.collider.TryGetComponent<PlayerCarActive>(out var playerCarActive))
            {
                playerCarActive.TakeDamage(damageValue);
                playerCarActive.TakeKnockBack(-contactPoint.x + knockPower);
            }

            Debug.Log("Tabrakan " + contactPoint);
        }
    }
}
