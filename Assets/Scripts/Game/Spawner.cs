using System;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject hexagonPrefab;

    public float spawnRate = 1f;

    private float nextTimeToSpawn = 0f;

    private void Update()
    {
        if (Time.time >= nextTimeToSpawn)
        {
            Instantiate(hexagonPrefab, Vector3.zero, Quaternion.identity);
            nextTimeToSpawn = Time.time + 1f / spawnRate;
        }
    }
}