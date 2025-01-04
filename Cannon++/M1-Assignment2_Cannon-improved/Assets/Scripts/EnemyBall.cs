using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBall : MonoBehaviour
{
    [SerializeField] private float homingSpeed = 5f; // Speed of homing movement
    private GameObject cannonObject; // Reference to the "Cannon" object

    void Start()
    {
        // Find the object with the "Cannon" tag
        cannonObject = GameObject.FindGameObjectWithTag("Cannon");

        if (cannonObject == null)
        {
            Debug.LogWarning("Cannon object not found!");
        }
    }

    void Update()
    {
        if (cannonObject != null)
        {
            // Move the ball towards the "Cannon" object
            Vector3 direction = (cannonObject.transform.position - transform.position).normalized;

            // Move the ball in the direction of the cannon with a speed factor
            transform.Translate(direction * homingSpeed * Time.deltaTime, Space.World);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cannon"))
        {
            // Destroy the ball when it collides with the "Cannon"
            Destroy(gameObject);
        }
        else if (other.CompareTag("Ball"))
        {
            // Destroy the ball if it collides with another "Ball"
            Destroy(gameObject);
        }
    }
}
