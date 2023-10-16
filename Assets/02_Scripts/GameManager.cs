using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance()
    {
        return _instance;
    }

    public PostProcessVolume postProcessing;
    public Light mainLight;
    public GameObject[] enemySpawnEffects;

    public int days = 0;
    public int gold = 0;
    public int diamond = 0;

    public float currentTime;
    public float nightTime;

    public int health;
    public int maxHealth;

    public int killCount = 0;
    public int getGold = 0;

    public bool isNight = false;
    public bool isDead = false;

    void Start()
    {
        if(_instance == null)
        {
            _instance = this;
        }
    }

    public void Update()
    {
        if (isNight)
        {
            StartCoroutine(NightLight());
            NightTime();
        }
        else if(!isNight && days != 0)
        {
            StartCoroutine(MorningLight());
        }
    }

    public void NightTime()
    {
        currentTime += Time.deltaTime;
        UIManager.Instance().nightTimeSlider.value = currentTime / nightTime;

        if(currentTime > nightTime)
        {
            currentTime = 0;
            isNight = false;

            UIManager.Instance().Victory();

            for(int i  = 0; i < enemySpawnEffects.Length; i++)
            {
                enemySpawnEffects[i].SetActive(false);
            }
        }
    }

    public void OnDamage(int _power)
    {
        health -= _power;
        UIManager.Instance().casleSlider.value = (float)health / maxHealth;

        if(health < 0)
        {
            isDead = true;
            UIManager.Instance().GameOver();
        }
    }

    public void InitCasle()
    {
        maxHealth += 10;
        health += 10;
    }

    public IEnumerator MorningLight()
    {
        while (mainLight.intensity < 1.1f)
        {
            mainLight.intensity += 0.01f;
            yield return new WaitForSeconds(0.5f);
        }
    }

    public IEnumerator NightLight()
    {
        while (mainLight.intensity > 0.15f)
        {
            mainLight.intensity -= 0.01f;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
