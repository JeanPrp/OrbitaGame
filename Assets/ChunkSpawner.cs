using UnityEngine;

public class ChunkSpawner : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private GameObject[] chunkPrefabs;
    [SerializeField] private float chunkHeight = 12f;
    [SerializeField] private int initialChunks = 4;
    [SerializeField] private float spawnAheadDistance = 28f;

    private float nextSpawnY = 0f;
    private int lastChunkIndex = -1;

    private void Start()
    {
        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }

        for (int i = 0; i < initialChunks; i++)
        {
            SpawnChunk();
        }
    }

    private void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver()) return;
        if (cameraTransform == null) return;

        if (cameraTransform.position.y + spawnAheadDistance > nextSpawnY)
        {
            SpawnChunk();
        }
    }

    private void SpawnChunk()
    {
        if (chunkPrefabs == null || chunkPrefabs.Length == 0) return;

        int index = GetNextChunkIndex();
        GameObject prefab = chunkPrefabs[index];

        Vector3 spawnPos = new Vector3(0f, nextSpawnY, 0f);
        Instantiate(prefab, spawnPos, Quaternion.identity, transform);

        nextSpawnY += chunkHeight;
        lastChunkIndex = index;
    }

    private int GetNextChunkIndex()
    {
        if (chunkPrefabs.Length == 1) return 0;

        int index = Random.Range(0, chunkPrefabs.Length);

        while (index == lastChunkIndex)
        {
            index = Random.Range(0, chunkPrefabs.Length);
        }

        return index;
    }
}