using UnityEngine;

public class PowerUpScript : MonoBehaviour
{
    [Header("Power UP Properties")]
    [SerializeField] int RepairValue;
    [SerializeField] float moveSpeed;
    [SerializeField] bool canTurn;

    [Header("Reference")]
    [SerializeField] CarModel carModel;

    void Awake()
    {
        carModel = FindFirstObjectByType<CarModel>();
    }

    void PowerUpMoving()
    {
        float powerMoving;
        if (carModel.carSpeed > 0)
        {
            powerMoving = -moveSpeed * Time.deltaTime;
        }
        else
        {
            powerMoving = (moveSpeed + 15f) * Time.deltaTime;
        }

        transform.position += new Vector3(0f, powerMoving, 0);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            carModel.damagePoint += RepairValue;
            Destroy(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

    //destroy ketika border
    private void OnDestroy()
    {
        carModel = null;
    }

    private void Update()
    {
        PowerUpMoving();
    }
}
