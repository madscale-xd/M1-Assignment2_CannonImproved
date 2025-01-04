using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PillarBehavior : MonoBehaviour
{
    [SerializeField] private float speed = 0f; // Movement speed
    [SerializeField] private float reverseDuration = 0f; // Duration to reverse movement
    [SerializeField] private float flashDuration = 0.1f; // Duration for each flash
    [SerializeField] private int flashCount = 3; // Number of flashes
    [SerializeField] private Color flashColor = Color.red; // Color to flash
    [SerializeField] private float scrollSpeedMultiplier = 2f; // Speed multiplier for scroll input
    [SerializeField] private Vector3 rotationRate = Vector3.zero; // Rotation rate along each axis

    // For shooting balls
    [SerializeField] private GameObject ballPrefab; // Reference to the ball prefab
    [SerializeField] private float shootInterval = 2f; // Time between each shot

    // Range for random Y position
    [SerializeField] private float minY = -1f; // Minimum Y position for spawning
    [SerializeField] private float maxY = 1f; // Maximum Y position for spawning
    private TextManager mngr;

    public float hp = 15;
    private bool isReversing = false;
    private Material objectMaterial; // Reference to the object's material
    private Color originalColor; // Store the original color of the material
    public float GetHP() => hp;

    void Start()
    {
        // Get the object's material and store the original color
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            objectMaterial = renderer.material;
            originalColor = objectMaterial.color;
        }

        // Start shooting balls at intervals
        if (ballPrefab != null)
        {
            StartCoroutine(ShootBalls());
        }

        mngr = GameObject.Find("ManagerCanvas").GetComponent<TextManager>();
    }

    void Update()
    {
        // Move the object forward based on its current rotation
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        // Rotate the pillar based on the rotation rate
        transform.Rotate(rotationRate * Time.deltaTime);

        // Check mouse scroll wheel input and move the pillar accordingly
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scrollInput) > 0.001f)
        {
            // Adjust position based on scroll input
            transform.Translate(Vector3.left * scrollInput * scrollSpeedMultiplier, Space.World);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            hp--;
            mngr.UpdateInfoText();
            if (hp == 0)
            {
                SceneManager.LoadScene("Win");
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

    private void ChangeBaseColor(Color newColor)
    {
        if (objectMaterial == null) return;

        // Determine the property name based on the shader
        string colorProperty = objectMaterial.HasProperty("_BaseColor") ? "_BaseColor" : "_Color";

        // Set the new color
        objectMaterial.SetColor(colorProperty, newColor);
    }

    // Coroutine to shoot balls at intervals
    private IEnumerator ShootBalls()
    {
        while (true)
        {
            if (ballPrefab != null)
            {
                // Generate a random Y position within the specified range
                float randomY = Random.Range(minY, maxY);

                // Set the new position for the ball with random Y value
                Vector3 spawnPosition = new Vector3(transform.position.x, randomY, transform.position.z);

                // Instantiate the ball prefab at the new position
                Instantiate(ballPrefab, spawnPosition, Quaternion.identity);

                // Wait for the shoot interval before spawning the next ball
                yield return new WaitForSeconds(shootInterval);
            }
            else
            {
                yield return null; // Wait until the ballPrefab is assigned
            }
        }
    }
}
