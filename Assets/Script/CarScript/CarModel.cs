using UnityEngine;

public class CarModel : MonoBehaviour
{
    [Header("Car Profile")]
    public string CarName;
    public bool canChangeAero; //optional
    public bool inAeroMode = false;

    [Header("Car Condition")]
    public int damagePoint;
    public int maxDamagePoint;

    [Header("CarFuel")]
    public float carFuel;
    public float maxCarFuel;

    [Header("CarPeformance")]
    public float carSpeed; 
    public float carMaxSpeed; //saat aero mode ditambah nilai nya, tiap naik tingkat kecepatan maxspeed bertambah
    public float maxSpeedUpgraded;
    public float carAcceleration;
    public float carDeceleration;
    public float carBraking;
    public float carSteeringPower; //saat aero mode dikurangi nilai nya

    [Header("FirstBooster")]
    public float BoostMaxGauge; //normalnya 100, max boost digunnakan untuk second boost juga
    public float BoostGauge; //update
    public bool isBoosting;
    public float boostMaxSpeed;
    public float secondBoosMaxSpeed;

    [Header("SecondBoost (Optional)")]
    public float secondBoostCount; //countdown sebelum second boost dengan default 3.99 detik
    public bool canSecondBoost; //optional
    public float secondBoostGauge; //optional
    public bool inSecondBoost; //optional

    [Header("CarSprite")]
    public Sprite CircuitMode;
    public Sprite CircuitShadow;
    public Sprite AeroMode;
    public Sprite AeroShadow;
    public Sprite SecondBoostForm;
    public Sprite SecondBoostShadow;

    [Header("BoostEffect")]
    public GameObject boostObj;
    public GameObject secondBoostObj;
}
