using System.Collections;
using UnityEngine;

public class EnemyCarActive : MonoBehaviour
{
    [Header("Enemy Component")]
    [SerializeField] float moveSpeed;
    [SerializeField] float defaultSpeed;
    [SerializeField] int damageValue;
    [SerializeField] float[] speedLevel;

    [Header("Enemy Status")]
    [SerializeField] bool canTurn;
    [SerializeField] bool isExploded = false;
    [SerializeField] bool isBumped = false;

    [Header("Visual Effect")]
    [SerializeField] GameObject explosionEffect;

    [Header("Reference")]
    [SerializeField] CarModel carModel;
    [SerializeField] PlayerCarActive playerCarActive;
    [SerializeField] GameManager gameManager;
    [SerializeField] NearMissScript nearMissScript;

    void Awake()
    {
        nearMissScript = GetComponentInChildren<NearMissScript>();
        gameManager = FindFirstObjectByType<GameManager>();
        carModel = FindFirstObjectByType<CarModel>();
        playerCarActive = FindAnyObjectByType<PlayerCarActive>();
    }

    private void Start()
    {
        EnemySpeedIncrease();
    }

    void NearMissActive()
    {
        if (carModel.isBoosting)
        {
            nearMissScript.enabled = false;
        }
        else
        {
            nearMissScript.enabled = true;
        }
    }

    void EnemySpeedIncrease()
    {
        speedLevel[0] = defaultSpeed;
        for (int i = 1; i < gameManager.gameMaxLevel + 1; i++)
        {
            speedLevel[i] = speedLevel[i - 1] + 0.6f;
        }
    }
    
    void CarMoving()
    {
        float carMoving;
        moveSpeed = speedLevel[gameManager.gameLevel];
        if (carModel.carSpeed > 0.45f)
        {
            carMoving = -moveSpeed * Time.deltaTime;
        }
        else
        {
            carMoving = (moveSpeed + 15f) * Time.deltaTime;
        }

        transform.position += new Vector3(0f, carMoving, 0);
    }

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Border"))
        {
            Destroy(gameObject);
        }
    }

    public void Explode()
    {
        Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public IEnumerator TakeDown()
    {
        while (true)
        {
            yield return null;
        }
    }

    private void OnDestroy()
    {
        carModel = null;
        playerCarActive = null;
    }

    void Update()
    {
        CarMoving();
        NearMissActive();
    }
}
