using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Resources
{
    public class ResourceSpawning : MonoBehaviour
    {
        [SerializeField] private GameObject _resourceNodePrefab;
        private GameObject[] _resourceSpawnLocations;
        private GameObject[] _closeResourceSpawnLocations;
        
        // how many resource nodes are spawned
        private int _resourceNodeCount = 5;

        private void Awake()
        {
            _resourceSpawnLocations = GameObject.FindGameObjectsWithTag("ResourceSpawnLocation");
            _closeResourceSpawnLocations = GameObject.FindGameObjectsWithTag("CloseResourceSpawnLocation");
            
            // spawn one close resource spawn
            Vector3 resourcePosition = _closeResourceSpawnLocations[Random.Range(0, _closeResourceSpawnLocations.Length)]
                .transform.position;
            Instantiate(_resourceNodePrefab, resourcePosition, Quaternion.identity);
            _resourceNodeCount--;
            
            // spawn resource node at some of the locations randomly
            for (int i = 0; i < _resourceNodeCount; i++)
            {
                resourcePosition = _resourceSpawnLocations[Random.Range(0, _resourceSpawnLocations.Length)]
                    .transform.position;
                Instantiate(_resourceNodePrefab, resourcePosition, Quaternion.identity);
            }

            // get rid of all the garbage
            foreach (var spawnLocation in _resourceSpawnLocations)
            {
                Destroy(spawnLocation);
            }
            foreach (var spawnLocation in _closeResourceSpawnLocations)
            {
                Destroy(spawnLocation);
            }

            Destroy(this);
        }
    }
}