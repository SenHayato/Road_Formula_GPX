using UnityEngine;

public class LoopPropertiManager : MonoBehaviour
{
    [Header("Trigger")]
    //[SerializeField] bool spawnObjRight = false;
    //[SerializeField] bool spawnObjLeft = false;
    [SerializeField] float spawnInterval;
    [SerializeField] float coolingDownMeter;

    [Header("Object Properties")]
    [SerializeField] GameObject[] rightObjects;
    [SerializeField] GameObject[] leftObjects;

    [Header("Reference")]
    [SerializeField] ScrollingBackgroud scrollingBackgroud;

    private void Awake()
    {
        scrollingBackgroud = FindFirstObjectByType<ScrollingBackgroud>();
    }
    void Start()
    {
        coolingDownMeter = spawnInterval;
    }

    //bool isCoolingDown = true;
    bool canSpawn = false;
    int rightObjNum;
    int leftObjNum;

    void TimerCooldown()
    {
        if (coolingDownMeter > 0)
        {
            spawnerReady = false;
        }
        else
        {
            spawnerReady = true;
        }
    }

    bool spawnerReady = false;
    void SpawningCooldown()
    {
        if (!spawnerReady)
        {
            coolingDownMeter -= Time.deltaTime;
            canSpawn = false;
        }
        else
        {
            rightObjNum = Random.Range(0, 7);
            leftObjNum = Random.Range(0, 7);
            coolingDownMeter = spawnInterval;
            canSpawn = true;
        }
    }

    void SpawningObject()
    {
        if (canSpawn)
        {
            if (rightObjNum <= 2) //tidak lebih dari 3 object
            {
                RightSpawn(rightObjNum);
            }

            if (leftObjNum <= 2)
            {
                LeftSpawn(leftObjNum);
            }
        }
        else
        {
            return;
        }
    }

    void RightSpawn(int objectNumber)
    {
        rightObjects[objectNumber].SetActive(true);
    }

    void LeftSpawn(int objectNumber)
    {
        leftObjects[objectNumber].SetActive(true);
    }

    void Update()
    {
        if (scrollingBackgroud.isRolling)
        {
            TimerCooldown();
            SpawningCooldown();
            SpawningObject();
        }
    }
}
