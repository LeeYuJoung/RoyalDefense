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
    public LIVINGENTITYSTATE livingEntityState = LIVINGENTITYSTATE.IDLE;

    private NavMeshAgent navAgent;
    private Animator animator;
    public Transform target;

    public int maxHealth;
    public int health;

    public float moveSpeed;
    public float rotateSpeed;
    public float power;

    public float attackDistance;
    public float attackCoolTime;
    public float currentTime;

    public bool isDead = false;
    public bool isDamage = false;

    void Start()
    {
        health = maxHealth;
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        TargetCheck();
        StateCheck();
    }

    public void StateCheck()
    {
        switch (livingEntityState)
        {
            case LIVINGENTITYSTATE.IDLE:
                break;
            case LIVINGENTITYSTATE.WALK:
                animator.SetInteger("LIVINGENTITYSTATE", (int)livingEntityState);
                navAgent.speed = moveSpeed;

                float distance = Vector3.Distance(transform.position, target.position);

                if(distance < attackDistance)
                {
                    livingEntityState = LIVINGENTITYSTATE.ATTACK;
                }
                else
                {
                    navAgent.SetDestination(target.position);
                }

                break;
            case LIVINGENTITYSTATE.ATTACK:
                animator.SetInteger("LIVINGENTITYSTATE", (int)livingEntityState);
                currentTime += Time.deltaTime;

                if(currentTime > attackCoolTime)
                {
                    currentTime = 0;

                    // АјАн
                }

                distance = Vector3.Distance(transform.position, target.position);

                if(distance > attackDistance)
                {
                    livingEntityState = LIVINGENTITYSTATE.WALK;
                }

                break;
            case LIVINGENTITYSTATE.HIT:
                animator.SetInteger("LIVINGENTITYSTATE", (int)livingEntityState);

                break;
            case LIVINGENTITYSTATE.DIE:
                animator.SetInteger("LIVINGENTITYSTATE", (int)livingEntityState);

                break;
            default:
                break;
        }
    }

    public void TargetCheck()
    {
        target = GameObject.FindWithTag("King").GetComponent<Transform>();
    }
}
