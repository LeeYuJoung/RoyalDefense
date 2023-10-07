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
            NightTime();
        }
    }

    public void NightTime()
    {
        currentTime += Time.deltaTime;
        UIManager.Instance().nightTimeSlider.value = currentTime / nightTime;

        if(currentTime > nightTime)
        {
            currentTime = 0;
            UIManager.Instance().IsMorning();
            isNight = false;
        }
    }
}
