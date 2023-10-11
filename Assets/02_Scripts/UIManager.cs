using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

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

    public GameObject mainPanel;
    public GameObject shopPanel;
    public GameObject upgradePanel;
    public GameObject createPanel;
    public GameObject sellPanel;
    public GameObject deletePanel;
    public GameObject victoryPanel;
    public GameObject deletePossiblePanel;
    public GameObject gameoverPanel;
    public GameObject pricePanel;

    public Slider casleSlider;

    public Text daysText;
    public Text goldText;
    public Text diaText;

    public Slider nightTimeSlider;
    public Text nightText;

    public GameObject[] createPossiblePanels;
    public int[] createPrices;

    public GameObject currentCreateObject;
    public Vector3 objectsPos;
    public int objectsNum;
    public bool objectsBuy = false;

    public void BuildingSlider(Slider _slider, float _value, float _coolTime)
    {
        _slider.value = _value / _coolTime;
    }

    public void BuyObjectsSelect()
    {
        string buildingType = EventSystem.current.currentSelectedGameObject.name;

        if (buildingType.Equals("Wall"))
        {
            objectsNum = 0;
            objectsPos = new Vector3(0, 1.0f, 0);
        }
        else if (buildingType.Equals("Baricade"))
        {
            objectsNum = 1;
            objectsPos = new Vector3(0, 1.5f, 0);
        }
        else if (buildingType.Equals("Balista"))
        {
            objectsNum = 2;
            objectsPos = new Vector3(0, 0, 0);
        }
        else if (buildingType.Equals("Tower"))
        {
            objectsNum = 3;
            objectsPos = new Vector3(0, 0, 0);
        }
        else if (buildingType.Equals("GoldMine"))
        {
            objectsNum = 4;
            objectsPos = new Vector3(0, 1.8f, 0);
        }
        else if (buildingType.Equals("Spearman"))
        {
            objectsNum = 5;
            objectsPos = new Vector3(0, 0.3f, 0);
        }
        else if (buildingType.Equals("ShieldBearer"))
        {
            objectsNum = 6;
            objectsPos = new Vector3(0, 0.0f, 0); 
        }
        else if (buildingType.Equals("Wizard"))
        {
            objectsNum = 7;
            objectsPos = new Vector3(0, 0.3f, 0);
        }

        shopPanel.SetActive(false);
        createPanel.SetActive(true);
        pricePanel.SetActive(false);

        objectsBuy = true;
    }

    public void CreateTrueButton()
    {
        if (currentCreateObject.CompareTag("Building"))
            GameManager.Instance().gold -= currentCreateObject.GetComponent<BuildingController>().createPrice;
        else if(currentCreateObject.CompareTag("Pawn"))
            GameManager.Instance().gold -= currentCreateObject.GetComponent<PawnController>().createPrice;

        createPanel.SetActive(false);
    }

    public void CreateFalseButton()
    {       
        Destroy(currentCreateObject);
        createPanel.SetActive(false);
    }

    public void CreateObjectLeftTurn()
    {
        Vector3 dir = currentCreateObject.transform.localRotation.eulerAngles + new Vector3(0, 90.0f, 0);
        currentCreateObject.transform.localRotation = Quaternion.Euler(dir);
    }

    public void CreateObjectRightTurn()
    {
        Vector3 dir = currentCreateObject.transform.localRotation.eulerAngles + new Vector3(0, -90.0f, 0);
        currentCreateObject.transform.localRotation = Quaternion.Euler(dir);
    }

    public void GoldTextChnage()
    {
        goldText.text = GameManager.Instance().gold + " G";
    }

    public void DiaTextChange()
    {
        diaText.text = GameManager.Instance().diamond + " D";
    }

    public void BuyPossibleCheck()
    {
        for(int i = 0; i < createPossiblePanels.Length; i++)
        {
            if (GameManager.Instance().gold >= createPrices[i])
            {
                createPossiblePanels[i].SetActive(false);
            }
            else
            {
                createPossiblePanels[i].SetActive(true);
            }
        }
    }

    public void SellPanelOpen()
    {
        GameObject[] _buildings = GameObject.FindGameObjectsWithTag("Building");

        if( _buildings.Length > 0 )
        {
            for (int i = 0; i < _buildings.Length; i++)
            {
                _buildings[i].transform.GetChild(0).gameObject.SetActive(true);
            }
        }

        UpgradeManager.Instance().sellPrice.text = "0 G";
        UpgradeManager.Instance().buildingName.text = "NONE";

        GoldTextChnage();
        DiaTextChange();
    }

    public void SellPanelClose()
    {
        GameObject[] _buildings = GameObject.FindGameObjectsWithTag("Building");

        if( _buildings.Length > 0)
        {
            for (int i = 0; i < _buildings.Length; i++)
            {
                _buildings[i].transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    public void NextStageButton()
    {
        nightText.text = "b";
        GameManager.Instance().days += 1;
        GameManager.Instance().nightTime += 15.0f;
        GameManager.Instance().isNight = true;

        daysText.text = String.Format("DAYS {0:00}", GameManager.Instance().days);
    }

    public void Victory()
    {
        victoryPanel.SetActive(true);
    }

    public void IsMorning()
    {
        nightText.text = "c";
        nightTimeSlider.value = 0;
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        gameoverPanel.SetActive(true);
    }

    public void RestartButton()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1;
    }

    public void GameExitButton()
    {
        Application.Quit();
    }
}
