using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
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
    public LIVINGENTITYSTATE enemyState;

    private AttackController attackController;
    private NavMeshAgent navAgent;
    private Animator animator;
    private Collider _collider;
    public Transform target;

    public int maxHealth;
    public int health;

    public float moveSpeed;
    public float rotateSpeed;
    public int power;

    public float attackDistance;
    public float attackCoolTime;
    public float currentTime;
    public float hitTime;

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
        TargetCheck();
        StateCheck();
    }

    public void StateCheck()
    {
        if (isDead)
            return;

        currentTime += Time.deltaTime;

        switch (enemyState)
        {
            case LIVINGENTITYSTATE.None: 
                break;
            case LIVINGENTITYSTATE.IDLE:
                animator.SetInteger("LIVINGENTITYSTATE", (int)enemyState);

                break;
            case LIVINGENTITYSTATE.WALK:
                animator.SetInteger("LIVINGENTITYSTATE", (int)enemyState);
                navAgent.isStopped = false;
                navAgent.speed = moveSpeed;

                float distance = Vector3.Distance(transform.position, target.position);

                if (distance < attackDistance)
                {
                    enemyState = LIVINGENTITYSTATE.ATTACK;
                }
                else
                {
                    navAgent.SetDestination(target.position);
                }

                break;
            case LIVINGENTITYSTATE.ATTACK:
                animator.SetInteger("LIVINGENTITYSTATE", (int)enemyState);
                navAgent.isStopped = true;
                navAgent.velocity = Vector3.zero;
                transform.LookAt(target.position);

                if (currentTime > attackCoolTime)
                {
                    currentTime = 0;

                    if(attackController.attackType == AttackController.ATTACKTYPE.SINGLE)
                    {
                        attackController.SingleAttack();
                    }
                    else
                    {
                        attackController.RangeAttack();
                    }
                }

                distance = Vector3.Distance(transform.position, target.position);

                if(distance > attackDistance)
                {
                    enemyState = LIVINGENTITYSTATE.WALK;
                }

                break;
            case LIVINGENTITYSTATE.HIT:
                animator.SetInteger("LIVINGENTITYSTATE", (int)enemyState);

                hitTime += Time.deltaTime;
                if (hitTime > 0.667f)
                {
                    hitTime = 0;
                    enemyState = LIVINGENTITYSTATE.WALK;
                }

                break;
            case LIVINGENTITYSTATE.DIE:
                animator.SetTrigger("DIE");
                enemyState = LIVINGENTITYSTATE.None;

                navAgent.enabled = false;
                _collider.enabled = false;
                isDead = true;

                Destroy(gameObject, 0.5f);

                break;
            default:
                break;
        }
    }

    public void OnDamage(int _power)
    {
        health -= _power;
        enemyState = LIVINGENTITYSTATE.HIT;

        if (health <= 0)
        {
            enemyState = LIVINGENTITYSTATE.DIE;
        }
    }

    public void TargetCheck()
    {
        Collider[] _coll = Physics.OverlapSphere(transform.position, 40.0f, 1 << 7);

        if (_coll.Length > 0)
        {
            float dis = Vector3.Distance(transform.position, _coll[0].transform.position);
            int minIdx = 0;

            for (int i = 0; i < _coll.Length; i++)
            {
                if (dis >= Vector3.Distance(transform.position, _coll[i].transform.position))
                {
                    dis = Vector3.Distance(transform.position, _coll[0].transform.position);
                    minIdx = i;
                }
            }

            target = _coll[minIdx].transform;
        }
    }
}
