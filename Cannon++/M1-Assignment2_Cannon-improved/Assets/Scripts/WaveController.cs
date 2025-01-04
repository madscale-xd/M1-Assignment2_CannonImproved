using System.Collections;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    // Serialized fields to control the durations of each coroutine
    [SerializeField] private float waveOneDuration = 2f;
    [SerializeField] private float waveTwoDuration = 3f;
    [SerializeField] private float waveThreeDuration = 4f;

    // Prefabs to instantiate
    [SerializeField] private GameObject waveOnePrefab;
    [SerializeField] private GameObject waveTwoPrefab;
    [SerializeField] private GameObject waveThreePrefab;

    [SerializeField] private TextManager mngr;
    [SerializeField] private Spawner spawn1;
    [SerializeField] private Spawner spawn2;

    public float wave;

    // Start is called before the first frame update
    void Start()
    {
        // Start the coroutines at the beginning
        StartCoroutine(PlayWaves());
    }

    // Coroutine for wave one behavior
    public IEnumerator WaveOne()
    {
        wave = 1;
        mngr.UpdateWaveText();

        // Reset spawn intervals for Wave 1
        spawn1.SetMinSpawnInterval(3);  // Set appropriate values for wave 1
        spawn1.SetMaxSpawnInterval(5);

        Debug.Log("Wave One Started");

        if (waveOnePrefab != null)
        {
            spawn1 = Instantiate(waveOnePrefab, new Vector3(82.77f, 1.6f, 0.04f), Quaternion.identity).GetComponent<Spawner>();
        }

        yield return new WaitForSeconds(waveOneDuration);

        // Destroy all enemies before proceeding
        DestroyAllEnemies();

        Debug.Log("Wave One Ended");
    }

    public IEnumerator WaveTwo()
    {
        wave = 2;
        mngr.UpdateWaveText();

        // Reset spawn intervals for Wave 2
        spawn1.SetMinSpawnInterval(5);  // Set appropriate values for wave 2
        spawn1.SetMaxSpawnInterval(8);

        spawn2.SetMinSpawnInterval(8);  // Set appropriate values for wave 2
        spawn2.SetMaxSpawnInterval(12);

        Debug.Log("Wave Two Started");

        if (waveTwoPrefab != null)
        {
            spawn2 = Instantiate(waveTwoPrefab, new Vector3(82.17f, 8.48f, 0.04f), Quaternion.identity).GetComponent<Spawner>();
        }

        yield return new WaitForSeconds(waveTwoDuration);

        // Destroy all enemies before proceeding
        DestroyAllEnemies();

        Debug.Log("Wave Two Ended");
    }

    public IEnumerator WaveThree()
    {
        wave = 3;
        mngr.UpdateWaveText();

        // Reset spawn intervals for Wave 3
        spawn1.SetMinSpawnInterval(8);  // Set appropriate values for wave 3
        spawn1.SetMaxSpawnInterval(12);

        spawn2.SetMinSpawnInterval(15);  // Set appropriate values for wave 3
        spawn2.SetMaxSpawnInterval(20);

        Debug.Log("Wave Three Started");

        if (waveThreePrefab != null)
        {
            Instantiate(waveThreePrefab, new Vector3(130.5f, 44.5f, -0.7f), Quaternion.identity);
        }

        yield return new WaitForSeconds(waveThreeDuration);

        // Destroy all enemies before proceeding
        DestroyAllEnemies();

        Debug.Log("Wave Three Ended");
    }


    private IEnumerator PlayWaves()
    {
        yield return StartCoroutine(WaveOne());
        yield return StartCoroutine(WaveTwo());
        yield return StartCoroutine(WaveThree());
    }

    private void DestroyAllEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }

        Debug.Log($"Destroyed {enemies.Length} enemies.");
    }
}
