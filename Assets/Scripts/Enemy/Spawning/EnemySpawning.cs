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
            _untilNextWave = 45f;
            _minSpawnDistance = 10;
            _waveValue = 10;
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
            

            _timeBetweenSpawns = (_untilNextWave / _enemiesToSpawn.Count) / 2;
        }

        
        private Vector3 GetSpawnPosition()
        {
            return _spawnLocations[Random.Range(0, _spawnLocations.Count)].transform.position;
            /*
            float xPosition = Random.Range(-20, 35);
            float zPosition = Random.Range(-20, 40);

            // if elevation is added to the map y check needs to be reworked
            float yPosition = _hqLocation.y;

            if (!Physics.Raycast(new Vector3(xPosition, 100, zPosition), Vector3.down,
                    1000f, LayerMask.GetMask("Obstacle"), QueryTriggerInteraction.Collide))
            {
                Collider[] colliders = Physics.OverlapSphere(new Vector3(xPosition, yPosition, zPosition),
                    _minSpawnDistance, ~LayerMask.GetMask("Ground", "Obstacle"), QueryTriggerInteraction.Collide);

                bool viableSpawn = true;
                foreach (var collider in colliders)
                {
                    if (!collider.CompareTag("Enemy"))
                    {
                        viableSpawn = false;
                        break;
                    }
                }

                if (viableSpawn)
                {
                    return new Vector3(xPosition, yPosition, zPosition);
                }
            }
            return GetSpawnPosition();
            */
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
                _waveValue = (int) (_waveValue * 1.2f);
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
