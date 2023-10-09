using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

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
    public Slider hpSlider;

    public Transform moveTarget;
    public Transform attackTarget;

    public int level = 1;
    public int maxHealth;
    public int health;
    public int createPrice;
    public int upgradePrice;

    public float moveSpeed;
    public float rotateSpeed;
    public int power;

    public float attackDistance;
    public float attackCoolTime;
    public float currentTime;
    public float hitTime;

    public bool isDead = false;
    public bool isAttack = false;
    public bool isHit = false;

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
        TargetCheck();
        StateCheck();
    }

    public void StateCheck()
    {
        if (isDead)
            return;

        if (!GameManager.Instance().isNight)
        {
            pawnState = LIVINGENTITYSTATE.IDLE;
        }

        currentTime += Time.deltaTime;

        switch (pawnState)
        {
            case LIVINGENTITYSTATE.None:
                break;
            case LIVINGENTITYSTATE.IDLE:
                animator.SetInteger("LIVINGENTITYSTATE", (int)pawnState);
                navAgent.isStopped = true;
                navAgent.velocity = Vector3.zero;

                break;
            case LIVINGENTITYSTATE.WALK:
                animator.SetInteger("LIVINGENTITYSTATE", (int)pawnState);
                navAgent.isStopped = false;
                navAgent.speed = moveSpeed;

                float distance = Vector3.Distance(transform.position, moveTarget.position);

                if (distance <= 1.0f)
                {
                    pawnState = LIVINGENTITYSTATE.IDLE;
                }
                else
                {
                    navAgent.SetDestination(moveTarget.position);
                }

                break;
            case LIVINGENTITYSTATE.ATTACK:
                animator.SetInteger("LIVINGENTITYSTATE", (int)pawnState);

                if(currentTime > attackCoolTime)
                {
                    currentTime = 0;

                    if (attackController.attackType == AttackController.ATTACKTYPE.SINGLE)
                    {
                        attackController.SingleAttack();
                        pawnState = LIVINGENTITYSTATE.IDLE;
                    }
                    else
                    {
                        attackController.RangeAttack();
                        pawnState = LIVINGENTITYSTATE.IDLE;
                    }
                }

                distance = Vector3.Distance(transform.position, attackTarget.position);

                if (distance > attackDistance)
                {
                    pawnState = LIVINGENTITYSTATE.IDLE;
                }

                break;
            case LIVINGENTITYSTATE.DIE:
                animator.SetTrigger("DIE");
                pawnState = LIVINGENTITYSTATE.None;

                _collider.enabled = false;
                isDead = true;

                break;
            default:
                break;
        }
    }

    public void OnDamage(int _power)
    {
        if (health <= 0)
        {
            pawnState = LIVINGENTITYSTATE.DIE;
        }

        health -= _power;
        hpSlider.value = (float)health / maxHealth;
    }

    public void TargetCheck()
    {
        if(pawnState == LIVINGENTITYSTATE.WALK)
        {
            return;
        }

        currentTime += Time.deltaTime;
        Collider[] _coll = Physics.OverlapSphere(transform.position, 2.0f, 1 << 6);

        if (_coll.Length > 0)
        {
            float minDis = Vector3.Distance(transform.position, _coll[0].transform.position);
            int minIdx = 0;

            for (int i = 0; i < _coll.Length; i++)
            {
                if (minDis >= Vector3.Distance(transform.position, _coll[i].transform.position))
                {
                    minDis = Vector3.Distance(transform.position, _coll[i].transform.position);
                    minIdx = i;
                }
            }

            attackTarget = _coll[minIdx].transform;
            transform.LookAt(attackTarget);
            pawnState = LIVINGENTITYSTATE.ATTACK;
        }
    }
}
