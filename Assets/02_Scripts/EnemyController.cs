using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

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
    public Slider hpSlider;

    public Transform basicTarget;
    public Transform target;

    public int maxHealth;
    public int health;

    public float moveSpeed;
    public float rotateSpeed;
    public int power;
    public int getGold;

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

        if (!GameManager.Instance().isNight)
        {
            enemyState = LIVINGENTITYSTATE.IDLE;
        }

        switch (enemyState)
        {
            case LIVINGENTITYSTATE.None: 
                break;
            case LIVINGENTITYSTATE.IDLE:
                animator.SetInteger("LIVINGENTITYSTATE", (int)enemyState);
                navAgent.isStopped = true;
                navAgent.velocity = Vector3.zero;

                if (GameManager.Instance().isNight)
                {
                    enemyState = LIVINGENTITYSTATE.WALK;
                }

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
                Vector3 dir = transform.localRotation.eulerAngles;
                dir.x = 0;
                transform.localRotation = Quaternion.Euler(dir);

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
                StartCoroutine(OnDie());

                break;
            default:
                break;
        }
    }

    public void OnDamage(int _power)
    {
        health -= _power;
        hpSlider.value = (float)health / maxHealth;

        if (health <= 0)
        {
            enemyState = LIVINGENTITYSTATE.DIE;
            GameManager.Instance().gold += getGold;
        }
    }

    public void TargetCheck()
    {
        if (isDead)
            return;

        Collider[] _coll = Physics.OverlapSphere(transform.position, 5.0f, 1 << 7);

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
        else
        {
            target = basicTarget;
        }

        if (target.CompareTag("Casle"))
        {
            attackDistance = 4.0f;
        }
        else if (target.CompareTag("Pawn"))
        {
            attackDistance = 3.0f;
        }
        else
        {
            attackDistance = 3.0f;
        }
    }

    IEnumerator OnDie()
    {
        animator.SetTrigger("DIE");
        _collider.enabled = false;
        navAgent.isStopped = true;
        navAgent.velocity = Vector3.zero;
        isDead = true;

        yield return new WaitForSeconds(1.2f);

        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 5);
    }
}
