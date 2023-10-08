using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public Transform[] spawnPos;

    public int[] enemyHealths;
    public float[] enemyPowers;
    public float[] enemyspeeds;

    public float currentTme;
    public float spawnCoolTime;

    void Start()
    {
        
    }

    void Update()
    {
        if (GameManager.Instance().isNight)
        {
            Spawn();
        }
    }

    public void Spawn()
    {
        currentTme += Time.deltaTime;

        if (currentTme > spawnCoolTime)
        {
            currentTme = 0; ;
            int enemyIdx = Random.Range(0, enemyPrefabs.Length);
            int posIdx = Random.Range(0, spawnPos.Length);

            Instantiate(enemyPrefabs[enemyIdx], spawnPos[posIdx].position, spawnPos[posIdx].rotation);
        }
    }

    public void InitEnemy()
    {

    }
}
