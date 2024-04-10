using System;
using System.Collections.Generic;
using System.Collections;
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
        private Vector3 _hqLocation;
        private int _spawnRange;
        private int _minSpawnDistance;

        private float _timeBetweenSpawns;

        private void Awake()
        {
            _hqLocation = GameObject.FindGameObjectWithTag("HQ").transform.position;
            _untilNextWave = 5f;
            _spawnRange = 100;
            _minSpawnDistance = 10;
            _waveValue = 10;
        }

        private void GenerateWave(int value)
        {
            _spawnableEnemies = new List<SpawnableEnemy>(Enemies);

            while (_spawnableEnemies.Count > 0)
            {
                int enemyIndex = Random.Range(0, _spawnableEnemies.Count);
                SpawnableEnemy enemy = _spawnableEnemies[enemyIndex];
                if (value - enemy.cost >= 0)
                {
                    value -= enemy.cost;
                    _enemiesToSpawn.Add(enemy.enemy);
                }
                else
                {
                    _spawnableEnemies.RemoveAt(enemyIndex);
                }
            }

            _timeBetweenSpawns = _untilNextWave / 2 / _spawnableEnemies.Count;
        }

        private Vector3 GetSpawnPosition()
        {
            float xPosition = Random.Range(_hqLocation.x - _spawnRange, _hqLocation.x + _spawnRange);
            float zPosition = Random.Range(_hqLocation.z - _spawnRange, _hqLocation.z + _spawnRange);
            
            // if elevation is added to the map y check needs to be reworked
            float yPosition = _hqLocation.y;
            
            if (Physics.Raycast(new Vector3(xPosition, 100, zPosition), Vector3.down, 
                    1000f, LayerMask.GetMask("Ground"), QueryTriggerInteraction.Collide))
            {
                Collider[] colliders = Physics.OverlapSphere(new Vector3(xPosition, yPosition, zPosition),
                    _minSpawnDistance, ~LayerMask.GetMask("Ground"), QueryTriggerInteraction.Collide);

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
                    _spawnRange = 100;
                    return new Vector3(xPosition, yPosition, zPosition);
                }
            }
            _spawnRange += 10;
            return GetSpawnPosition();
        }

        IEnumerator spawnEnemies()
        {
            while (_enemiesToSpawn.Count > 0)
            {
                Vector3 spawnPoint = GetSpawnPosition();
                spawnPoint.y += _enemiesToSpawn[0].GetComponent<Collider>().bounds.extents.y;
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
                _untilNextWave = 15f;
                GenerateWave(_waveValue);
                _waveValue = (int) (_waveValue * 1.5f);
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
