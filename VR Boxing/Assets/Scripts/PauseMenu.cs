using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class PauseMenu : MonoBehaviour
{
    public AudioSource m_MyAudioSource;
    //Value from the slider, and it converts to volume level
    public float m_MySliderValue;
    public GameObject pause;
    public Slider volume;
    public Slider sfx;
    public Toggle snap_turn;


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

}

