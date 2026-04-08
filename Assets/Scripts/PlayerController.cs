using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controls the player ship movement, shooting, and slow-motion activation.
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.25f;

    [Header("Slow Motion")]
    public float slowTimeScale = 0.3f;
    public float slowDuration = 3f;

    // Screen boundary references
    private Camera _cam;
    private float _minX, _maxX, _minY, _maxY;

    // Fire rate tracking
    private float _nextFireTime = 0f;

    // Slow-mo tracking
    private bool _slowActive = false;
    private float _slowTimer = 0f;

    void Start()
    {
        _cam = Camera.main;
        UpdateBounds();
    }

    void Update()
    {
        HandleMovement();
        HandleShooting();
        HandleSlowMotion();
    }

    void UpdateBounds()
    {
        float halfHeight = _cam.orthographicSize;
        float halfWidth  = halfHeight * _cam.aspect;
        Vector3 size = GetComponent<SpriteRenderer>().bounds.extents;

        _minX = -halfWidth  + size.x;
        _maxX =  halfWidth  - size.x;
        _minY = -halfHeight + size.y;
        _maxY =  halfHeight - size.y;
    }

    void HandleMovement()
    {
        float h = 0f;
        float v = 0f;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) h = 1f;
            else if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) h = -1f;

            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) v = 1f;
            else if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) v = -1f;
        }

        Vector3 pos = transform.position;
        pos.x += h * moveSpeed * Time.unscaledDeltaTime;
        pos.y += v * moveSpeed * Time.unscaledDeltaTime;

        pos.x = Mathf.Clamp(pos.x, _minX, _maxX);
        pos.y = Mathf.Clamp(pos.y, _minY, _maxY);

        transform.position = pos;
    }

    void HandleShooting()
    {
        if (Keyboard.current != null && Keyboard.current.spaceKey.isPressed && Time.unscaledTime >= _nextFireTime)
        {
            _nextFireTime = Time.unscaledTime + fireRate;
            Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position;
            Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
        }
    }

    void HandleSlowMotion()
    {
        // Manual trigger via Left Shift (when slow-mo is unlocked by GameManager)
        if (Keyboard.current != null && Keyboard.current.leftShiftKey.wasPressedThisFrame && GameManager.Instance.IsSlowMotionUnlocked && !_slowActive)
        {
            ActivateSlowMotion();
        }

        if (_slowActive)
        {
            _slowTimer -= Time.unscaledDeltaTime;
            if (_slowTimer <= 0f)
            {
                DeactivateSlowMotion();
            }
        }
    }

    public void ActivateSlowMotion()
    {
        _slowActive = true;
        _slowTimer  = slowDuration;
        Time.timeScale        = slowTimeScale;
        Time.fixedDeltaTime   = 0.02f * Time.timeScale;
    }

    public void DeactivateSlowMotion()
    {
        _slowActive           = false;
        Time.timeScale        = 1f;
        Time.fixedDeltaTime   = 0.02f;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            DeactivateSlowMotion();
            GameManager.Instance.TriggerGameOver();
        }
    }
}
