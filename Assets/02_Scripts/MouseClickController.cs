using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseClickController : MonoBehaviour
{
    public GameObject[] objectsPrefabs;
    public GameObject _object;

    public RaycastHit hit;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out hit))
            {
                if(hit.collider != null)
                {
                    switch(hit.collider.gameObject.tag)
                    {
                        case "Tile":
                            if (GameManager.Instance().isNight && _object != null)
                            {
                                _object.GetComponent<PawnController>().moveTarget = hit.transform;
                                _object.GetComponent<PawnController>().pawnState = PawnController.LIVINGENTITYSTATE.WALK;

                                _object = null;
                                Time.timeScale = 1.0f;
                            }
                            else if(!GameManager.Instance().isNight && UIManager.Instance().objectsBuy && !UIManager.Instance().shopPanel.activeSelf)
                            {
                                GameObject _object = Instantiate(objectsPrefabs[UIManager.Instance().objectsNum]);
                                _object.transform.position = hit.transform.position + UIManager.Instance().objectsPos;
                                UIManager.Instance().currentCreateObject = _object.gameObject;
                                UIManager.Instance().objectsBuy = false;
                            }

                            break;
                        case "Pawn":
                            if (GameManager.Instance().isNight)
                            {
                                _object = hit.collider.gameObject;
                                Time.timeScale = 0.25f;
                            }
                            UpgradeManager.Instance().UpgradeClick(hit.collider);

                            break;
                        case "Building":
                            UpgradeManager.Instance().UpgradeClick(hit.collider);

                            if (UIManager.Instance().sellPanel.activeSelf)
                            {
                                UpgradeManager.Instance().BuildingSelect(hit.collider);
                            }

                            break;
                        case "Obstacle":
                            if (!GameManager.Instance().isNight && !UIManager.Instance().createPanel.activeSelf && !UIManager.Instance().upgradePanel.activeSelf && !UIManager.Instance().shopPanel.activeSelf && !UIManager.Instance().sellPanel.activeSelf)
                            {
                                UIManager.Instance().mainPanel.SetActive(false);
                                UIManager.Instance().deletePanel.SetActive(true);
                                UIManager.Instance().pricePanel.SetActive(true);

                                if (GameManager.Instance().gold >= 4)
                                {
                                    UIManager.Instance().deletePossiblePanel.SetActive(false);
                                }
                                else
                                {
                                    UIManager.Instance().deletePossiblePanel.SetActive(true);
                                }
                                UpgradeManager.Instance().objectCollider = hit.collider;
                            }

                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
