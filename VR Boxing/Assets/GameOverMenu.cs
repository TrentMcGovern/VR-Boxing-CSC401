using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class GameOverMenu : MonoBehaviour
{
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

    private void Awake()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    private void Start()
    {
        gameOverPanel.SetActive(false);
    }

    public void ShowGameOver(int finalScore)
    {
        if (isGameOver) return;

        isGameOver = true;

        Time.timeScale = 0f;

        gameOverPanel.SetActive(true);
        gameOverPanel.transform.position = playerHead.position + playerHead.forward * 6f + playerHead.up * 4;
        gameOverPanel.transform.rotation = Quaternion.Euler(0, playerHead.eulerAngles.y, 0);
        SetInteractorsActive(false, true);
        if (finalScoreText != null)
            finalScoreText.text = "Final Score: " + finalScore;

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
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}



