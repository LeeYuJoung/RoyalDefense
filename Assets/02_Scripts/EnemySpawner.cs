using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public Transform[] spawnPos;

    public int[] enemyHealths;
    public int[] enemyPowers;
    public float[] enemySpeeds;
    public int[] enemyGolds;

    public float currentTme;
    public float spawnCoolTime;

    void Start()
    {
        
    }

    void Update()
    {
        if (GameManager.Instance().isNight)
        {
            StartCoroutine(Spawn());
        }
    }

    public IEnumerator Spawn()
    {
        currentTme += Time.deltaTime;

        if (currentTme > spawnCoolTime)
        {
            currentTme = 0; ;
            int enemyIdx = Random.Range(0, 100);

            int posIdx = Random.Range(0, spawnPos.Length);

            StartCoroutine(SpawnEffect(posIdx));
            yield return new WaitForSeconds(0.5f);

            if(enemyIdx > 20)
            {
                GameObject _enemy = Instantiate(enemyPrefabs[0], spawnPos[posIdx].position, spawnPos[posIdx].rotation);
                _enemy.GetComponent<EnemyController>().maxHealth = enemyHealths[0];
                _enemy.GetComponent<EnemyController>().power = enemyPowers[0];
                _enemy.GetComponent<EnemyController>().moveSpeed = enemySpeeds[0];
                _enemy.GetComponent<EnemyController>().getGold = enemyGolds[0];
            }
            else
            {
                enemyIdx = Random.Range(0, 100);

                if (enemyIdx > 10)
                {
                    GameObject _enemy = Instantiate(enemyPrefabs[1], spawnPos[posIdx].position, spawnPos[posIdx].rotation);
                    _enemy.GetComponent<EnemyController>().maxHealth = enemyHealths[1];
                    _enemy.GetComponent<EnemyController>().power = enemyPowers[1];
                    _enemy.GetComponent<EnemyController>().moveSpeed = enemySpeeds[1];
                    _enemy.GetComponent<EnemyController>().getGold = enemyGolds[1];
                }
                else
                {
                    GameObject _enemy = Instantiate(enemyPrefabs[2], spawnPos[posIdx].position, spawnPos[posIdx].rotation);
                    _enemy.GetComponent<EnemyController>().maxHealth = enemyHealths[2];
                    _enemy.GetComponent<EnemyController>().power = enemyPowers[2];
                    _enemy.GetComponent<EnemyController>().moveSpeed = enemySpeeds[2];
                    _enemy.GetComponent<EnemyController>().getGold = enemyGolds[2];
                }
            }
        }
    }

    IEnumerator SpawnEffect(int _idx)
    {
        GameManager.Instance().enemySpawnEffects[_idx].SetActive(true);
        yield return new WaitForSeconds(2.5f);
        GameManager.Instance().enemySpawnEffects[_idx].SetActive(false);
    }

    public void InitEnemy()
    {
        for(int i = 0; i < enemyPrefabs.Length; i++)
        {
            enemyHealths[i] += 10;
            enemyPowers[i] += 2;
            enemySpeeds[i] += 0.25f;
        }
    }
}
