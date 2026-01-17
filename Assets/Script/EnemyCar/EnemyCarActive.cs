using System.Collections;
using UnityEngine;

public class EnemyCarActive : MonoBehaviour
{
    [Header("Enemy Component")]
    float moveSpeed;
    [SerializeField] float maxSpeed;
    [SerializeField] float defaultSpeed;
    [SerializeField] int damageValue;
    [SerializeField] float[] maxSpeedLevel;
    [SerializeField] Rigidbody2D rigid2d;
    [SerializeField] BoxCollider2D boxCollider;
    float carAcceleration;

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
        rigid2d = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        EnemySpeedIncrease();
        rigid2d.gravityScale = 0;
        carAcceleration = carModel.carAcceleration;
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
        maxSpeedLevel[0] = defaultSpeed;
        for (int i = 1; i < gameManager.gameMaxLevel + 1; i++)
        {
            maxSpeedLevel[i] = maxSpeedLevel[i - 1] + 0.6f;
        }
    }

    void MaxSpeedIncrease()
    {
        maxSpeed = maxSpeedLevel[gameManager.gameLevel];
    }

    float maxSpeedNow;
    void CarMoving()
    {
        maxSpeedNow = maxSpeed;

        if (carModel.carSpeed > 0.45f)
        {
            if (!carModel.inAeroMode)
            {
                moveSpeed = -carAcceleration * Time.deltaTime;
            }
            else
            {
                moveSpeed = -(carAcceleration + 1.5f) * Time.deltaTime;
            }
        }
        else
        {
            moveSpeed = 15f * Time.deltaTime;
        }

        if (carModel.isBoosting)
        {
            PlayerBoostSpeed();
        }

        moveSpeed = Mathf.MoveTowards(moveSpeed, Mathf.Clamp(moveSpeed, -10f, maxSpeedNow), 5f * Time.deltaTime);
        transform.position += new Vector3(0f, moveSpeed, 0);
    }

    void PlayerBoostSpeed()
    {
        if (carModel.inSecondBoost)
        {

        }
        else
        {

        }
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
        //Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    bool isTakenDown = false;
    float knockRotateSpeed = 200f;
    float knockAxisX = 20f;
    float knockAxisY = 30f;
    public void TakeDown()
    {
        if (!isTakenDown)
        {
            rigid2d.gravityScale = 1.5f;
            boxCollider.enabled = false;
            transform.position = new(transform.position.x, transform.position.y, -6);
            StartCoroutine(KnockRotation());
            Vector2 knockback = new(Random.Range(-knockAxisX, knockAxisX), knockAxisY);
            rigid2d.AddForce(500f * Time.deltaTime * knockback);
            Invoke(nameof(Explode), Random.Range(1,3));
            isTakenDown = true;
        }
    }

    IEnumerator KnockRotation()
    {
        int clockwiseRot = Random.Range(-1, 2); //random dari -1 sampai 1
        if (clockwiseRot == 0)
        {
            clockwiseRot = 1;
        }

        while (true)
        {
            transform.rotation *= Quaternion.Euler(0, 0, clockwiseRot * knockRotateSpeed * Time.deltaTime);
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
        MaxSpeedIncrease();
        NearMissActive();

        if (!isTakenDown)
        {
            CarMoving();
        }
    }
}
