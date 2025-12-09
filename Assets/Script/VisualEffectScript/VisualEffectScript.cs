using UnityEngine;

public class VisualEffectScript : MonoBehaviour
{
    [Header("Visual Effect")]
    [SerializeField] GameObject windEffect;

    [Header("Reference")]
    [SerializeField] CarModel carModel;

    private void Awake()
    {
        carModel = FindFirstObjectByType<CarModel>();
    }

    void Start()
    {
        
    }

    void WindEffect()
    {
        if (carModel.inSecondBoost)
        {
            windEffect.SetActive(true);
        }
        else
        {
            windEffect.SetActive(false);
        }
    }

    void Update()
    {
        WindEffect();
    }
}
