using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    public Color newColor = Color.red; // Example color, change as needed

    void Start()
    {
        // Get the Renderer component and access the material
        Renderer renderer = GetComponent<Renderer>();

        // If using URP or HDRP with a Lit material
        if (renderer != null && renderer.material.HasProperty("_BaseColor"))
        {
            renderer.material.SetColor("_BaseColor", newColor); // Changes color
        }
    }
}