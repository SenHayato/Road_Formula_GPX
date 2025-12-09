using UnityEngine;

public class NearMissScript : MonoBehaviour
{
    //[SerializeField] int nearMissScore;
    [SerializeField] GameManager gameManager;

    private void Awake()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.TryGetComponent<PlayerCarActive>(out var playerCarActive))
            {
                playerCarActive.BoostFillNearMiss();
            }
        }
    }
}
