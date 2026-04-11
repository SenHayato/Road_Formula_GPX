using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyCarActive : MonoBehaviour
{
    [Header("Enemy Component")]
    [SerializeField] EnemyType enemyType;
    [SerializeField] float moveSpeed;
    [SerializeField] float maxSpeed;
    [SerializeField] float defaultSpeed;
    [SerializeField] int damageValue;
    [SerializeField] float[] maxSpeedLevel;
    [SerializeField] Rigidbody2D rigid2d;
    [SerializeField] BoxCollider2D boxCollider;
    float carAcceleration;

    [Header("Enemy Status")]
    [SerializeField] bool canTurn;
    [SerializeField] bool isKnocked = false;
    [SerializeField] bool canKnocked;

    [Header("Visual Effect")]
    [SerializeField] GameObject explosionEffect;

    [Header("Reference")]
    [SerializeField] CarModel carModel;
    [SerializeField] PlayerCarActive playerCarActive;
    [SerializeField] GameManager gameManager;
    [SerializeField] NearMissScript nearMissScript;
    [SerializeField] AudioSource audioSource;

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
            maxSpeedLevel[i] = maxSpeedLevel[i - 1] + 0.5f;
        }
    }

    void MaxSpeedIncrease()
    {
        maxSpeed = maxSpeedLevel[gameManager.gameLevel];
    }

    [SerializeField] float maxSpeedNow;
    void CarMoving()
    {
        if (carModel.isBoosting)
        {
            PlayerBoostSpeed();
        }
        else
        {
            maxSpeedNow = -maxSpeed;
        }

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


        moveSpeed = Mathf.MoveTowards(moveSpeed, Mathf.Clamp(moveSpeed, -10f, maxSpeedNow), 5f * Time.deltaTime);
        transform.position += new Vector3(0f, moveSpeed, 0);
    }

    void PlayerBoostSpeed()
    {
        if (carModel.inSecondBoost)
        {
            maxSpeedNow = -(carModel.secondBoostMaxSpeed + 5f);
        }
        else
        {
            maxSpeedNow = -(carModel.boostMaxSpeed + 2f);
        }
    }

    void TakeKnockBack(float knockedPower)
    {
        if (!isKnocked)
        {
            isKnocked = true;
            StartCoroutine(KnockBack(knockedPower));
        }
    }

    IEnumerator KnockBack(float knockPower)
    {
        if (!isKnocked) yield break;

        Vector3 knockDirection = new Vector3(knockPower, 0f, 0f);
        float duration = 0.2f;
        float timer = 0f;

        while (timer < duration)
        {
            transform.position += 1.5f * Time.deltaTime * knockDirection;
            timer += Time.deltaTime;
            yield return null;
        }

        isKnocked = false;
    }

     float knockValue;
     int direction;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            ContactPoint2D contact = collision.GetContact(0);

            Vector2 contactPoint = contact.normal;
            Vector2 contactPointWorld = contact.point;

            if (collision.collider.TryGetComponent<PlayerCarActive>(out var playerCarActive))
            {
                playerCarActive.TakeDamage(damageValue);

                if (canKnocked)
                {
                    if (contactPoint.x != 0)
                    {
                        knockValue = contactPoint.x * 2.5f;
                    }
                    else if (contactPoint.y != 0)
                    {
                        if (contactPoint.y == -1)
                        {
                            direction = -1;
                        }
                        else
                        {
                            direction = 1;
                        }
                        knockValue = direction * 2.5f;
                    }

                    TakeKnockBack(knockValue);
                    playerCarActive.TakeKnockBack(-knockValue);
                }
                //Debug.Log("Enemy car Contact " + contactPoint + " Knock Value " + knockValue);
                //Debug.Log("Posisi tabrakan " + contactPointWorld);
                VisualEffectManager.Instance.BumpEffect(contactPointWorld);
                SoundManager.Instance.PlayCrashSFX(audioSource);
            }
        }
        else if (collision.collider.CompareTag("Wall"))
        {
            Explode();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Border"))
        {
            Destroy(gameObject);
        }
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
            SoundManager.Instance.PlayCrashSFX(audioSource);
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

    public void Explode()
    {
        //Instantiate(explosionEffect, transform.position, Quaternion.identity);
        VisualEffectManager.Instance.ExplodeEffect(transform.position);
        SoundManager.Instance.PlayCrashSFX(audioSource);
        Destroy(gameObject);
    }

    int scoreToAdd;
    private void OnDestroy()
    {
        if (isKnocked || isTakenDown)
        {
            switch (enemyType)
            {
                case EnemyType.Car:
                    scoreToAdd = 500;
                    break;
                case EnemyType.Truck:
                    scoreToAdd = 1000;
                    break;
            }
            gameManager.ScoreAdd(scoreToAdd);
        }
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

    private enum EnemyType
    {
        Car, Truck
    }
}
