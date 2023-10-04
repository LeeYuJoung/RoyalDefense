using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public enum ATTACKTYPE
    {
        SINGLE,
        RANGE
    }
    public ATTACKTYPE attackType;

    private EnemyController livingEntityController;

    void Start()
    {
        livingEntityController = GetComponent<EnemyController>();
    }

    void Update()
    {
        
    }

    public void SingleAttack()
    {
        switch (livingEntityController.target.tag)
        {
            case "King":
                livingEntityController.target.GetComponent<PlayerController>().OnDamage(livingEntityController.power);

                break;
            case "Pawn":
                livingEntityController.target.GetComponent<PawnController>().OnDamage(livingEntityController.power);

                break;
            case "Building":
                break;
            case "Enemy":
                break;
            default: 
                break;
        }
    }

    public void RangeAttack()
    {

    }
}
