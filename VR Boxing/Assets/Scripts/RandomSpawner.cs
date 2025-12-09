using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    // Array of fruits to spawn
    [Header("Game Objects")]
    [SerializeField]
    private GameObject[] fruitToSpawn;
    [SerializeField] private Transform targetCenter;
    [SerializeField] private float landingOffset = .5f;
    [SerializeField]
    private Transform fruitSpawnerPos;

    [Header("Fruit Initial Spawn Speed")]
    [SerializeField]
    private float fruitSpeed = 35f;
    [SerializeField]
    private float arcHeight = .1f;

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
    private float spawnRate = 2f;
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

        Vector3 direction = (targetPos - randomSpawnPosition).normalized;

        // Add arc (very complicated)
        Vector3 arced = direction + Vector3.up * arcHeight;
        arced.Normalize();
        Rigidbody rb = spawnedFruit.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 start = randomSpawnPosition;
            Vector3 end = targetPos;

            float gravity = Mathf.Abs(Physics.gravity.y);
            float speed = fruitSpeed;

            Vector3 toTarget = end - start;
            Vector3 toTargetXZ = new Vector3(toTarget.x, 0f, toTarget.z);

            float xzDist = toTargetXZ.magnitude;
            float yDist = toTarget.y;

            float v2 = speed * speed;

            float discriminant = (v2 * v2) - gravity * (gravity * xzDist * xzDist + 2f * yDist * v2);

            Vector3 velocity;

            if (discriminant >= 0f)
            {
                float root = Mathf.Sqrt(discriminant);

                // LOW ARC
                float angleLow = Mathf.Atan((v2 - root) / (gravity * xzDist));

                // HIGH ARC
                float angleHigh = Mathf.Atan((v2 + root) / (gravity * xzDist));

                // Blend between the two using arcHeight (0 = flat, 1 = high)
                float t = Mathf.Clamp01(arcHeight);
                float angle = Mathf.Lerp(angleLow, angleHigh, t);

                Vector3 dirXZ = toTargetXZ.normalized;

                velocity =
                    dirXZ * Mathf.Cos(angle) * speed +
                    Vector3.up * Mathf.Sin(angle) * speed;
            }
            else
            {
                // If impossible (too far), shoot straight
                velocity = toTarget.normalized * speed;
            }

            rb.velocity = velocity;
        }


        Destroy(spawnedFruit, destroyDelay);
    }
}
