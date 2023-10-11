using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    private static UpgradeManager _instance;

    public static UpgradeManager Instance()
    {
        return _instance;
    }

    public Text sellPrice;
    public Text buildingName;

    public GameObject possiblePanel;
    public Text priceText;
    public Text levelText;

    public Collider objectCollider;
    public int price;
    public int level;

    void Start()
    {
        if(_instance == null)
        {
            _instance = this;
        }
    }

    public void UpgradeClick(Collider _object)
    {
        if (!GameManager.Instance().isNight && !UIManager.Instance().victoryPanel.activeSelf && !UIManager.Instance().createPanel.activeSelf && !UIManager.Instance().shopPanel.activeSelf && !UIManager.Instance().sellPanel.activeSelf && !UIManager.Instance().upgradePanel.activeSelf) 
        {
            UIManager.Instance().mainPanel.SetActive(false);
            UIManager.Instance().upgradePanel.SetActive(true);
            UIManager.Instance().pricePanel.SetActive(true);
        }

        objectCollider = _object;

        if (objectCollider.CompareTag("Pawn"))
        {
            price = objectCollider.GetComponent<PawnController>().upgradePrice;
            level = objectCollider.GetComponent<PawnController>().level;

            priceText.text = price + " D";
            levelText.text = string.Format("LV.{0:00}", level);

            if(GameManager.Instance().diamond >= price)
            {
                possiblePanel.SetActive(false);
            }
            else
            {
                possiblePanel.SetActive(true);
            }
        }
        else
        {
            price = objectCollider.GetComponent<BuildingController>().upgradePrice;
            level = objectCollider.GetComponent<BuildingController>().level;

            priceText.text = price + " G";
            levelText.text = string.Format("LV.{0:00}", level);

            if (GameManager.Instance().gold >= price)
            {
                possiblePanel.SetActive(false);
            }
            else
            {
                possiblePanel.SetActive(true);
            }
        }

        if(level >= 5)
        {
            levelText.text = "MAX LV";
            possiblePanel.SetActive(true);
        }
    }

    public void UpgradeButton()
    {
        if (objectCollider.CompareTag("Pawn"))
        {
            GameManager.Instance().diamond -= price;
            objectCollider.GetComponent<PawnController>().LevelUP();
        }
        else
        {
            GameManager.Instance().gold -= price;
            objectCollider.GetComponent<BuildingController>().LevelUP();
        }

        UIManager.Instance().GoldTextChnage();
        UIManager.Instance().DiaTextChange();
        UpgradeClick(objectCollider);
    }

    public void ObstacleDelete()
    {
        GameManager.Instance().gold -= 4;

        if (objectCollider != null)
        {
            UIManager.Instance().GoldTextChnage();
            UIManager.Instance().DiaTextChange();
            Destroy(objectCollider.gameObject);
        }
    }

    public void BuildingSelect(Collider _building)
    {
        objectCollider = _building;
        price = objectCollider.GetComponent<BuildingController>().sellPrice;

        buildingName.text = objectCollider.GetComponent<BuildingController>().buildingName;
        sellPrice.text =  price + " G";
    }

    public void BuildingSellButton()
    {
        GameManager.Instance().gold += price;

        if(objectCollider != null)
        {
            UIManager.Instance().GoldTextChnage();
            UIManager.Instance().DiaTextChange();
            Destroy(objectCollider.gameObject);
        }
    }

    public void InitObject()
    {
        price = 0;
        objectCollider = null;
    }
}
