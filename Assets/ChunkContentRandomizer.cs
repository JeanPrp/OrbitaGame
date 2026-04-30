using System.Collections.Generic;
using UnityEngine;

public class ChunkContentRandomizer : MonoBehaviour
{
    [Header("Planetas")]
    [SerializeField] private Transform planetSpawnPoint;
    [SerializeField] private GameObject[] possiblePlanets;

    [Header("Moedas")]
    [SerializeField] private Transform[] coinSpawnPoints;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private int minCoinsToSpawn = 2;
    [SerializeField] private int maxCoinsToSpawn = 5;

    [Header("Obstáculos")]
    [SerializeField] private Transform[] hazardSpawnPoints;
    [SerializeField] private GameObject[] possibleHazards;
    [SerializeField] private int minHazardsToSpawn = 0;
    [SerializeField] private int maxHazardsToSpawn = 2;

    private void Start()
    {
        SpawnPlanet();
        SpawnCoins();
        SpawnHazards();
    }

    private void SpawnPlanet()
    {
        if (planetSpawnPoint == null || possiblePlanets == null || possiblePlanets.Length == 0)
            return;

        int index = Random.Range(0, possiblePlanets.Length);
        Instantiate(possiblePlanets[index], planetSpawnPoint.position, Quaternion.identity, transform);
    }

    private void SpawnCoins()
    {
        if (coinPrefab == null || coinSpawnPoints == null || coinSpawnPoints.Length == 0)
            return;

        int amount = Random.Range(minCoinsToSpawn, maxCoinsToSpawn + 1);
        amount = Mathf.Min(amount, coinSpawnPoints.Length);

        List<int> usedIndexes = new List<int>();

        for (int i = 0; i < amount; i++)
        {
            int randomIndex = GetUniqueRandomIndex(coinSpawnPoints.Length, usedIndexes);
            usedIndexes.Add(randomIndex);

            Instantiate(coinPrefab, coinSpawnPoints[randomIndex].position, Quaternion.identity, transform);
        }
    }

    private void SpawnHazards()
    {
        if (possibleHazards == null || possibleHazards.Length == 0 || hazardSpawnPoints == null || hazardSpawnPoints.Length == 0)
            return;

        int amount = Random.Range(minHazardsToSpawn, maxHazardsToSpawn + 1);
        amount = Mathf.Min(amount, hazardSpawnPoints.Length);

        List<int> usedIndexes = new List<int>();

        for (int i = 0; i < amount; i++)
        {
            int spawnIndex = GetUniqueRandomIndex(hazardSpawnPoints.Length, usedIndexes);
            usedIndexes.Add(spawnIndex);

            int hazardIndex = Random.Range(0, possibleHazards.Length);

            Instantiate(
                possibleHazards[hazardIndex],
                hazardSpawnPoints[spawnIndex].position,
                Quaternion.identity,
                transform
            );
        }
    }

    private int GetUniqueRandomIndex(int max, List<int> usedIndexes)
    {
        int tries = 0;
        int index = Random.Range(0, max);

        while (usedIndexes.Contains(index) && tries < 50)
        {
            index = Random.Range(0, max);
            tries++;
        }

        return index;
    }
}