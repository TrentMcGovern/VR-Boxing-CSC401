using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleScript : MonoBehaviour
{
    public GameObject deathParticles;
    // Start is called before the first frame update

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision Detected");
        // Optional: Check the tag of the object collided with
        // if (collision.gameObject.CompareTag("Enemy"))
        // {
        // Get the contact point and rotation for correct orientation
        ContactPoint contact = collision.contacts[0];
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 position = contact.point;

        // Instantiate the particle effect at the collision point and rotation
        GameObject effectInstance = Instantiate(deathParticles, position, rotation);

    }
}
