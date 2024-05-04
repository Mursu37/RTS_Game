using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemy.Spawning
{
    public class EnemySpawning : MonoBehaviour
    {

        public List<SpawnableEnemy> Enemies = new List<SpawnableEnemy>();
        private List<SpawnableEnemy> _spawnableEnemies;
        private List<GameObject> _enemiesToSpawn = new List<GameObject>();

        private int _waveCount;
        private int _waveValue;
        
        private float _untilNextWave;
        private float _timeBetweenWaves;
        
        private Vector3 _hqLocation;
        private int _spawnRange;
        private int _minSpawnDistance;

        private float _timeBetweenSpawns;

        [SerializeField] private GameObject hive;
        private List<GameObject> _spawnLocations = new List<GameObject>();
        
        private void Awake()
        {
            CreateHives(3);
            _hqLocation = GameObject.FindGameObjectWithTag("HQ").transform.position;
            
            // Time until first wave
            _untilNextWave = 45f;
            // Size of first wave. 10 = 2 bugs
            _waveValue = 10;
            // Time between waves after first
            _timeBetweenWaves = 45f;
        }

        private void CreateHives(int amount)
        {
            GameObject[] temp = GameObject.FindGameObjectsWithTag("HiveSpawn");
            List<GameObject> possibleHiveLocations = temp.ToList();
            
            // spawn Hives
            for (int i = 0; i < amount; i++)
            {
                int index = Random.Range(0, possibleHiveLocations.Count);
                var newHive = Instantiate(hive,
                    possibleHiveLocations[index].transform.position,
                    Quaternion.identity);
                possibleHiveLocations.RemoveAt(index);
                _spawnLocations.Add(newHive);
            }

            // Destroy Hive spawn locations
            foreach (var hives in possibleHiveLocations)
            {
                Destroy(hives);
            }
            
        }

        private void GenerateWave(int value)
        {
            _spawnableEnemies = new List<SpawnableEnemy>(Enemies);

            while (_spawnableEnemies.Count > 0)
            {
                int enemyIndex = Random.Range(0, _spawnableEnemies.Count);
                SpawnableEnemy enemy = _spawnableEnemies[enemyIndex];
                if (value - enemy.cost > 0)
                {
                    value -= enemy.cost;
                    _enemiesToSpawn.Add(enemy.enemy);
                }
                else
                {
                    _spawnableEnemies.RemoveAt(enemyIndex);
                }
            }
            

            _timeBetweenSpawns = (_untilNextWave / _enemiesToSpawn.Count) / 2; // Change number at the end to separate spawns in wave less
        }

        
        private Vector3 GetSpawnPosition()
        {
            return _spawnLocations[Random.Range(0, _spawnLocations.Count)].transform.position;
        }

        IEnumerator spawnEnemies()
        {
            while (_enemiesToSpawn.Count > 0)
            {
                Vector3 spawnPoint = GetSpawnPosition();
                spawnPoint.y += _enemiesToSpawn[0].GetComponentInChildren<Collider>().bounds.extents.y;
                Instantiate(_enemiesToSpawn[0], spawnPoint, Quaternion.identity);
                _enemiesToSpawn.RemoveAt(0);
                yield return new WaitForSecondsRealtime(_timeBetweenSpawns);
            }
        }

        private void FixedUpdate()
        {
            _untilNextWave -= Time.fixedDeltaTime;
            if (_untilNextWave <= 0)
            {
                _untilNextWave = _timeBetweenWaves;
                GenerateWave(_waveValue);
                _waveValue = (int) (_waveValue * 1.2f); // 1.2f default. change this to scale waves faster
                StartCoroutine(spawnEnemies());
            }
        }


        [Serializable]
        public class SpawnableEnemy
        {
            public GameObject enemy;
            public int cost;
        }
    }
}
