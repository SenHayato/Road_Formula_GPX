using UnityEngine;

public class ScrollingBackgroud : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] CarModel carModel;

    [Header("Component")]
    [SerializeField] Renderer[] scrollingMaterials;
    public float speed;
    [SerializeField] float rollingValue;
    public bool isRolling; //sesuaikan dengan status mobil, master properti dari semua envi scrolling

    void Awake()
    {
        scrollingMaterials = GetComponentsInChildren<Renderer>();
        carModel = FindFirstObjectByType<CarModel>();
    }

    void MatchTheSpeed()
    {
        speed = carModel.carSpeed;

        if (speed <= 0)
        {
            isRolling = false;
        }
        else
        {
            isRolling = true;
        }
    }

    void RollingBackground()
    {
        if (isRolling)
        {
            rollingValue += speed * Time.deltaTime;
        }
        //else
        //{
        //    rollingValue -= 0.2f * Time.deltaTime;
        //}

        if (rollingValue >= 1f || rollingValue <= -1f)
        {
            rollingValue = 0;
        }

        foreach (Renderer renderer in scrollingMaterials)
        {
            renderer.material.mainTextureOffset = new Vector2(0, rollingValue);
        }
    }

    // Update is called once per frame
    void Update()
    {
        MatchTheSpeed();
        RollingBackground();
    }
}
