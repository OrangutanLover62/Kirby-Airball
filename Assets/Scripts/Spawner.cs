using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] public Renderer map;
    [SerializeField] public List<GameObject> objectToSpawn;

    public float period = 10f;
    float timer;
    float dropHeight = 100;

    Vector3 minVector;
    Vector3 maxVector;

    private void Awake()
    {
        minVector = map.bounds.min;
        maxVector = map.bounds.max;
        minVector = minVector * 0.9f;
        maxVector = maxVector * 0.9f;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > period)
        {
            timer = 0;
            Spawn();
        }
    }

    void Spawn()
    {
        Vector3 spawnPosition = new Vector3(
            Random.Range(minVector.x, maxVector.x), 
            dropHeight, 
            Random.Range(minVector.z, maxVector.z)
        );
        Instantiate(
            objectToSpawn[Random.Range(0, objectToSpawn.Count)], 
            spawnPosition, 
            transform.rotation
        );
    }
}
