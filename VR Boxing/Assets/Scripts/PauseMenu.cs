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
        // You can set slider defaults here if needed
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

        // DO NOT pause XR input or tracking with timeScale=0
        Time.timeScale = 0f;

        pauseMenu.SetActive(true);

        // Position UI in front of the player’s eyes
        Vector3 forwardFlat = Vector3.ProjectOnPlane(playerHead.forward, Vector3.up).normalized;
        pauseMenu.transform.position = playerHead.position + forwardFlat * 5f + Vector3.up * 3;
        pauseMenu.transform.rotation = Quaternion.LookRotation(forwardFlat);
    }
    
    public void ResumeGame()
    {
        isPaused = false;

        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
    }
}