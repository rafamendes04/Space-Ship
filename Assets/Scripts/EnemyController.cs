using UnityEngine;

/// <summary>
/// Controls an individual enemy: moves left and destroys itself off-screen.
/// </summary>
public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 3f;
    public int scoreValue = 10;

    private Camera _cam;
    private float _leftLimit;

    void Start()
    {
        _cam = Camera.main;
        _leftLimit = -(_cam.orthographicSize * _cam.aspect) - 1f;
    }

    void Update()
    {
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;

        if (transform.position.x < _leftLimit)
        {
            Destroy(gameObject);
        }
    }
}
