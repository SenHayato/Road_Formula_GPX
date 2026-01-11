using UnityEngine;

public class BoostColliderScript : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] CarModel carModel;

    private void Awake()
    {
        carModel = GetComponentInParent<CarModel>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("Kena Musuh Boost");
            int BumpGenerator = Random.Range(0, 2);
            if (collision.TryGetComponent<EnemyCarActive>(out var enemyCarActive))
            {
                if (BumpGenerator == 3)
                {
                    enemyCarActive.Explode();
                }
                else
                {
                    enemyCarActive.TakeDown();
                }
            }
        }
    }
}
