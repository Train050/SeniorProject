using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public int minTime;
    public int maxTime;
    public bool showSpawnRadius;
    public float spawnRadius;
    public bool spawnerEnabled = true;

    public GameObject[] enemyPrefabs;



    void Update()
    {
        if (!spawnerEnabled)
            return;

        GameObject chosenPrefab;
        if (enemyPrefabs.Length > 1) 
        {
            int randIndex = Random.Range(0, enemyPrefabs.Length);
            chosenPrefab = enemyPrefabs[randIndex];
        }
        else if (enemyPrefabs.Length == 1)
        {
            chosenPrefab = enemyPrefabs[0];
        }
        else
        {
            return;
        }

        if (spawnerEnabled)
        {
            float random_x = Random.Range(-1 * spawnRadius, spawnRadius);
            float random_y = Random.Range(-1 * spawnRadius, spawnRadius);
            Vector3 randPos = new Vector3(random_x, random_y, 0);
            GameObject enemy = Instantiate(chosenPrefab, transform.position + randPos, transform.rotation);
            StartCoroutine(CoolDown());
            spawnerEnabled = false;
        }
    }

    private IEnumerator CoolDown()
	{
        int waitTime = Random.Range(minTime, maxTime);
        Debug.Log("Waiting for " + waitTime);
		yield return new WaitForSeconds(waitTime);
        spawnerEnabled = true;
	}

    	private void OnDrawGizmos()
	{
		if (showSpawnRadius)
			Gizmos.DrawWireSphere(transform.position, spawnRadius);
	}
}
