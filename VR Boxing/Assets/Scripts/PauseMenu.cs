using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;



public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    private bool isPaused;

    public InputActionProperty pauseButton;
    public AudioSource m_MyAudioSource;
    //Value from the slider, and it converts to volume level
    public float m_MySliderValue;
   
    public Slider volume;
    public Slider sfx;
    public Toggle snap_turn;

    void Awake()
    {
        if(pauseMenu != null)
        {
            pauseMenu.SetActive(false);
        }
    }

    private void OnEnable()
    {
        pauseButton.action.performed += OnPauseAction; 
    }
    void Start()
    {
        //Initiate the Slider value to half way
        volume.value = 0.5f;
        sfx.value = 0.5f;
        //Fetch the AudioSource from the GameObject
        m_MyAudioSource = GetComponent<AudioSource>();
        //Play the AudioClip attached to the AudioSource on startup
        //m_MyAudioSource.Play();
    }

    private void OnPauseAction(InputAction.CallbackContext context)
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f; // Pauses most game physics and movement
        pauseMenu.SetActive(true); // Show the menu

        // You may need additional code here to manage the VR cursor visibility 
        // or switch hand models (as Half-Life Alyx does)
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // Resumes game time
        pauseMenu.SetActive(false); // Hide the menu
        // Re-hide the cursor if necessary
    }

}

