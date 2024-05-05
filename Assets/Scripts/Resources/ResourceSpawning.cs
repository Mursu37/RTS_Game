using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Resources
{
    public class ResourceSpawning : MonoBehaviour
    {
        [SerializeField] private GameObject _resourceNodePrefab;
        private List<GameObject> _resourceSpawnLocations;
        private GameObject[] _closeResourceSpawnLocations;
        
        // how many resource nodes are spawned
        private int _resourceNodeCount = 5;

        private void Awake()
        {
            var temp = GameObject.FindGameObjectsWithTag("ResourceSpawnLocation");
            _resourceSpawnLocations = temp.ToList();
            _closeResourceSpawnLocations = GameObject.FindGameObjectsWithTag("CloseResourceSpawnLocation");
            // spawn one close resource spawn
            Vector3 resourcePosition = _closeResourceSpawnLocations[Random.Range(0, _closeResourceSpawnLocations.Length)]
                .transform.position;
            Debug.Log(resourcePosition);
            Instantiate(_resourceNodePrefab, resourcePosition, Quaternion.identity);
            _resourceNodeCount--;
            
            // spawn resource node at some of the locations randomly
            for (int i = 0; i < _resourceNodeCount; i++)
            {
                int index = Random.Range(0, _resourceSpawnLocations.Count);
                resourcePosition = _resourceSpawnLocations[index]
                    .transform.position;
                Debug.Log(resourcePosition);
                Instantiate(_resourceNodePrefab, resourcePosition, Quaternion.identity);
                _resourceSpawnLocations.RemoveAt(index);
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