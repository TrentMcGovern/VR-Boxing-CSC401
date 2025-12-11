using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager: MonoBehaviour
{
    public static ScoreManager Instance;

    public int Score = 0;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI directionText;

    [Header("Difficulty Thresholds")]
    public int unlockSecondSide = 20;
    public int unlockThirdSide = 50;
    public int unlockFourthSide = 100;

    [Header("Spawning Management")]
    public GameObject[] spawnerSides;
    public float spawnRate = 2.5f;


    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    private void Start()
    {
        if (spawnerSides.Length == 0)
        {
            Debug.LogError("No fruit spawns assigned");
            return;
        }

        foreach (GameObject spawner in spawnerSides)
        {
            spawner.SetActive(false);
        }

        spawnerSides[0].SetActive(true);
        if (directionText != null) directionText.text = "Incoming from: " + spawnerSides[0].name;

        StartCoroutine(SpawnRoutine());
        UpdateHUD();
    }

    public void AddScore(int amount)
    {
        Score += amount;
        UpdateHUD();
        EvaluateDifficulty();
    }

    void EvaluateDifficulty()
    {
        int sidesToActivate = 1;
        if (Score >= unlockFourthSide)
            sidesToActivate = 4;
        else if (Score >= unlockThirdSide)
            sidesToActivate = 3;
        else if (Score >= unlockSecondSide)
            sidesToActivate = 2;




        for (int i = 0; i < spawnerSides.Length; i++)
        {
            // If the current index 'i' is less than the required count, set active.
            spawnerSides[i].SetActive(i < sidesToActivate);
        }
    }

    private void UpdateHUD()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + Score;
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            RandomSpawnObject();
            yield return new WaitForSeconds(spawnRate);
        }
    }
    private void RandomSpawnObject()
    {
        // Find all spawner GameObjects that are currently active in the hierarchy
        List<GameObject> activeSpawners = new List<GameObject>();
        foreach (GameObject spawner in spawnerSides)
        {
            if (spawner.activeInHierarchy)
            {
                activeSpawners.Add(spawner);
            }
        }

        if (activeSpawners.Count == 0) return;

        // Select randomly from the list of currently active spawners
        GameObject chosenSpawnerGO = activeSpawners[Random.Range(0, activeSpawners.Count)];

        RandomSpawner chosenSpawnerScript = chosenSpawnerGO.GetComponent<RandomSpawner>();

        if (chosenSpawnerScript != null)
        {
            chosenSpawnerScript.SpawnObject();
            if (directionText != null) directionText.text = "Incoming from: " + chosenSpawnerGO.name;
        }
    }

}
