using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using DG.Tweening;
using System.Xml.Linq;

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

        DOTween.Init();
    }

    public CanvasGroup victoryCanvas;
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

    public GameObject descriptionPanel;
    public GameObject descriptionBack;
    public CanvasGroup[] descriptions;
    public GameObject[] descriptionsButtons;
    public int descriptionIdx = 0;

    public GameObject optionPanel;
    public GameObject optionMask;
    public Sprite[] optionSprits;
    public Image optionButton;
    public Image SoundImage;
    public Image StopImage;
    public Image doubleSpeedImage;
    
    public Slider casleSlider;

    public Text daysText;
    public Text goldText;
    public Text diaText;
    public Text killCountText;
    public Text getGoldText;

    public Slider nightTimeSlider;
    public Text nightText;

    public GameObject[] createPossiblePanels;
    public int[] createPrices;

    public CanvasGroup[] shopDescriptions;

    public GameObject currentCreateObject;
    public Vector3 objectsPos;
    public Vector3 objectSize;
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
            objectSize = new Vector3(1f, 1f, 1f);
        }
        else if (buildingType.Equals("Baricade"))
        {
            objectsNum = 1;
            objectsPos = new Vector3(0, 1.5f, 0);
            objectSize = new Vector3(1f, 1f, 1f);
        }
        else if (buildingType.Equals("Balista"))
        {
            objectsNum = 2;
            objectsPos = new Vector3(0, 0, 0);
            objectSize = new Vector3(1f, 1f, 1f);
        }
        else if (buildingType.Equals("Tower"))
        {
            objectsNum = 3;
            objectsPos = new Vector3(0, 0, 0);
            objectSize = new Vector3(1f, 1f, 1f);
        }
        else if (buildingType.Equals("GoldMine"))
        {
            objectsNum = 4;
            objectsPos = new Vector3(0, 1.8f, 0);
            objectSize = new Vector3(0.5f, 0.5f, 0.5f);
        }
        else if (buildingType.Equals("Spearman"))
        {
            objectsNum = 5;
            objectsPos = new Vector3(0, 0.3f, 0);
            objectSize = new Vector3(1f, 1f, 1f);
        }
        else if (buildingType.Equals("ShieldBearer"))
        {
            objectsNum = 6;
            objectsPos = new Vector3(0, 0.0f, 0);
            objectSize = new Vector3(1f, 1f, 1f);
        }
        else if (buildingType.Equals("Wizard"))
        {
            objectsNum = 7;
            objectsPos = new Vector3(0, 0.3f, 0);
            objectSize = new Vector3(1f, 1f, 1f);
        }

        shopPanel.SetActive(false);
        pricePanel.SetActive(false);
        createPanel.SetActive(true);
        createPanel.transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f).SetEase(Ease.InExpo).SetEase(Ease.OutBounce);

        objectsBuy = true;
    }

    public void DescriptionOpen(int idx)
    {
        StartCoroutine(Fade(true, shopDescriptions[idx]));
    }

    public void DescriptionClose(int idx)
    {
        StartCoroutine(Fade(false, shopDescriptions[idx]));
    }

    public void CreateTrueButton()
    {
        if (currentCreateObject.CompareTag("Building"))
            GameManager.Instance().gold -= currentCreateObject.GetComponent<BuildingController>().createPrice;
        else if(currentCreateObject.CompareTag("Pawn"))
            GameManager.Instance().gold -= currentCreateObject.GetComponent<PawnController>().createPrice;

        createPanel.transform.DOScale(new Vector3(0.05f, 0.05f, 0.05f), 0.15f).SetEase(Ease.InOutExpo).OnComplete(() => createPanel.SetActive(false));
    }

    public void CreateFalseButton()
    {
        if(currentCreateObject != null)
        {
            currentCreateObject.transform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), 0.15f).SetEase(Ease.InOutExpo).SetEase(Ease.OutBounce).OnComplete(() => Destroy(currentCreateObject));
        }
        createPanel.transform.DOScale(new Vector3(0.05f, 0.05f, 0.05f), 0.15f).SetEase(Ease.InOutExpo).OnComplete(() => createPanel.SetActive(false));
    }

    public void CreateObjectLeftTurn()
    {
        if (currentCreateObject == null)
            return;

        Vector3 dir = currentCreateObject.transform.localRotation.eulerAngles + new Vector3(0, 90.0f, 0);
        currentCreateObject.transform.localRotation = Quaternion.Euler(dir);
    }

    public void CreateObjectRightTurn()
    {
        if (currentCreateObject == null)
            return;

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
        sellPanel.SetActive(true);
        sellPanel.transform.DOScale(new Vector3(1.0f, 1.0f, 1.0f), 0.5f).SetEase(Ease.InExpo).SetEase(Ease.OutBounce);

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
        sellPanel.transform.DOScale(new Vector3(0.05f, 0.05f, 0.05f), 0.15f).SetEase(Ease.InOutExpo).OnComplete(() => sellPanel.SetActive(false));

        GameObject[] _buildings = GameObject.FindGameObjectsWithTag("Building");

        if( _buildings.Length > 0)
        {
            for (int i = 0; i < _buildings.Length; i++)
            {
                _buildings[i].transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    public void OptionButton()
    {
        if (optionPanel.GetComponent<RectTransform>().anchoredPosition.y < 550.0f)
        {
            optionButton.sprite = optionSprits[0];
            optionPanel.GetComponent<RectTransform>().DOAnchorPosY(555f, 0.6f).OnComplete(() => optionMask.SetActive(false));
        }
        else
        {
            optionButton.sprite = optionSprits[1];
            optionMask.SetActive(true);
            optionPanel.GetComponent<RectTransform>().DOAnchorPosY(0.0f, 0.6f);
        }
    }

    public void SoundButton()
    {
        AudioSource _audio = GameObject.Find("BackgroundSound").GetComponent<AudioSource>();

        if(_audio.volume == 0)
        {
            _audio.volume = 0.2f;
            SoundImage.sprite = optionSprits[2];
        }
        else
        {
            _audio.volume = 0;
            SoundImage.sprite = optionSprits[3];
        }
    }

    public void GameStopButton()
    {
        if(Time.timeScale == 0)
        {
            Time.timeScale = 1;
            StopImage.sprite = optionSprits[4];
        }
        else
        {
            Time.timeScale = 0;
            StopImage.sprite = optionSprits[5];
        }
    }

    public void DoubleSpeedButton()
    {
        if(Time.timeScale != 0 && Time.timeScale != 2)
        {
            Time.timeScale = 2;
            doubleSpeedImage.sprite = optionSprits[7];
        }
        else if(Time.timeScale != 0 && Time.timeScale != 1)
        {
            Time.timeScale = 1;
            doubleSpeedImage.sprite = optionSprits[6];
        }
    }

    public void HomeButton()
    {
        SceneManager.LoadScene(0);
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
        StartCoroutine(Fade(true, victoryCanvas));

        killCountText.text = "KILL MONSTER" + "\n" + String.Format("<color=red>{0:00}</color>", GameManager.Instance().killCount);
        getGoldText.text = "GOLD" + "\n" + String.Format("<color=orange>{0:00}</color>", GameManager.Instance().getGold);
        nightText.text = "c";
    }

    public void IsMorning()
    {
        StartCoroutine(Fade(false, victoryCanvas));

        GameManager.Instance().killCount = 0;
        GameManager.Instance().getGold = 0;
        nightTimeSlider.value = 0;

        Time.timeScale = 1;
        doubleSpeedImage.sprite = optionSprits[6];
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

    public void DescriptionPanelOpen()
    {
        for(int i = 0; i < descriptions.Length; i++)
        {
            if(i == 0)
            {
                descriptions[i].gameObject.SetActive(true);
                descriptions[i].alpha = 1;
            }
            else
            {
                descriptions[i].gameObject.SetActive(false);
            }
        }
        descriptionsButtons[0].SetActive(false);
        descriptionsButtons[1].SetActive(true);
        descriptionIdx = 0;

        descriptionBack.GetComponent<RectTransform>().DOAnchorPosY(50.0f, 0.8f).SetEase(Ease.InExpo).SetEase(Ease.OutBounce);
    }

    public void DescriptionPanelClose()
    {
        descriptionBack.GetComponent<RectTransform>().DOAnchorPosY(-1000.0f, 0.5f).SetEase(Ease.InOutExpo).OnComplete(() => descriptionPanel.SetActive(false));
        StartCoroutine(MainPanelOpen());
    }

    public void DescriptionPanelContoll(bool isRight)
    {
        if (isRight)
        {
            StartCoroutine(Fade(false, descriptions[descriptionIdx]));
            descriptionIdx++;
            StartCoroutine(Fade(true, descriptions[descriptionIdx]));
        }
        else
        {
            StartCoroutine(Fade(false, descriptions[descriptionIdx]));
            descriptionIdx--;
            StartCoroutine(Fade(true, descriptions[descriptionIdx]));
        }

        if (descriptionIdx == 4)
        {
            descriptionsButtons[1].SetActive(false);
        }
        else
        {
            descriptionsButtons[1].SetActive(true);
        }
        if (descriptionIdx == 0)
        {
            descriptionsButtons[0].SetActive(false);
        }
        else
        {
            descriptionsButtons[0].SetActive(true);
        }
    }

    IEnumerator MainPanelOpen()
    {
        optionButton.sprite = optionSprits[0];
        optionPanel.GetComponent<RectTransform>().DOAnchorPosY(555f, 0.6f).OnComplete(() => optionMask.SetActive(false));
        yield return new WaitForSeconds(0.5f);
        mainPanel.SetActive(true);
    }

    public IEnumerator Fade(bool isFadeIn, CanvasGroup _canvas)
    {
        if (isFadeIn)
        {
            _canvas.alpha = 0;
            _canvas.gameObject.SetActive(true);
            Tween tween = _canvas.DOFade(1f, 0.65f);
            yield return tween.WaitForCompletion();
        }
        else
        {
            _canvas.alpha = 1;
            Tween tween = _canvas.DOFade(0f, 0.65f);
            yield return tween.WaitForCompletion();
            _canvas.gameObject.SetActive(false);
        }
    }

    public void ButtonTweenEnter(GameObject _object)
    {
        _object.transform.DOScale(new Vector3(0.85f, 0.85f, 0.85f), 0.2f);
    }

    public void ButtonTweenExit(GameObject _object)
    {
        _object.transform.DOScale(new Vector3(1f, 1f, 1f), 0.25f);
    }
}
