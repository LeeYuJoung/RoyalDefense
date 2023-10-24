using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance()
    {
        return _instance;
    }

    public Light mainLight;
    public Light[] castleFireLights;

    public GameObject[] enemySpawnEffects;
    public GameObject[] fireEffects;
    public GameObject damageEffect;

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
            StartCoroutine(CastleLightOn(true));
            StartCoroutine(CastleFireLightOn(true));
            NightTime();
        }
        else if(!isNight && days != 0)
        {
            StartCoroutine(MorningLight());
            StartCoroutine(CastleLightOn(false));
            StartCoroutine(CastleFireLightOn(false));
        }

        if (health <= 70)
        {
            fireEffects[0].SetActive(true);
            fireEffects[1].SetActive(true);
        }
        else
        {
            fireEffects[0].SetActive(false);
            fireEffects[1].SetActive(false);
        }

        if (health <= 50)
        {
            fireEffects[2].SetActive(true);
        }
        else
        {
            fireEffects[2].SetActive(false);
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

    public void OnDamage(int _power, Transform _pos)
    {
        StartCoroutine(DamageEffect(_pos));
        UIManager.Instance().casleSlider.value = (float)health / maxHealth;
        health -= _power;

        if (health < 0)
        {
            isDead = true;
            UIManager.Instance().GameOver();
        }
    }

    public IEnumerator DamageEffect(Transform _pos)
    {
        GameObject _damageEffect = Instantiate(damageEffect, _pos.position + (Vector3.up * 2) + (Vector3.forward * 1.5f), _pos.rotation);
        yield return new WaitForSeconds(0.25f);
        Destroy(_damageEffect, 0.15f);
    }

    public void InitCasle()
    {
        maxHealth += 10;
        health += 10;
    }

    public IEnumerator MorningLight()
    {
        while (mainLight.intensity < 1.25f)
        {
            mainLight.intensity += 0.01f;
            yield return new WaitForSeconds(0.5f);
        }
    }

    public IEnumerator NightLight()
    {
        while (mainLight.intensity > 0f)
        {
            mainLight.intensity -= 0.01f;
            yield return new WaitForSeconds(0.5f);
        }
    }

    public IEnumerator CastleLightOn(bool isOn)
    {
        if (isOn)
        {
            while (castleFireLights[2].intensity < 2.1f)
            {
                castleFireLights[2].intensity += 0.02f;
                castleFireLights[3].intensity += 0.02f;
                yield return new WaitForSeconds(0.5f);
            }
        }
        else
        {
            while (mainLight.intensity > 0f)
            {
                castleFireLights[2].intensity -= 0.02f;
                castleFireLights[3].intensity -= 0.02f;
                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    public IEnumerator CastleFireLightOn(bool isOn)
    {
        if (isOn)
        {
            while (castleFireLights[0].intensity < 2.7f)
            {
                castleFireLights[0].intensity += 0.03f;
                castleFireLights[1].intensity += 0.03f;
                yield return new WaitForSeconds(0.5f);
            }
        }
        else
        {
            while (mainLight.intensity > 0f)
            {
                castleFireLights[0].intensity -= 0.03f;
                castleFireLights[1].intensity -= 0.03f;
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}
