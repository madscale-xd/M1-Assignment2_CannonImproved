using UnityEngine;
using System.Collections;

public class LaunchCannonball : MonoBehaviour
{
    private TextManager mngr;
    private Vector2 _initialVelocity;
    private Vector2 _initialPosition;
    private bool _isLaunched;
    private bool _isCharging;
    private int ballCount = 0;

    private float _time;
    private float _chargeTime;

    [SerializeField] private GameObject projectilePrefab; // reference to cannonball prefab
    [SerializeField] private Transform launchPoint; // launch point of cannonball
    [SerializeField] private Rigidbody _cannonball; // reference to cannonball prefab's rb comp

    [Range(5, 250)] [SerializeField] private float _minPower = 5;
    [Range(5, 250)] [SerializeField] private float _maxPower = 250;
    public float _power;

    [Range(0, 90)] [SerializeField] private float _angle;

    // for debug log purposes
    private float currPower;
    private float currAngle;

    [SerializeField] private Transform cannonBase;
    [SerializeField] private Transform cannonBarrel;

    private Quaternion initialCannonBaseRotation;
    private Quaternion initialCannonBarrelRotation;

    // Add a serialized field for the Renderer component of the object
    [SerializeField] private Renderer Cannon;

    // Flag to check if the object has already flashed
    private bool hasFlashed = false;

    private Material cannonMaterial; // Reference to the Cannon's material
    private Color originalColor; // Store the original color of the Cannon's material
    [SerializeField] private Color flashColor = Color.blue; // The color to flash
    [SerializeField] private float flashDuration = 0.1f; // Duration of each flash
    [SerializeField] private int flashCount = 2; // Number of flashes

    // SETTERS and GETTERS for the debug log
    public void SetIsLaunched(bool value) => _isLaunched = value;

    public bool GetIsLaunched() => _isLaunched;

    public void SetHasFlashed(bool value) => hasFlashed = value;

    public void SetBallCount() => ballCount++;

    public int GetBallCount() => ballCount;

    public void SetAngle() => currAngle = _angle;

    public float GetAngle() => currAngle;

    public void SetPower() => currPower = _power;

    public float GetPower() => currPower;

    private void Start()
    {
        // Store the initial local rotations of the cannon base and barrel for its eventual rotation
        initialCannonBaseRotation = cannonBase.localRotation;
        initialCannonBarrelRotation = cannonBarrel.localRotation;
        _power = _minPower; // Initialize power to the minimum
        mngr = GameObject.Find("ManagerCanvas").GetComponent<TextManager>();

        // Get the Cannon's material and store its original color
        if (Cannon != null)
        {
            Renderer renderer = Cannon.GetComponent<Renderer>();
            if (renderer != null)
            {
                cannonMaterial = renderer.material;
                originalColor = cannonMaterial.color;
            }
        }
    }

    public void Launch()
    {
        SetBallCount();
        Debug.Log("Cannonball " + GetBallCount() + " launched with power: " + _power);

        // Instantiate the cannonball at launch point with the same rotation as the cannon
        GameObject projectile = Instantiate(projectilePrefab, launchPoint.position, launchPoint.rotation);

        LocationOnContact cannonballTrigger = projectile.GetComponent<LocationOnContact>();

        // Set initial position of the cannonball (where it was instantiated)
        if (cannonballTrigger != null)
        {
            cannonballTrigger.SetInitialPosition(launchPoint.position); // Set the launch position
        }

        // Access and assign a variable to cannonball rb for later
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        // Calculate the initial velocity based on the set angle and power
        _initialVelocity = new Vector2(
            Mathf.Cos(_angle * Mathf.PI / 180) * _power,
            Mathf.Sin(_angle * Mathf.PI / 180) * _power
        );

        // Set the initial velocity of cannonball rb
        projectileRb.velocity = new Vector3(_initialVelocity.x, _initialVelocity.y, 0);

        _initialPosition = new Vector2(_cannonball.position.x, _cannonball.position.y);
        _isLaunched = true;

        SetAngle();
        SetPower();
        _chargeTime = 0; // Reset charge time after launch
    }

    private float KinematicEquation(float acceleration, float velocity, float position, float time)
    {
        return (0.5f * acceleration * time * time) + (velocity * time) + position;
    }

    private void RotateCannon() // Rotate the cannon in real-time when manipulating ANGLE value (visual)
    {
        float totalAngle = _angle;

        cannonBase.localRotation = initialCannonBaseRotation * Quaternion.Euler(-totalAngle, 0, 0);
        cannonBarrel.localRotation = initialCannonBarrelRotation * Quaternion.Euler(0, 0, -totalAngle);
    }

    private void Update()
    {
        // Rotate the cannon based on angle
        RotateCannon();

        // Adjust angle using mouse scroll wheel
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0)
        {
            _angle += scrollInput * 20f; // Adjust the sensitivity here
            _angle = Mathf.Clamp(_angle, 0, 90); // Ensure angle stays within range
        }

        // Handle input for charging and launching
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) && !_isLaunched)
        {
            _isCharging = true;
            _chargeTime = 0;
        }

        if ((Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space)) && _isCharging)
        {
            _chargeTime += Time.deltaTime;
            _power = Mathf.Lerp(_minPower, _maxPower, _chargeTime / 2f); // Adjust the divisor for sensitivity
            _power = Mathf.Clamp(_power, _minPower, _maxPower);
            mngr.UpdatePowerText();
        }

        if ((Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.Space)) && _isCharging)
        {
            _isCharging = false;
            Launch();
        }

        // Update position of the launched cannonball
        if (_isLaunched)
        {
            _time += Time.deltaTime;

            float newRockX = KinematicEquation(0, _initialVelocity.x, _initialPosition.x, _time);
            float newRockY = KinematicEquation(-9.81f, _initialVelocity.y, _initialPosition.y, _time);

            _cannonball.position = new Vector3(newRockX, newRockY, _cannonball.position.z);
        }

        // Check if isLaunched is false and flash the object blue once
        if (!_isLaunched && Cannon != null && !hasFlashed)
        {
            hasFlashed = true; // Set flag to true to prevent multiple flashes
            StartCoroutine(FlashCannon());
        }
    }

    // Coroutine to flash the object with the red color like in LoseScript
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
