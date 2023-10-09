using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance()
    {
        return _instance;
    }

    public Light mainLight;

    public int days = 0;
    public int gold = 0;
    public int diamond = 0;

    public float currentTime;
    public float nightTime;

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
            StartCoroutine(LerpColor(Color.black));
            NightTime();
        }
        else if(!isNight && days != 0)
        {
            StartCoroutine(LerpColor(Color.white));
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
        }
    }

    public void InitCasle()
    {
        PlayerController _casle = GameObject.Find("Casle").GetComponent<PlayerController>();
        _casle.maxHealth += 10;
        _casle.health += 10;
    }

    public IEnumerator LerpColor(Color _changeColor)
    {
        while(mainLight.color != _changeColor)
        {
            mainLight.color = Color.Lerp(mainLight.color, _changeColor, 0.025f);
            yield return new WaitForSeconds(0.05f);
        }
    }
}
