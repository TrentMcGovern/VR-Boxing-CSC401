using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FruitVFX : MonoBehaviour
{


    public GameObject deathParticles;
    // Start is called before the first frame update

    void Start()
    {
       
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ContactPoint contact = collision.contacts[0];
            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, contact.normal);
            Vector3 position = contact.point;
            // Instantiate the particle effect at the collision point and rotation
            GameObject effectInstance = Instantiate(deathParticles, position, rotation);

            Destroy(this.gameObject, .1f);
        }

    }



}

