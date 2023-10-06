using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance()
    {
        return _instance;
    }

    void Start()
    {
        if(_instance == null)
        {
            _instance = this;
        }
    }


}
