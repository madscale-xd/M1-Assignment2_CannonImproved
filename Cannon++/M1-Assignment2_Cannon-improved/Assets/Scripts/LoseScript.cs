using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseScript : MonoBehaviour
{
    [SerializeField] public float hp = 5; // Player's health
    [SerializeField] private GameObject cannon; // The Cannon GameObject
    [SerializeField] private Color flashColor = Color.red; // The color to flash
    [SerializeField] private float flashDuration = 0.1f; // Duration of each flash
    [SerializeField] private int flashCount = 2; // Number of flashes

    private TextManager mngr;
    private Material cannonMaterial; // Reference to the Cannon's material
    private Color originalColor; // Store the original color of the Cannon's material

    void Start()
    {
        // Get the TextManager
        mngr = GameObject.Find("ManagerCanvas").GetComponent<TextManager>();

        // Get the Cannon's material and store its original color
        if (cannon != null)
        {
            Renderer renderer = cannon.GetComponent<Renderer>();
            if (renderer != null)
            {
                cannonMaterial = renderer.material;
                originalColor = cannonMaterial.color;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.enabled = false;
            hp--;
            mngr.UpdateHPText();
            Debug.Log("You got damaged");

            // Trigger the red flicker on the Cannon
            if (cannonMaterial != null)
            {
                StartCoroutine(FlashCannon());
            }

            if (hp <= 0)
            {
                Destroy(gameObject);
                SceneManager.LoadScene("Lose");
            }
        }
    }

    private IEnumerator FlashCannon()
    {
        for (int i = 0; i < flashCount; i++)
        {
            // Set to flash color
            cannonMaterial.color = flashColor;
            yield return new WaitForSeconds(flashDuration);

            // Revert to original color
            cannonMaterial.color = originalColor;
            yield return new WaitForSeconds(flashDuration);
        }
    }
}
