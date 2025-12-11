using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [Header("UI")]
    public GameObject pauseMenu;
    public Slider volume;
    public Slider sfx;
    public Toggle snap_turn;

    [Header("VR")]
    public Transform playerHead;
    public InputActionProperty pauseButton;
    public GameObject leftGloveInteractor;  
    public GameObject rightGloveInteractor; 
    public GameObject leftRayInteractor;   
    public GameObject rightRayInteractor;  

    [Header("Audio")]
    public AudioSource m_MyAudioSource;

    private bool isPaused = false;

    private void Awake()
    {
        if (pauseMenu != null)
            pauseMenu.SetActive(false);

        if (m_MyAudioSource == null)
            m_MyAudioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        pauseButton.action.performed += OnPauseAction;
    }

    private void OnDisable()
    {
        pauseButton.action.performed -= OnPauseAction;
    }

    private void Start()
    {
        pauseMenu.SetActive(false);
        // Ensure gloves are active and rays are inactive at start
        SetInteractorsActive(true, false);
    }

    private void OnPauseAction(InputAction.CallbackContext context)
    {
        // Prevent multiple toggles from a single press
        if (!context.performed) return;

        if (isPaused)
            ResumeGame();
        else
            PauseGame();


    }

    public void PauseGame()
    {
        isPaused = true;

 
        Time.timeScale = 0f;

        pauseMenu.SetActive(true);

        // Position UI in front of the player’s eyes
        pauseMenu.transform.position = playerHead.position + playerHead.forward * 6f + playerHead.up * 4;
        pauseMenu.transform.rotation = Quaternion.Euler(0, playerHead.eulerAngles.y, 0);

        SetInteractorsActive(false, true);
    }

    public void ResumeGame()
    {

        isPaused = false;
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        SetInteractorsActive(true, false);

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