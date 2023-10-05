using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseClickController : MonoBehaviour
{
    public GameObject[] buildingPrefabs;

    public RaycastHit hit;
    public GameObject _object;

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
                            else
                            {
                                GameObject _building = Instantiate(buildingPrefabs[3]);
                                _building.transform.position = hit.transform.position;
                            }

                            break;
                        case "Pawn":
                            _object = hit.collider.gameObject;
                            Time.timeScale = 0.25f;

                            break;
                        case "Building":
                            break;
                        case "King":
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
