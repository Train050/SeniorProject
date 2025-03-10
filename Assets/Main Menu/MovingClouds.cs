using UnityEngine;
using System.Collections.Generic;

public class MovingClouds : MonoBehaviour
{
    public GameObject[] clouds;
    public float slowestCloudSpeed = 0.5f;
    public float fastestCloudSpeed = 1.5f;
    public float timeToSpawn = 8f;
    public int highestY = 725;
    public int lowestY = 200;
    public int startingX = -2450;
    public int endingX = 2450;
    private List<GameObject> cloudObjects;
    private float currentTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cloudObjects = new List<GameObject>();
        currentTime = timeToSpawn;

        //Initial clouds
        SpawnCloudAt(-1500, 400);
        SpawnCloudAt(50, 750);
        SpawnCloudAt(1110, 530);
        SpawnCloudAt(-2000, 600);
    }

    // Update is called once per frame
    void Update()
    {
        //Timer to spawn a new cloud
        currentTime -= Time.deltaTime;

        if(currentTime <= 0)
        {
            SpawnCloud();
            currentTime = timeToSpawn;
        }

        for(int i = 0; i < cloudObjects.Count; i++)
        {
            //The cloud goes to the right, no vertical movement
            if(cloudObjects[i].transform.position.x > endingX)
            {
                Destroy(cloudObjects[i]);
                cloudObjects.RemoveAt(i);
                continue;
            }

            cloudObjects[i].transform.position = new Vector3(cloudObjects[i].transform.position.x + Random.Range(slowestCloudSpeed, fastestCloudSpeed), cloudObjects[i].transform.position.y, 1);
        }
    }

    void SpawnCloud()
    {
        int randomCloudSelection = Random.Range(0, clouds.Length);
        GameObject newCloud = Instantiate(clouds[randomCloudSelection]);
        newCloud.transform.position = new Vector3(startingX, Random.Range(lowestY, highestY), 1);
        cloudObjects.Add(newCloud);
    }

    void SpawnCloudAt(int x, int y)
    {
        int randomCloudSelection = Random.Range(0, clouds.Length);
        GameObject newCloud = Instantiate(clouds[randomCloudSelection]);
        newCloud.transform.position = new Vector3(x, y, 1);
        cloudObjects.Add(newCloud);
    }
}
