using UnityEngine;

/// <summary>
/// Spawns enemies from the right side at random heights at regular intervals.
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    public GameObject[] enemyPrefabs;   // Drag Ship02, Ship03, Ship04 prefabs here

    [Header("Spawning")]
    public float spawnInterval = 2f;
    public float minY = -2.5f;
    public float maxY =  2.5f;

    private Camera _cam;
    private float _spawnX;
    private float _timer;

    void Start()
    {
        _cam    = Camera.main;
        _spawnX = _cam.orthographicSize * _cam.aspect + 1f;
        _timer  = spawnInterval;
    }

    void Update()
    {
        if (!GameManager.Instance.IsPlaying) return;

        _timer -= Time.deltaTime;
        if (_timer <= 0f)
        {
            _timer = spawnInterval;
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefabs == null || enemyPrefabs.Length == 0) return;

        int index = Random.Range(0, enemyPrefabs.Length);
        float y   = Random.Range(minY, maxY);
        Vector3 pos = new Vector3(_spawnX, y, 0f);
        Instantiate(enemyPrefabs[index], pos, Quaternion.identity);
    }
}
