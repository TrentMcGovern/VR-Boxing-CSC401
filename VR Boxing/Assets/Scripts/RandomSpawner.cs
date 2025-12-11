using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    // Array of fruits to spawn
    [Header("Game Objects")]
    [SerializeField]
    private GameObject[] fruitToSpawn;
    [SerializeField] 
    private Transform targetCenter;
    [SerializeField] 
    private float landingOffset = 5f;
    [SerializeField]
    private Transform fruitSpawnerPos;


    // Spawn Area
    [Header("Spawn Area Bounds")]
    [SerializeField]
    private float minX = -50;
    [SerializeField]
    private float maxX = 50;
    [SerializeField]
    private float minZ = 0;
    [SerializeField]
    private float maxZ = 0;
    [SerializeField]
    private float minY = 0; 
    [SerializeField]
    private float maxY = 0;


    [Header("Fruit Spawn Rate and Death Timer")]
    [SerializeField]
    // Time between spawns/Destroy
    private float spawnRate = 2.5f;
    [SerializeField]
    private float destroyDelay = 8f;

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

    // math/physics very complicated
    public static Vector3 CalculateLaunchVelocity(Vector3 start, Vector3 target, float angle)
    {
        float gravity = Mathf.Abs(Physics.gravity.y);

        // Horizontal displacement
        Vector3 horizontal = new Vector3(target.x - start.x, 0, target.z - start.z);
        float distance = horizontal.magnitude;

        // Height difference
        float heightDifference = target.y - start.y;

        float rad = angle * Mathf.Deg2Rad;

        // Velocity magnitude formula
        float velocitySquared =
            (gravity * distance * distance) /
            (2 * (distance * Mathf.Tan(rad) - heightDifference) * Mathf.Pow(Mathf.Cos(rad), 2));

        if (velocitySquared < 0)
            return Vector3.zero; // no valid solution

        float velocity = Mathf.Sqrt(velocitySquared);

        // Create velocity vector
        Vector3 launchVelocity = horizontal.normalized * velocity * Mathf.Cos(rad);
        launchVelocity.y = velocity * Mathf.Sin(rad);

        return launchVelocity;
    }
    void SpawnObject()
    {
        int randomNum = Random.Range(0, fruitToSpawn.Length);

        float randomX = Random.Range(minX, maxX);
        float randomZ = Random.Range(minZ, maxZ);
        float randomY = Random.Range(minY, maxY);

        Vector3 randomSpawnPosition = new Vector3(
            fruitSpawnerPos.position.x + randomX,
            fruitSpawnerPos.position.y + randomY,
            fruitSpawnerPos.position.z + randomZ
        );

        GameObject spawnedFruit = Instantiate(
            fruitToSpawn[randomNum],
            randomSpawnPosition,
            Random.rotation
        );

        // Target is center of ring with variation
        Vector3 targetPos = targetCenter.position;
        targetPos.x += Random.Range(-landingOffset, landingOffset);
        targetPos.z += Random.Range(-landingOffset, landingOffset);
        Vector3 v0 = CalculateLaunchVelocity(randomSpawnPosition, targetPos, 45f);
        Rigidbody rb = spawnedFruit.GetComponent<Rigidbody>();
        rb.velocity = v0;
        Destroy(spawnedFruit, destroyDelay);
    }
}
