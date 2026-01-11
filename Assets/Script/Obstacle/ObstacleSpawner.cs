using System.Collections;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Object to Spawn")]
    [SerializeField] GameObject[] enemyCar;
    [SerializeField] GameObject enemyTruck;
    [SerializeField] GameObject[] powerUps;

    [Header("SpawnerProperties")]
    [SerializeField] Transform[] spawnerPoint; //diurutakan dari spawner pertama
    [SerializeField] float spawnInterval;
    [SerializeField] float timeToSpawn;
    [SerializeField] bool isSpawning = false;

    [Header("SpawnerRates")]
    [SerializeField] int[] carRatesLevel;
    [SerializeField] int[] truckRateLevel;
    [SerializeField] int powerUpRate;

    [Header("Reference")]
    [SerializeField] CarModel carModel;
    //[SerializeField] PlayerCarActive playerCarActive;
    [SerializeField] GameManager gameManager;

    //flag
    bool wasSpawning;

    private void Awake()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        carModel = FindFirstObjectByType<CarModel>();
        //playerCarActive = FindFirstObjectByType<PlayerCarActive>();
    }

    void Start()
    {
        timeToSpawn = spawnInterval;

        StartCoroutine(SpawningSystem());
    }

    IEnumerator SpawningSystem()
    {
        while (true)
        {
            if (carModel.carSpeed > 0.3f)
            {
                timeToSpawn -= Time.deltaTime;

                if (timeToSpawn <= 0)
                {
                    timeToSpawn = 0;
                    SpawningStart();
                    SpawningPowerUp();
                    yield return new WaitForSeconds(1f);
                }
                ResetCooldown();
            }
            yield return null;
        }
    }

    int truckSpawnPosition;
    int CarSpawnPosition;
    void SpawningStart()
    {
        int carRate = Random.Range(0, 100);
        int truckRate = (gameManager.gameLevel >= 1) ? Random.Range(0, 100) : 1;

        SpawningEnemy(carRate, truckRate);
    }

    void SpawningEnemy(int carRates, int truckRates)
    {
        if (carRates <= carRatesLevel[gameManager.gameLevel])
        {
            int carNumber = Random.Range(0, enemyCar.Length);
            CarSpawnPosition = Random.Range(0, spawnerPoint.Length);
            Instantiate(enemyCar[carNumber], spawnerPoint[CarSpawnPosition].position, Quaternion.identity);
        }

        if (truckRates <= truckRateLevel[gameManager.gameLevel])
        {
            do
            {
                truckSpawnPosition = Random.Range(0, spawnerPoint.Length);
            }
            while (truckSpawnPosition == CarSpawnPosition);

            Instantiate(enemyTruck, spawnerPoint[truckSpawnPosition].position, Quaternion.identity);
        }
    }

    void SpawningPowerUp()
    {
        if (Random.Range(0, 100) <= powerUpRate)
        {
            int powerUpItem = Random.Range(0, 2);
            int positionNum = Random.Range(0, spawnerPoint.Length);
            Instantiate(powerUps[powerUpItem], spawnerPoint[positionNum].position, Quaternion.identity);
        }
    }


    void ResetCooldown()
    {
        if (timeToSpawn <= 0)
        {
            timeToSpawn = spawnInterval;
        }
    }

    void Update()
    {

    }
}
