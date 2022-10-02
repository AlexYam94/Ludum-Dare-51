using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject[] _enemyToSpawn;
    [SerializeField] float _spawnInterval;
    [SerializeField] float _increaseSpawnInterval;
    [SerializeField] int _increaseSpawnAmount;
    [SerializeField] Collider2D _spawnArea;
    [SerializeField] int _maxAvailable = 50;
    [SerializeField] int _maxSpawnNumber = 20;

    float _spawnCounter;
    float _increaseCounter;
    private int _spawnNumber;
    Bounds _bounds;
    List<GameObject> _spawnedEnemies;

    // Start is called before the first frame update
    void Start()
    {
        _spawnCounter = 0;
        _spawnNumber = 1;
        _increaseCounter = _increaseSpawnInterval;
        _bounds = _spawnArea.bounds;
        _spawnedEnemies = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        CleanupDeadEnemies();
        _spawnCounter -= Time.deltaTime;
        _increaseCounter -= Time.deltaTime;
        if (_increaseCounter < 0 && _spawnedEnemies.Count < _maxAvailable)
        {
            _spawnNumber++;
            _increaseCounter = _increaseSpawnInterval;
        }
        if (_spawnCounter < 0)
        {
            if (_spawnedEnemies.Count < _maxAvailable)
            {
                Spawn();
            }
            _spawnCounter = _spawnInterval;
        }
    }

    private void CleanupDeadEnemies()
    {
        for(int i=0;i<_spawnedEnemies.Count;i++)
        {
            if(_spawnedEnemies[i]==null)
            {
                _spawnedEnemies.RemoveAt(i);
            }
        }
    }

    private void Spawn()
    {
        for (int i = 0; i < _spawnNumber; i++)
        {
            System.Random rand = new System.Random();
            int index = rand.Next(0, _enemyToSpawn.Length); 
            Vector2 pos = new Vector2(
             UnityEngine.Random.Range(_bounds.min.x, _bounds.max.x),
             UnityEngine.Random.Range(_bounds.min.y, _bounds.max.y));

            _spawnedEnemies.Add(Instantiate(_enemyToSpawn[index], pos, transform.rotation));
        }
    }
}
