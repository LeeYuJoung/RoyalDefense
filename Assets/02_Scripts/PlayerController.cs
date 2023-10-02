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

    public int currentHealth;
    public int maxHealth;

    public float currentTime;
    public float attackCoolTime = 2.0f;
    public float attackTime = 0.667f;

    public bool isAttack = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        currentTime += Time.deltaTime;

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
        switch (playerState)
        {
            case PLAYERSTATE.IDLE:
                animator.SetInteger("PLAYERSTATE", (int)playerState);

                break; 
            case PLAYERSTATE.WALK:
                animator.SetInteger("PLAYERSTATE", (int)playerState);

                break; 
            case PLAYERSTATE.ATTACK:
                animator.SetInteger("PLAYERSTATE", (int)playerState);

                if(currentTime > attackTime)
                {
                    currentTime = 0;
                    isAttack = false;
                }

                break;
            case PLAYERSTATE.DAMAGE:
                animator.SetInteger("PLAYERSTATE", (int)playerState);

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
        if (isAttack)
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
}
