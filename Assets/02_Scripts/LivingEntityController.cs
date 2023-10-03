using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LivingEntityController : MonoBehaviour
{
    public enum LIVINGENTITYSTATE
    {
        IDLE = 0,
        WALK,
        ATTACK,
        HIT,
        DIE
    }
    public LIVINGENTITYSTATE LivingEntityState = LIVINGENTITYSTATE.IDLE;

    private NavMeshAgent navAgent;

    public float moveSpeed;
    public float rotateSpeed;
    public float power;

    public float health;

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        StateCheck();
    }

    public void StateCheck()
    {
        switch (LivingEntityState)
        {
            case LIVINGENTITYSTATE.IDLE:
                break;
            case LIVINGENTITYSTATE.WALK:
                break;
            case LIVINGENTITYSTATE.ATTACK:
                break;
            case LIVINGENTITYSTATE.HIT:
                break;
            case LIVINGENTITYSTATE.DIE:
                break;
            default:
                break;
        }
    }
}
