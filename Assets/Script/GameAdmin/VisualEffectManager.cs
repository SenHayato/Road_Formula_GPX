using System.Collections;
using TMPro;
using UnityEngine;

public class VisualEffectManager : MonoBehaviour
{
    [Header("Visual Effect Library")]
    [SerializeField] GameObject[] bumpVisualEffect;
    [SerializeField] GameObject[] explodeVisualEffect;
    [SerializeField] GameObject itemGetVisualEffect;
    [SerializeField] GameObject textPopUp;

    public static VisualEffectManager Instance { get; private set; }

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

    public void ExplodeEffect(Vector3 spawnTransform)
    {
        int visualEffectNum = Random.Range(0, explodeVisualEffect.Length);
        spawnTransform.z = -2f;
        Instantiate(explodeVisualEffect[visualEffectNum], spawnTransform, Quaternion.identity);
    }

    public void ShowTextPop(string textToAssign, Transform spawnToPosition)
    {
        StartCoroutine(TextPopOut(textToAssign, spawnToPosition));
    }

    bool isSpawning = false;

    IEnumerator TextPopOut(string textAssigned, Transform spawnPosition)
    {
        if (isSpawning) yield break;

        isSpawning = true;

        GameObject textPop = Instantiate(textPopUp, spawnPosition.position, Quaternion.identity);
        TextMeshProUGUI textComponent = textPop.GetComponentInChildren<TextMeshProUGUI>();

        if (textComponent != null)
        {
            textComponent.text = textAssigned;
        }

        yield return new WaitForSeconds(0.3f);
        Destroy(textPop);
        isSpawning = false;
    }
}
