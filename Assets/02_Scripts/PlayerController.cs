using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;

public class PlayerController : MonoBehaviour
{
    public enum PLAYERSTATE
    {
        IDLE = 0,
        WALK,
        ATTACK,
        DAMAGE,
        DIE
    }
    public PLAYERSTATE playerState = PLAYERSTATE.IDLE;

    private Rigidbody rb;
    private Animator animator;

    Vector3 dir;
    public float h;
    public float v;
    public float moveSpeed = 3.0f;
    public float rotateSpeed = 2.0f;

    public int health;
    public int maxHealth;

    public float currentTime;
    public float hitTime;
    public float attackCoolTime = 2.0f;
    public float attackTime = 0.667f;
    public int power;

    public bool isAttack = false;
    public bool isDamage = false;
    public bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();

        health = maxHealth;
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(1) && currentTime > attackCoolTime)
        {
            currentTime = 0;
            isAttack = true;
            playerState = PLAYERSTATE.ATTACK;
        }

        StateCheck();
        Move();
    }

    public void StateCheck()
    {
        if(isDead)
        {
            // 게임 오버 UI 활성화

            return;
        }

        switch (playerState)
        {
            case PLAYERSTATE.IDLE:
                animator.SetInteger("PLAYERSTATE", (int)playerState);
                currentTime += Time.deltaTime;

                break; 
            case PLAYERSTATE.WALK:
                animator.SetInteger("PLAYERSTATE", (int)playerState);
                currentTime += Time.deltaTime;

                break; 
            case PLAYERSTATE.ATTACK:
                animator.SetInteger("PLAYERSTATE", (int)playerState);

                currentTime += Time.deltaTime;
                if (currentTime > attackTime)
                {
                    Attack();
                    currentTime = 0;
                    isAttack = false;
                }

                break;
            case PLAYERSTATE.DAMAGE:
                animator.SetInteger("PLAYERSTATE", (int)playerState);

                hitTime += Time.deltaTime;
                if (hitTime > 0.667f)
                {
                    isDamage = false;
                    hitTime = 0;
                    playerState = PLAYERSTATE.IDLE;
                }

                break;
            case PLAYERSTATE.DIE:
                animator.SetInteger("PLAYERSTATE", (int)playerState);

                break;
            default:
                break;
        }
    }

    public void Move()
    {
        if (isAttack || isDamage)
            return;

        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        dir = new Vector3(h, 0, v);
        dir = dir.normalized * moveSpeed * Time.deltaTime;
        transform.localPosition += dir;

        if (h != 0 || v != 0)
        {
            playerState = PLAYERSTATE.WALK;

            Quaternion newRotation = Quaternion.LookRotation(dir);
            rb.rotation = Quaternion.Slerp(rb.rotation, newRotation, rotateSpeed * Time.deltaTime);
        }
        else
        {
            playerState = PLAYERSTATE.IDLE;
        }
    }

    public void Attack()
    {
        Collider[] _coll = Physics.OverlapSphere(transform.position, 2.0f, 1 << 6);

        if(_coll.Length > 0)
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

            _coll[minIdx].GetComponent<EnemyController>().OnDamage(power);
        }
    }

    public void OnDamage(int _power)
    {
        isDamage = true;
        currentTime = 0;
        health -= _power;
        playerState = PLAYERSTATE.DAMAGE;

        if (health <= 0)
        {
            isDead = true;
            playerState = PLAYERSTATE.DIE;
        }
    }
}
