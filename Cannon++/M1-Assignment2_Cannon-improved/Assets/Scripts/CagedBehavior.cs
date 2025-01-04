using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CagedBehavior : MonoBehaviour
{
    [SerializeField] private float speed = 8f; // Movement speed
    [SerializeField] private float reverseDuration = 0.25f; // Duration to reverse movement
    [SerializeField] private float minOscillationAmplitude = 0.5f; // Minimum amplitude of the sine wave
    [SerializeField] private float maxOscillationAmplitude = 2f; // Maximum amplitude of the sine wave
    [SerializeField] private float oscillationFrequency = 1f; // Frequency of the sine wave
     [SerializeField] private float flashDuration = 0.1f; // Duration for each flash
    [SerializeField] private int flashCount = 2; // Number of flashes
    [SerializeField] private Color flashColor = Color.red; // Color to flash

    private float hp = 2;
    private bool isReversing = false;
    private float initialY; // Store the initial Y position for oscillation
    private float timeElapsed = 0f; // Track elapsed time for sine wave calculation
    private float oscillationAmplitude; // Actual amplitude for this instance

    private Material objectMaterial; // Reference to the object's material
    private Color originalColor; // Store the original color of the material

    // Start is called before the first frame update
    void Start()
    {
        initialY = transform.position.y; // Store the initial Y position

        // Randomize the oscillation amplitude within the specified range
        oscillationAmplitude = Random.Range(minOscillationAmplitude, maxOscillationAmplitude);

        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            objectMaterial = renderer.material;
            originalColor = objectMaterial.color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player is holding down the mouse button or space key
        if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space))
        {
            // Move the object forward based on its current rotation
            transform.Translate(Vector3.left * speed * Time.deltaTime);

            // Update elapsed time
            timeElapsed += Time.deltaTime;

            // Calculate the new Y position using a sine wave
            float newY = initialY + Mathf.Sin(timeElapsed * oscillationFrequency) * oscillationAmplitude;

            // Update the position of the object with the new Y value
            Vector3 newPosition = transform.position;
            newPosition.y = newY;
            transform.position = newPosition;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cannon"))
        {
            Destroy(gameObject);
        }
        else if (other.CompareTag("Ball"))
        {
            hp--;
            if (hp == 0)
            {
                Destroy(gameObject);
            }
            else if (!isReversing)
            {
                StartCoroutine(ReverseMovement());
                 StartCoroutine(FlashColor());
            }
        }
    }

    private IEnumerator ReverseMovement()
    {
        isReversing = true;

        // Reverse the speed
        speed = -speed;

        // Wait for the specified duration
        yield return new WaitForSeconds(reverseDuration);

        // Revert the speed back to normal
        speed = -speed;

        isReversing = false;
    }

    private IEnumerator FlashColor()
    {
        if (objectMaterial == null) yield break;

        for (int i = 0; i < flashCount; i++)
        {
            // Set to flash color
            objectMaterial.color = flashColor;
            yield return new WaitForSeconds(flashDuration);

            // Revert to original color
            objectMaterial.color = originalColor;
            yield return new WaitForSeconds(flashDuration);
        }
    }
}
