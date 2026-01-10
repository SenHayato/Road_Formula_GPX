using UnityEngine;

public class BoostColliderScript : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] CarModel carModel;

    [Header("Size Setting")]
    [SerializeField] Vector3 isBoostingSize;
    [SerializeField] Vector3 secondBoostSize;

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

    void ColliderSizeSetting()
    {
        if (carModel.isBoosting && !carModel.inSecondBoost)
        {
            transform.localScale = isBoostingSize;
        }
        else
        {
            transform.localScale = secondBoostSize;
        }
    }

    private void Update()
    {
        ColliderSizeSetting();
    }
}
