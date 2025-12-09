using UnityEngine;

public class ScrollOnceObj : MonoBehaviour
{ 
    [Header("Component")]
    [SerializeField] Renderer scrollingMaterial;
    [SerializeField] float startOffsetValue;
    [SerializeField] float stopOffsetValue;
    private ScrollingBackgroud scrollingBackgroud;
    private float rollingValue;

    void Awake()
    {
        scrollingBackgroud = FindFirstObjectByType<ScrollingBackgroud>();
        scrollingMaterial = GetComponent<Renderer>();
    }

    private void OnEnable()
    {
        rollingValue = startOffsetValue; // posisi awal offset
        scrollingMaterial.material.mainTextureOffset = new Vector2(0f, rollingValue);
    }

    void MovingDown()
    {
        if (scrollingBackgroud.isRolling)
        {
            rollingValue += scrollingBackgroud.speed * Time.deltaTime;
        }

        scrollingMaterial.material.mainTextureOffset = new Vector2(0f, rollingValue);
    }

    void DisableObject()
    {
        if (scrollingMaterial.material.mainTextureOffset.y >= stopOffsetValue)
        {
            gameObject.SetActive(false);
        }
    }

    void Update()
    {
        MovingDown();
        DisableObject();
    }
}
