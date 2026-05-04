using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform spawnPoint;

    [Header("Spawn Settings")]
    public float spawnInterval = 3f;
    public float spawnDelay = 2f;

    void Start()
    {
        Debug.Log("Spawn script ada di object: " + gameObject.name);

        // Cek apakah prefab sudah terisi
        if (enemyPrefab == null)
        {
            Debug.LogError("Enemy Prefab BELUM diisi di Inspector!");
        }
        else
        {
            Debug.Log("Enemy Prefab terdeteksi: " + enemyPrefab.name);
        }

        // Cek apakah spawn point sudah terisi
        if (spawnPoint == null)
        {
            Debug.LogError("Spawn Point BELUM diisi di Inspector!");
        }
        else
        {
            Debug.Log("Spawn Point terdeteksi di posisi: " + spawnPoint.position);
        }

        InvokeRepeating(nameof(SpawnEnemy), spawnDelay, spawnInterval);
    }

    void SpawnEnemy()
    {
        Debug.Log("Mencoba spawn enemy...");

        if (enemyPrefab == null)
        {
            Debug.LogError("Enemy Prefab NULL saat spawn!");
            return;
        }

        if (spawnPoint == null)
        {
            Debug.LogError("Spawn Point NULL saat spawn!");
            return;
        }

        GameObject enemy = Instantiate(enemyPrefab);

        // Paksa posisi spawn
        enemy.transform.position = spawnPoint.position;

        enemy.transform.rotation = Quaternion.identity;

        Debug.Log("Enemy berhasil spawn di: " + spawnPoint.position);
    }
}