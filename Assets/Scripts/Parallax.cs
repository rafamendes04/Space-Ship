using UnityEngine;

/// <summary>
/// Infinite parallax scroll: moves left and wraps to the right when off-screen.
/// Place two copies of each sprite side-by-side (A at X=0, B at X=spriteWidth).
/// Attach this script to EACH sprite copy individually.
/// </summary>
public class Parallax : MonoBehaviour
{
    [Tooltip("Speed multiplier (0 = static, 1 = full speed).")]
    [Range(0f, 1f)]
    public float parallaxEffect = 0.5f;

    [Tooltip("Base scroll speed in units/sec.")]
    public float baseSpeed = 2f;

    private float _spriteWidth;
    private float _startX;

    void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        _spriteWidth = sr.bounds.size.x;
        _startX      = transform.position.x;
    }

    void Update()
    {
        float delta = baseSpeed * parallaxEffect * Time.deltaTime;
        transform.position += Vector3.left * delta;

        // Once the sprite has moved one full width to the left of its start, jump right
        if (transform.position.x <= _startX - _spriteWidth)
        {
            transform.position = new Vector3(
                _startX + _spriteWidth,
                transform.position.y,
                transform.position.z);
        }
    }
}
