using UnityEngine;

public class NearMissScript : MonoBehaviour
{
    //[SerializeField] int nearMissScore;
    [SerializeField] EnemyType enemyType;
    [SerializeField] GameManager gameManager;

    private void Awake()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    bool isNearMiss = false;
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!isNearMiss)
            {
                isNearMiss = true;
                if (collision.TryGetComponent<PlayerCarActive>(out var playerCarActive))
                {
                    playerCarActive.BoostFillNearMiss();
                }

                switch (enemyType)
                {
                    case EnemyType.Car:
                        SoundManager.Instance.PlaySFXOnce(null, "CarPassingBy");
                        break;
                    case EnemyType.Truck:
                        SoundManager.Instance.PlaySFXOnce(null, "Truckpassingby");
                        break;
                }
            }
        }
    }

    private enum EnemyType
    {
        Car, Truck
    }
}
