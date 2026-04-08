using UnityEngine;

/// <summary>
/// Moves the bullet to the right and destroys it when it leaves the screen.
/// </summary>
public class Bullet : MonoBehaviour
{
    public float speed = 10f;

    private Camera _cam;
    private float _rightLimit;

    void Start()
    {
        _cam = Camera.main;
        _rightLimit = _cam.orthographicSize * _cam.aspect + 1f;
    }

    void Update()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;

        if (transform.position.x > _rightLimit)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyController enemy = other.GetComponent<EnemyController>();
            if (enemy != null)
            {
                GameManager.Instance.AddScore(enemy.scoreValue);
            }
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
