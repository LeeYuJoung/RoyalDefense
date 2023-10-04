using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseClickController : MonoBehaviour
{
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
                            if (GameManager.Instance().isNight)
                            {
                                _object.GetComponent<EnemyController>().target = hit.transform;
                            }

                            break;
                        case "Pawn":
                            _object = hit.collider.gameObject;

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
