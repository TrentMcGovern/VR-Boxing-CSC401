using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FruitVFX : MonoBehaviour
{


    public GameObject deathParticles;
    // Start is called before the first frame update

    public AudioClip hitSound; // Assign in Inspector
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>(); // Gets the AudioSource on this GameObject
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Collision Detected");
            // Optional: Check the tag of the object collided with
            // if (collision.gameObject.CompareTag("Enemy"))
            // {
            // Get the contact point and rotation for correct orientation
            audioSource.PlayOneShot(hitSound, 0.5f);
            ContactPoint contact = collision.contacts[0];
            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, contact.normal);
            Vector3 position = contact.point;


            // Instantiate the particle effect at the collision point and rotation
            GameObject effectInstance = Instantiate(deathParticles, position, rotation);

        }

    }



}

