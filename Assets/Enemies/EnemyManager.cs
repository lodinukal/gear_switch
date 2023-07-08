using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private RoadManager _road;
    [SerializeField]
    private VanManager _van;

    public List<Enemy> Enemies;
    public List<Enemy> SpawnedEnemies;

    [SerializeField]
    private GameObject _spawnRect;

    public float TimeUntilNextSpawn = 3f;
    public int EnemiesPerSpawn = 3;
    private float _timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        SpawnedEnemies = new List<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= TimeUntilNextSpawn)
        {
            _timer = 0f;
            for (int i = 0; i < EnemiesPerSpawn; i++)
            {
                SpawnedEnemies.Add(SpawnEnemy());
            }
        }

        for (var index = SpawnedEnemies.Count - 1; index >= 0; index--)
        {
            var enemy = SpawnedEnemies[index];
            if (enemy == null)
            {
                SpawnedEnemies.RemoveAt(index);
            }
        }
    }

    Enemy GetRandomEnemy()
    {
        return Enemies[Random.Range(0, Enemies.Count)];
    }

    Enemy SpawnEnemy()
    {
        var enemy = GetRandomEnemy();
        var spawnedEnemy = Instantiate(enemy, transform.position, Quaternion.identity);
        spawnedEnemy.Road = _road;
        spawnedEnemy.Van = _van;
        spawnedEnemy.transform.parent = transform;

        var spawnRectMin = _spawnRect.transform.position - _spawnRect.transform.localScale / 2;
        var spawnRectMax = _spawnRect.transform.position + _spawnRect.transform.localScale / 2;

        var spawnX = Random.Range(spawnRectMin.x, spawnRectMax.x);
        var spawnY = Random.Range(spawnRectMin.y, spawnRectMax.y);

        spawnedEnemy.transform.position = new Vector3(spawnX, spawnY, spawnedEnemy.transform.position.z);

        return spawnedEnemy;
    }
}
