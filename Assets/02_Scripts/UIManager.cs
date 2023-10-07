using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public Text goldText;
    public Text diaText;

    public Slider nightTimeSlider;
    public Text nightText;
    public Text daysText;

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
            objectsPos = new Vector3(0, 0.3f, 0);
        }
        else if (buildingType.Equals("Wizard"))
        {
            objectsNum = 7;
            objectsPos = new Vector3(0, 0.3f, 0);
        }

        shopPanel.SetActive(false);
        createPanel.SetActive(true);

        objectsBuy = true;
    }

    public void CreateTrueButton()
    {
        GameManager.Instance().gold -= currentCreateObject.GetComponent<BuildingController>().createPrice;
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

    public void NextStageButton()
    {
        nightText.text = "b";
        GameManager.Instance().isNight = true;
    }

    public void IsMorning()
    {
        nightText.text = "c";
    }
}
