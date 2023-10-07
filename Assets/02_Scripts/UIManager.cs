using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

    public GameObject shopPanel;
    public GameObject createPanel;

    public int buildingNum;
    public bool buildingBuy = false;

    public void BuildingSlider(Slider _slider, float _value, float _coolTime)
    {
        _slider.value = _value / _coolTime;
    }

    public void BuyBuildingSelect()
    {
        string buildingType = EventSystem.current.currentSelectedGameObject.name;

        if (buildingType.Equals("Wall"))
            buildingNum = 0;
        else if (buildingType.Equals("Baricade"))
            buildingNum = 1;
        else if (buildingType.Equals("Balista"))
            buildingNum = 2;
        else if (buildingType.Equals("Tower"))
            buildingNum = 3;
        else if (buildingType.Equals("GoldMine"))
            buildingNum = 4;

        shopPanel.SetActive(false);
        createPanel.SetActive(true);
        buildingBuy = true;
    }

    public void CreateTrueButton()
    {
        createPanel.SetActive(false);
    }

    public void CreateFalseButton()
    {
        createPanel.SetActive(false);
    }
}
