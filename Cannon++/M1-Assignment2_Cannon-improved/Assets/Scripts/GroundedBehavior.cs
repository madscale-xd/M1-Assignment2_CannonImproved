using System.Collections;
using UnityEngine;

public class GroundedBehavior : MonoBehaviour
{
    [SerializeField] private float speed = 5f; // Movement speed
    [SerializeField] private float reverseDuration = 0.25f; // Duration to reverse movement
    [SerializeField] private float flashDuration = 0.1f; // Duration for each flash
    [SerializeField] private int flashCount = 2; // Number of flashes
    [SerializeField] private Color flashColor = Color.red; // Color to flash

    private float hp = 3;
    private bool isReversing = false;
    private Material objectMaterial; // Reference to the object's material
    private Color originalColor; // Store the original color of the material

    void Start()
    {
        // Get the object's material and store the original color
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            objectMaterial = renderer.material;
            originalColor = objectMaterial.color;
        }
    }

    void Update()
    {
        // Move the object forward based on its current rotation
        transform.Translate(Vector3.left * speed * Time.deltaTime);
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
