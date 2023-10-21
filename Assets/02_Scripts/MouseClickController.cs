using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class MouseClickController : MonoBehaviour
{
    public GameObject[] objectsPrefabs;
    public GameObject _object;
    public GameObject clickPrefab;

    public RaycastHit hit;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out hit))
            {
                if(hit.collider != null)
                {
                    switch(hit.collider.gameObject.tag)
                    {
                        case "Tile":
                            AudioManager.Instance().SoundPlay(AudioManager.Instance().clickSound);

                            if (GameManager.Instance().isNight && _object != null)
                            {
                                _object.GetComponent<PawnController>().moveTarget = hit.transform;
                                _object.GetComponent<PawnController>().pawnState = PawnController.LIVINGENTITYSTATE.WALK;
                                _object.GetComponent<PawnController>().clickObject = Instantiate(clickPrefab, hit.transform.position, hit.transform.rotation);
                                _object = null;

                                if(UIManager.Instance().doubleSpeedImage.sprite == UIManager.Instance().optionSprits[6])
                                {
                                    Time.timeScale = 1.0f;
                                }
                                else
                                {
                                    Time.timeScale = 2.0f;
                                }
                            }
                            else if(!GameManager.Instance().isNight && UIManager.Instance().objectsBuy && !UIManager.Instance().shopPanel.activeSelf)
                            {
                                GameObject _object = Instantiate(objectsPrefabs[UIManager.Instance().objectsNum]);
                                _object.transform.DOScale(UIManager.Instance().objectSize, 0.25f).SetEase(Ease.InExpo).SetEase(Ease.OutBounce);

                                _object.transform.position = hit.transform.position + UIManager.Instance().objectsPos;
                                UIManager.Instance().currentCreateObject = _object.gameObject;
                                UIManager.Instance().objectsBuy = false;
                            }

                            break;
                        case "Pawn":
                            AudioManager.Instance().SoundPlay(AudioManager.Instance().clickSound);

                            if (GameManager.Instance().isNight)
                            {
                                StartCoroutine(ObjectClick(hit.transform.gameObject));
                                _object = hit.collider.gameObject;
                                Destroy(_object.GetComponent<PawnController>().clickObject);
                                Time.timeScale = 0.2f;
                            }

                            if (!GameManager.Instance().isNight && !UIManager.Instance().victoryPanel.activeSelf && !UIManager.Instance().createPanel.activeSelf && !UIManager.Instance().shopPanel.activeSelf && !UIManager.Instance().sellPanel.activeSelf && !UIManager.Instance().deletePanel.activeSelf)
                            {
                                StartCoroutine(ObjectClick(hit.transform.gameObject));
                                UpgradeManager.Instance().UpgradeClick(hit.collider);
                            }

                            break;
                        case "Building":
                            AudioManager.Instance().SoundPlay(AudioManager.Instance().clickSound);

                            if (!GameManager.Instance().isNight && !UIManager.Instance().victoryPanel.activeSelf && !UIManager.Instance().createPanel.activeSelf && !UIManager.Instance().shopPanel.activeSelf && !UIManager.Instance().sellPanel.activeSelf && !UIManager.Instance().deletePanel.activeSelf)
                            {
                                UpgradeManager.Instance().UpgradeClick(hit.collider);
                                StartCoroutine(ObjectClick(hit.transform.gameObject));
                            }

                            if (UIManager.Instance().sellPanel.activeSelf)
                            {
                                UpgradeManager.Instance().BuildingSelect(hit.collider);
                                StartCoroutine(ObjectClick(hit.transform.gameObject));
                            }

                            break;
                        case "Obstacle":
                            AudioManager.Instance().SoundPlay(AudioManager.Instance().clickSound);

                            if (!GameManager.Instance().isNight && !UIManager.Instance().victoryPanel.activeSelf && !UIManager.Instance().createPanel.activeSelf && !UIManager.Instance().upgradePanel.activeSelf && !UIManager.Instance().shopPanel.activeSelf && !UIManager.Instance().sellPanel.activeSelf)
                            {
                                UIManager.Instance().deletePanel.SetActive(true);
                                UIManager.Instance().deletePanel.transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f).SetEase(Ease.InExpo).SetEase(Ease.OutBounce);

                                UIManager.Instance().mainPanel.SetActive(false);
                                UIManager.Instance().pricePanel.SetActive(true);
                                UIManager.Instance().GoldTextChnage();
                                UIManager.Instance().DiaTextChange();

                                if (GameManager.Instance().gold >= 4)
                                {
                                    UIManager.Instance().deletePossiblePanel.SetActive(false);
                                }
                                else
                                {
                                    UIManager.Instance().deletePossiblePanel.SetActive(true);
                                }

                                StartCoroutine(ObjectClick(hit.transform.gameObject));
                                UpgradeManager.Instance().ObstacleCheck(hit.collider);
                            }

                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }

    public IEnumerator ObjectClick(GameObject _object)
    {
        BuildingController _building = _object.GetComponent<BuildingController>();
        if(_building != null && _building.buildingType == BuildingController.BUILDINGTYPE.GOLDMINE)
        {
            _object.transform.DOScale(new Vector3(0.6f, 0.6f, 0.6f), 0.15f);
            yield return new WaitForSeconds(0.15f);
            _object.transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 0.15f);
        }
        else
        {
            _object.transform.DOScale(new Vector3(1.15f, 1.15f, 1.15f), 0.15f);
            yield return new WaitForSeconds(0.15f);
            _object.transform.DOScale(new Vector3(1.0f, 1.0f, 1.0f), 0.15f);
        }
    }
}
