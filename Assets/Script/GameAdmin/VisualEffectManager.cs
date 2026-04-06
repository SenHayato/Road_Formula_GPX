using UnityEngine;

public class VisualEffectManager : MonoBehaviour
{
    [Header("Visual Effect Library")]
    [SerializeField] GameObject[] bumpVisualEffect;
    [SerializeField] GameObject[] explodeVisualEffect;
    [SerializeField] GameObject itemGetVisualEffect;

    public static VisualEffectManager Instance  {  get; private set; }

    private void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void BumpEffect(Vector3 spawnTransform)
    {
        int visualEffectNum = Random.Range(0, bumpVisualEffect.Length);
        spawnTransform.z = -2f;
        Instantiate(bumpVisualEffect[visualEffectNum], spawnTransform, Quaternion.identity);
    }

    public void ItemGetEffect(Vector2 spawnTransform)
    {
        Instantiate(itemGetVisualEffect, spawnTransform, Quaternion.identity);
    }
}
