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

    public EnemyController enemyController;
    public PawnController pawnController;

    void Start()
    {
        enemyController = GetComponent<EnemyController>();
        pawnController = GetComponent<PawnController>();
    }

    void Update()
    {
        
    }

    public void SingleAttack()
    {
        if (gameObject.CompareTag("Enemy"))
        {
            switch (enemyController.target.tag)
            {
                case "King":
                    enemyController.target.GetComponent<PlayerController>().OnDamage(enemyController.power);
                    break;
                case "Casle":
                    enemyController.target.GetComponent<PlayerController>().OnDamage(enemyController.power);
                    break;
                case "Pawn":
                    enemyController.target.GetComponent<PawnController>().OnDamage(enemyController.power);
                    break;
                case "Building":
                    enemyController.target.GetComponent<BuildingController>().OnDamage(enemyController.power);
                    break;
                default:
                    break;
            }
        }
        else if (gameObject.CompareTag("Pawn"))
        {
            pawnController.attackTarget.GetComponent<EnemyController>().OnDamage(pawnController.power);
        }
    }

    public void RangeAttack()
    {

    }
}
