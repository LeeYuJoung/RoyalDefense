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

    public bool isNight = false;

    void Start()
    {
        if(_instance == null)
        {
            _instance = this;
        }
    }


}
