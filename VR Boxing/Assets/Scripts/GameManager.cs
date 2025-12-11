using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager: MonoBehaviour
{
    [Header("Score Management")]
    public static GameManager Instance;
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

    [Header("Timer Management")]
    [SerializeField]
    private float timer = 120f; // 2 minutes
    private bool timerRunning = true;
    public TextMeshProUGUI timerText;

    [Header("UI")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI finalScoreText;

    [Header("VR")]
    public Transform playerHead;
    public GameObject leftGloveInteractor;
    public GameObject rightGloveInteractor;
    public GameObject leftRayInteractor;
    public GameObject rightRayInteractor;

    private bool isGameOver = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }
    private void Start()
    {
        gameOverPanel.SetActive(false);
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
        StartCoroutine(TimerRoutine());
        UpdateHUD();
    }

    public void AddScore(int amount)
    {
        Score += amount;
        timer += 5f;
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
    void UpdateTimerText()
    {
        if (timerText == null) return;

        int minutes = Mathf.FloorToInt(timer / 60);
        int seconds = Mathf.FloorToInt(timer % 60);

        timerText.text = $"{minutes:00}:{seconds:00}";
    }
    IEnumerator TimerRoutine()
    {
        while (timerRunning)
        {
            timer -= 1f;
            UpdateTimerText();

            if (timer <= 0)
            {
                timer = 0;
                timerRunning = false;
                ShowGameOver();
            }

            yield return new WaitForSeconds(1f);
        }
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
    private void ShowGameOver()
    {
        if (isGameOver) return;
        PauseMenu pm = FindObjectOfType<PauseMenu>();
        if (pm != null)
        {
            pm.pauseMenu.SetActive(false);   // hide pause UI if visible
            pm.enabled = false;              // disable the whole script
        }
        isGameOver = true;

        Time.timeScale = 0f;

        gameOverPanel.SetActive(true);
        gameOverPanel.transform.position = playerHead.position + playerHead.forward * 6f + playerHead.up * 4;
        gameOverPanel.transform.rotation = Quaternion.Euler(0, playerHead.eulerAngles.y, 0);
        SetInteractorsActive(false, true);
        if (finalScoreText != null)
            finalScoreText.text = "Final Score: " + Score;
    }
    private void SetInteractorsActive(bool directActive, bool rayActive)
    {
        leftGloveInteractor.SetActive(directActive);
        rightGloveInteractor.SetActive(directActive);
        leftRayInteractor.SetActive(rayActive);
        rightRayInteractor.SetActive(rayActive);
    }
    public void RestartGame()
    {
        Debug.Log("RESTART BUTTON PRESSED");
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
