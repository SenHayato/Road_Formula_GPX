using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Slider")]
    [SerializeField] Slider boostSlider;
    [SerializeField] Slider secondBoostSlider;
    [SerializeField] Slider damageBar;
    [SerializeField] Slider fuelBar;

    [Header("Text Info")]
    [SerializeField] GameObject countdownObj;
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI speedText;
    public TextMeshProUGUI lenghtText;
    [SerializeField] TextMeshProUGUI damagePoint;
    [SerializeField] TextMeshProUGUI fuelPoint;
    [SerializeField] TextMeshProUGUI boostCountdown;

    [Header("Refenrence")]
    [SerializeField] CarModel carModel;

    void Awake()
    {
        carModel = FindFirstObjectByType<CarModel>();
    }

    void Start()
    {
        StartCoroutine(SpeedometerIndicator());
        if (!carModel.canSecondBoost)
        {
            boostCountdown.enabled = false;
        }
    }

    void BarMonitor()
    {
        //AeroBoost Monitor
        boostSlider.value = carModel.BoostGauge;
        boostSlider.maxValue = carModel.BoostMaxGauge;

        if (carModel.canSecondBoost)
        {
            //SecondBoost Monitor
            secondBoostSlider.maxValue = carModel.BoostMaxGauge;
            secondBoostSlider.value = carModel.secondBoostGauge;
        }

        //DamagePoint Monitor
        damageBar.value = carModel.damagePoint;
        damageBar.maxValue = carModel.maxDamagePoint;
        damagePoint.text = carModel.damagePoint.ToString() + " %";

        //fuelMonitor
        int carFuelValue = (int) carModel.carFuel;

        fuelBar.value = carFuelValue;
        fuelBar.maxValue = (int)carModel.maxCarFuel;
        fuelPoint.text = carFuelValue.ToString() + " %";
    }

    void BarColorMonitor()
    {
        return;
    }

    void BoostMonitor()
    {
        if (carModel.canSecondBoost && carModel.isBoosting)
        {
            if (carModel.BoostGauge <= 60f)
            {
                if (!carModel.inSecondBoost)
                {
                    boostCountdown.enabled = true;
                    BoostCountDown();
                }
                else
                {
                    Invoke(nameof(DisableCountDown), 0.5f);
                }
            }
        }
        else
        {
            boostCountdown.enabled = false;
            return;
        }
    }

    void BoostCountDown()
    {
        int secondBoost = (int) carModel.secondBoostCount;
        if (carModel.secondBoostCount > 1)
        {
            boostCountdown.text = secondBoost.ToString();
            boostCountdown.color = Color.white;
        }
        else if (carModel.secondBoostCount > 0f && carModel.secondBoostCount < 1f)
        {
            boostCountdown.text = "0       boost";
            boostCountdown.color = Color.red;
        }
    }

    void DisableCountDown()
    {
        boostCountdown.enabled = false;
    }

    IEnumerator SpeedometerIndicator()
    {
        while (true)
        {
            int carSpeedometer = (int)(carModel.carSpeed * 100f);
            speedText.text = carSpeedometer.ToString() + " Km/H";
            yield return null;
        }
    }

    public void HideCountdown()
    {
        Invoke(nameof(CountdownHide), 1.2f);
    }

    void CountdownHide()
    {
        countdownObj.SetActive(false);
    }

    void Update()
    {
        BoostMonitor();
        BarMonitor();
        BarColorMonitor();
    }
}
