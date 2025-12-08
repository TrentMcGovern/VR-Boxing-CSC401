using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    // Array of fruits to spawn
    public GameObject[] fruitToSpawn;

    // Spawn Area
    public float minX = -10f;
    public float maxX = 10f;
    public float minZ = -10f;
    public float maxZ = 10f;
    public float spawnY = 5f; // A fixed Y position

    // Time between spawns/Destroy
    public float spawnRate = 2f;
    public float destroyDelay = 5f;

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        // Infinite loop to keep spawning
        while (true) 
        {
            SpawnObject();
            yield return new WaitForSeconds(spawnRate); // Wait for the specified time
        }
    }

    void SpawnObject()
    {
        //generate random number to determine which fruit spawns
        int randomNum = Random.Range(1, 6);

        // Generate a random position within the range
        float randomX = Random.Range(minX, maxX);
        float randomZ = Random.Range(minZ, maxZ);
        Vector3 randomSpawnPosition = new Vector3(randomX, spawnY, randomZ);

        // Instantiate the fruit at the random position at random rotation
        GameObject spawnedFruit = Instantiate(fruitToSpawn[randomNum], randomSpawnPosition, Random.rotation);

        //Destroy fruit after set delay
        Destroy(spawnedFruit, destroyDelay);
    }
}
