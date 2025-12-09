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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            int BumpGenerator = Random.Range(0, 2);
            if (collision.collider.TryGetComponent<EnemyCarActive>(out var enemyCarActive))
            {
                if (BumpGenerator == 0)
                {
                    enemyCarActive.Explode();
                }
                else
                {
                    StartCoroutine(enemyCarActive.TakeDown());
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
