using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseClickController : MonoBehaviour
{
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
                            break;
                        case "Pawn":
                            break;
                        case "Building":
                            break;
                    }
                }
            }
        }
    }
}
