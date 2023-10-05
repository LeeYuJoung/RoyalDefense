using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    public enum BUILDINGTYPE
    {
        DEFENCETYPE,
        ATTACKTYPE
    }
    public BUILDINGTYPE buildingType;

    public enum BUILDINGSTATE
    {
        IDLE,
        ATTACK,
        UPGRADE,
        NONE
    }
    public BUILDINGSTATE buildingState;

    public GameObject _bulletPrefabs;
    public GameObject target;

    public int health;
    public int power;

    public float attackRange;
    public float currentTme;
    public float attackCoolTime;

    void Start()
    {
        
    }

    void Update()
    {
        switch (buildingState)
        {
            case BUILDINGSTATE.IDLE:
                TargetCheck();

                if(target != null && buildingType == BUILDINGTYPE.ATTACKTYPE)
                {
                    buildingState = BUILDINGSTATE.ATTACK;
                }

                break;
            case BUILDINGSTATE.ATTACK: 
                if(target != null)
                {
                    transform.LookAt(target.transform.position);
                    currentTme += Time.deltaTime;

                    if(currentTme > attackCoolTime)
                    {
                        currentTme = 0;

                    }
                }
                else
                {
                    currentTme = 0;
                    buildingState = BUILDINGSTATE.IDLE;
                }

                break;   
            case BUILDINGSTATE.UPGRADE: 
                break;
            default:
                break;
        }
    }

    public void TargetCheck()
    {
        Collider[] _coll = Physics.OverlapSphere(transform.position, attackRange, 1 << 6);

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

            target = _coll[minIdx].gameObject;
        }
     }
}
