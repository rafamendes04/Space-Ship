using UnityEngine;

/// <summary>
/// Scrolls a sprite to the left and wraps it around when it exits the screen.
/// Attach to each background/star layer sprite.
/// </summary>
public class Parallax : MonoBehaviour
{
    [Tooltip("Speed multiplier relative to base scroll speed. 0 = static, 1 = full speed.")]
    [Range(0f, 1f)]
    public float parallaxEffect = 0.5f;

    [Tooltip("Base scroll speed (units per second). Shared across all layers.")]
    public float baseSpeed = 2f;

    private SpriteRenderer _spriteRenderer;
    private float _spriteWidth;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteWidth = _spriteRenderer.bounds.size.x;
    }

    void Update()
    {
        float delta = baseSpeed * parallaxEffect * Time.deltaTime;
        transform.position += Vector3.left * delta;

        // When the sprite has scrolled fully off the left edge, reposition to the right
        if (transform.position.x <= -_spriteWidth)
        {
            transform.position += new Vector3(_spriteWidth * 2f, 0f, 0f);
        }
    }
}
