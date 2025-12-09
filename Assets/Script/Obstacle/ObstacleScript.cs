using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
    [Header("Obstacle Component")]
    [SerializeField] int damageValue;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (collision.collider.TryGetComponent<PlayerCarActive>(out var playerCarActive))
            {
                playerCarActive.TakeDamage(damageValue);
            }
        }
    }
}
