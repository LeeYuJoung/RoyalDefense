using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PawnGroupController : MonoBehaviour
{
    private NavMeshAgent agent;
    private PawnController[] _pawns = new PawnController[9];

    public Transform moveTarget;
    public float moveSpeed;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        for(int i = 0; i < _pawns.Length; i++)
        {
            _pawns[i] = GetComponentInChildren<PawnController>();
        }
    }


    void Update()
    {
        if(moveTarget == null)
        {
            return;
        }

        agent.isStopped = false;
        agent.speed = moveSpeed;

        float distance = Vector3.Distance(transform.position, moveTarget.position);

        if (distance <= 3.0f)
        {
            //for (int i = 0; i < _pawns.Length; i++)
            //{
            //    _pawns[i].pawnState = PawnController.LIVINGENTITYSTATE.IDLE;
            //}
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
        }
        else
        {
            agent.SetDestination(moveTarget.position);
        }
    }
}
