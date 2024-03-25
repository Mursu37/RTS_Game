using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public GameObject[] unitPrefabs;
    public Transform spawnPoint;
    public float[] spawnTimes;



    public void SpawnUnit(int unitIndex)
    {
        if(unitIndex >= 0 && unitIndex < unitPrefabs.Length)
        {
            StartCoroutine(SpawnUnitCoroutine(unitIndex));
          
        }

        else
        {
            Debug.LogError("invalid unit index");
        }
    }

    private IEnumerator SpawnUnitCoroutine(int unitIndex)
    {
        yield return new WaitForSeconds(spawnTimes[unitIndex]);
        Instantiate(unitPrefabs[unitIndex], spawnPoint.position, spawnPoint.rotation);
    }
}
