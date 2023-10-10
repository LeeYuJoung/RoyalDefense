using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    private static UpgradeManager _instance;

    public static UpgradeManager Instance()
    {
        return _instance;
    }

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
        }

        objectCollider = _object;

        if (_object.CompareTag("Pawn"))
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
        UpgradeClick(objectCollider);
    }
}
