using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PawnController : MonoBehaviour
{
    public enum LIVINGENTITYSTATE
    {
        None = -1,
        IDLE = 0,
        WALK,
        ATTACK,
        HIT,
        DIE
    }
    public LIVINGENTITYSTATE pawnState;

    private AttackController attackController;
    private NavMeshAgent navAgent;
    private Animator animator;
    private Collider _collider;

    public Transform moveTarget;
    public Transform attackTarget;

    public int maxHealth;
    public int health;

    public float moveSpeed;
    public float rotateSpeed;
    public int power;

    public float attackDistance;
    public float attackCoolTime;
    public float currentTime;

    public bool isDead = false;
    public bool isDamage = false;

    void Start()
    {
        attackController = GetComponent<AttackController>();
        navAgent = GetComponent<NavMeshAgent>();
        _collider = GetComponent<Collider>();
        animator = GetComponentInChildren<Animator>();

        health = maxHealth;
    }

    void Update()
    {
        StateCheck();
    }

    public void StateCheck()
    {
        if (isDead)
            return;

        switch (pawnState)
        {
            case LIVINGENTITYSTATE.None:
                break;
            case LIVINGENTITYSTATE.IDLE:
                animator.SetInteger("LIVINGENTITYSTATE", (int)pawnState);

                break;
            case LIVINGENTITYSTATE.WALK:
                animator.SetInteger("LIVINGENTITYSTATE", (int)pawnState);
                navAgent.speed = moveSpeed;

                float distance = Vector3.Distance(transform.position, moveTarget.position);

                if (distance < attackDistance)
                {
                    pawnState = LIVINGENTITYSTATE.ATTACK;
                }
                else
                {
                    navAgent.SetDestination(moveTarget.position);
                }

                break;
            case LIVINGENTITYSTATE.ATTACK:
                animator.SetInteger("LIVINGENTITYSTATE", (int)pawnState);

                currentTime += Time.deltaTime;
                if (currentTime > attackCoolTime)
                {
                    currentTime = 0;

                    if (attackController.attackType == AttackController.ATTACKTYPE.SINGLE)
                    {
                        attackController.SingleAttack();
                    }
                    else
                    {
                        attackController.RangeAttack();
                    }
                }

                distance = Vector3.Distance(transform.position, moveTarget.position);

                if (distance > attackDistance)
                {
                    pawnState = LIVINGENTITYSTATE.WALK;
                }

                break;
            case LIVINGENTITYSTATE.HIT:
                animator.SetInteger("LIVINGENTITYSTATE", (int)pawnState);

                currentTime += Time.deltaTime;
                if (currentTime > 0.667f)
                {
                    currentTime = 0;
                    pawnState = LIVINGENTITYSTATE.WALK;
                }

                break;
            case LIVINGENTITYSTATE.DIE:
                animator.SetTrigger("DIE");
                pawnState = LIVINGENTITYSTATE.None;

                navAgent.enabled = false;
                _collider.enabled = false;
                isDead = true;

                break;
            default:
                break;
        }
    }

    public void OnDamage(int _power)
    {
        health -= _power;
        pawnState = LIVINGENTITYSTATE.HIT;

        if (health <= 0)
        {
            pawnState = LIVINGENTITYSTATE.DIE;
        }
    }

    public void TargetCheck()
    {

    }
}
