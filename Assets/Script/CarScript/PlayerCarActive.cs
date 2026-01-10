using System;
using System.Collections;
using UnityEngine;

public class PlayerCarActive : MonoBehaviour
{
    public static PlayerCarActive PlayerActive { get; private set; }

    [Header("Player Car Condition")]
    public bool carExplode = false;

    [Header("Global Player Car Component")]
    [SerializeField] float boostFillValue;
    [SerializeField] CarModel carModel;
    [SerializeField] InputScript inputScript;

    [Header("Car Properties")]
    [SerializeField] SpriteRenderer carRenderer;
    [SerializeField] SpriteRenderer shadowCarRenderer;
    [SerializeField] Rigidbody2D rigid2D;

    [Header("Player Car Level")]
    [SerializeField] int carLevel;
    [SerializeField] float[] carMaxSpeedlevel;

    [Header("Visual Effect")]
    [SerializeField] GameObject[] boosterObj;
    [SerializeField] Animator[] boosterAnim;

    [Header("Reference")]
    [SerializeField] GameManager gameManager;

    void Awake()
    {
        if (PlayerActive != null && PlayerActive != this)
        {
            Destroy(gameObject);
        }
        else
        {
            PlayerActive = this;
        }

        gameManager = FindFirstObjectByType<GameManager>();
        carModel = GetComponentInParent<CarModel>();
        inputScript = FindFirstObjectByType<InputScript>();

        rigid2D = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        //car setup
        carModel.damagePoint = carModel.maxDamagePoint;
        carModel.carFuel = carModel.maxCarFuel;

        StartCoroutine(FillingBoostGauge());
        StartCoroutine(BoostGaugeDepleted());
        StartCoroutine(SecondBoostCountdown());
        MaxSpeedIncrease();

        for (int i = 0; i < boosterObj.Length; i++)
        {
            boosterAnim[i] = boosterObj[i].GetComponent<Animator>();
        }

        Invoke(nameof(CarFuel), 4.2f); //sesuaikan dengan countdown
    }

    void MaxSpeedIncrease()
    {
        carMaxSpeedlevel[0] = carModel.carMaxSpeed;
        for (int i = 1; i < gameManager.gameMaxLevel + 1; i++)
        {
            carMaxSpeedlevel[i] = carMaxSpeedlevel[i-1] + 0.5f;
        }
    }

    void CarLevelConfig()
    {
        carLevel = gameManager.gameLevel;
    }

    public void BoostFillNearMiss()
    {
        //Debug.Log("Near Miss");

        if (carModel.BoostGauge <= carModel.BoostMaxGauge && !carModel.isBoosting)
        {
            carModel.BoostGauge += 20f;
        }
    }

    void CarFuel() //Berperan sebagai timer pada game
    {
        StartCoroutine(CarFuelDepleted());
    }

    public void FuelRefill(float fuelAdd)
    {
        carModel.carFuel += fuelAdd;
    }

    IEnumerator CarFuelDepleted()
    {
        while (true)
        {
            carModel.carFuel -= 1f * Time.deltaTime;
            carModel.carFuel = Mathf.Clamp(carModel.carFuel, 0f, carModel.maxCarFuel);

            yield return null;
        }
    }

    IEnumerator FillingBoostGauge()
    {
        while (true)
        {
            if (carModel.carSpeed > 0f && !carModel.isBoosting)
            {
                carModel.BoostGauge += boostFillValue * Time.deltaTime;
            }

            carModel.BoostGauge = Mathf.Clamp(carModel.BoostGauge, 0f, carModel.BoostMaxGauge);

            yield return null;
        }
    }

    IEnumerator BoostGaugeDepleted()
    {
        while (true)
        {
            if (carModel.isBoosting)
            {
                carModel.BoostGauge -= 7f * Time.deltaTime;
                if (carModel.BoostGauge <= 0)
                {
                    carModel.isBoosting = false;
                    carModel.inSecondBoost = false; //sementara
                    carModel.BoostGauge = 0;
                }
            }
            yield return null;
        }
    }

    float steeringPowerNow;
    void CarSteering()
    {
        Vector2 steerValue = steeringPowerNow * carModel.carSpeed * Time.deltaTime * inputScript.steerAction.ReadValue<Vector2>();

        transform.position += new Vector3(steerValue.x, 0f, 0f);
    }

    float maxSpeedNow;
    void CarAcceleration()
    {
        if (inputScript.gasAction.IsPressed())
        {
            carModel.carSpeed += carModel.carAcceleration * Time.deltaTime;
        }
        else if (inputScript.brakeAction.IsPressed())
        {
            carModel.carSpeed -= carModel.carBraking * Time.deltaTime;
        }
        else
        {
            carModel.carSpeed -= carModel.carDeceleration * Time.deltaTime;
        }

        if (carModel.isBoosting)
        {
            BoostSpeed();
        }
        else
        {
            FormSpeed();
        }

        carModel.carSpeed = Mathf.MoveTowards(carModel.carSpeed, Mathf.Clamp(carModel.carSpeed, 0f, maxSpeedNow), 5f * Time.deltaTime);

        //HUD Monitor
        carModel.maxSpeedUpgraded = maxSpeedNow;
    }

    void FormSpeed()
    {
        if (!carModel.inAeroMode)
        {
            maxSpeedNow = carMaxSpeedlevel[carLevel];
            steeringPowerNow = carModel.carSteeringPower;
        }
        else
        {
            maxSpeedNow = carMaxSpeedlevel[carLevel] + 1.5f;
            steeringPowerNow = carModel.carSteeringPower * 0.5f;
        }
    }

    void BoostSpeed()
    {
        if (carModel.inSecondBoost)
        {
            maxSpeedNow = carModel.secondBoosMaxSpeed;
            steeringPowerNow = 0f;
        }
        else
        {
            //AeroBoost
            maxSpeedNow = carModel.boostMaxSpeed;
            steeringPowerNow = carModel.carSteeringPower * 0.7f;
        }
    }

    void BoostMode()
    {
        if (inputScript.boostAction.triggered && carModel.BoostGauge >= carModel.BoostMaxGauge && !carModel.isBoosting)
        {
            if (carModel.canChangeAero)  //Jika mobil aero maka harus di cek apakah dia dalam aero mode
            {
                if (carModel.inAeroMode)
                {
                    carModel.isBoosting = true;
                    if (carModel.canSecondBoost)
                    {
                        carModel.secondBoostCount = 3.99f;
                    }
                }
            }
            else //jika tidak langsung boost
            {
                carModel.isBoosting = true;
            }
        }

        if (carModel.canSecondBoost)
        {
            SecondBoostMode();
        }
    }

    //void BoostConditon()
    //{
    //    if (carModel.isBoosting)
    //    {
    //        rigid2D.position.y = false;
    //    }
    //}

    bool secondBoostReady = false;
    IEnumerator SecondBoostCountdown()
    {
        if (!carModel.canSecondBoost) yield break;

        while (carModel.canSecondBoost)
        {
            if (carModel.BoostGauge <= 60f && carModel.isBoosting) //60f patokan batas boost aktif untuk diUI
            {
                carModel.secondBoostCount -= 1f * Time.deltaTime;

                if (carModel.secondBoostCount > 0f && carModel.secondBoostCount < 1f)
                {
                    secondBoostReady = true;
                }
                else if (carModel.secondBoostCount < 0f)
                {
                    secondBoostReady = false;
                    carModel.secondBoostCount = 0f;
                }
            }
            yield return null;
        }
    }

    void SecondBoostMode()
    {
        if (secondBoostReady && inputScript.boostAction.triggered)
        {
            if (carModel.isBoosting)
            {
                carModel.inSecondBoost = true;
                secondBoostReady = false;
            }
        }
    }

    void ChangeForm()
    {
        CarFormMonitor();

        if (inputScript.changeFormAction.triggered)
        {
            if (!carModel.inAeroMode)
            {
                carModel.inAeroMode = true;
            }
            else
            {
                carModel.inAeroMode = false;
            }
        }
    }

    void CarFormMonitor()
    {
        if (carModel.inSecondBoost)
        {
            carRenderer.sprite = carModel.SecondBoostForm;
            shadowCarRenderer.sprite = carModel.SecondBoostShadow;
            return;
        }

        if (carModel.isBoosting) return;

        bool aero = carModel.inAeroMode; //jika aero = true (true diwakilkan oleh inAeroMode)
        carRenderer.sprite = aero ? carModel.AeroMode : carModel.CircuitMode;  //jika a = true "maka(?)" kondisi true : kondisi false
        shadowCarRenderer.sprite = aero ? carModel.AeroShadow : carModel.CircuitShadow;
    }

    public void TakeDamage(int damage)
    {
        if (!carExplode && !carModel.isBoosting)
        {
            carModel.damagePoint -= damage;
        }
    }

    void CarExploded()
    {
        if (carModel.damagePoint <= 0)
        {
            carExplode = true;
            carModel.damagePoint = 0; //sementara
        }
    }

    void CarDamageCap()
    {
        if (carModel.damagePoint > carModel.maxDamagePoint)
        {
            carModel.damagePoint = carModel.maxDamagePoint;
        }
    }

    void CarVisualEffect()
    {
        for (int i = 0; i < boosterObj.Length; i++)
        {            
            if (carModel.isBoosting)
            {
                boosterObj[i].SetActive(true);
                boosterAnim[i].enabled = true;
                //boosterAnim[i].SetBool("IsBoosting", true);

                if (!carModel.inSecondBoost)
                {
                    boosterAnim[i].SetBool("SecondBoost", false);
                }
                else
                {
                    boosterAnim[i].SetBool("SecondBoost", true);
                }
            }
            else
            {
                //boosterAnim[i].SetBool("IsBoosting", false);
                boosterAnim[i].enabled = false;
                boosterObj[i].SetActive(false);
            }
        }
    }

    void Update()
    {
        //Setting
        CarLevelConfig();
        CarDamageCap();

        //Visual Effect
        CarVisualEffect();

        //Behaviour
        CarExploded();
        BoostMode();
        if (carModel.canChangeAero)
        {
            ChangeForm();
        }
        CarAcceleration();
        CarSteering();
    }
}
