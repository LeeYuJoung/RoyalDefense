using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static PlayerController;

public class BuildingController : MonoBehaviour
{
    public enum BUILDINGTYPE
    {
        DEFENCETYPE,
        BARICADE,
        BALISTA,
        TOWER,
        GOLDMINE
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

    public AttackController attackController;

    public GameObject bulletPrefab;
    public GameObject towerHead;
    public GameObject target;

    public int health;
    public int power;

    public float attackRange;
    public float currentTme;
    public float attackCoolTime;

    public bool isDead = false;

    void Start()
    {
        attackController = GetComponentInChildren<AttackController>();
    }

    void Update()
    {
        TargetCheck();

        switch (buildingState)
        {
            case BUILDINGSTATE.IDLE:

                if(target != null && (buildingType == BUILDINGTYPE.BALISTA || buildingType == BUILDINGTYPE.TOWER || buildingType == BUILDINGTYPE.BARICADE))
                {
                    buildingState = BUILDINGSTATE.ATTACK;
                }

                break;
            case BUILDINGSTATE.ATTACK: 
                if(target != null)
                {
                    if(buildingType == BUILDINGTYPE.BALISTA)
                    {
                        transform.LookAt(target.transform.position);
                        Vector3 dir = transform.localRotation.eulerAngles;
                        dir.x = 0;
                        transform.localRotation = Quaternion.Euler(dir);

                        currentTme += Time.deltaTime;

                        if (currentTme > attackCoolTime)
                        {
                            currentTme = 0;
                            Shot();
                        }
                    }
                    else if(buildingType == BUILDINGTYPE.TOWER)
                    {
                        towerHead.transform.LookAt(target.transform.position);
                        currentTme += Time.deltaTime;

                        if (currentTme > attackCoolTime)
                        {
                            currentTme = 0;
                            Shot();
                        }
                    }
                    else
                    {
                        currentTme += Time.deltaTime;

                        if (currentTme > attackCoolTime)
                        {
                            currentTme = 0;
                            target.GetComponent<EnemyController>().OnDamage(power);
                        }
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

    public void Shot()
    {
        GameObject _bullet =  Instantiate(bulletPrefab, attackController.transform.position, Quaternion.identity);
        _bullet.GetComponent<BulletController>().power = this.power;
        _bullet.GetComponent<BulletController>().dir = attackController.transform;
    }

    public void OnDamage(int _power)
    {
        health -= _power;

        if (health <= 0)
        {
            isDead = true;
            Destroy(gameObject, 1.25f);
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
        else
        {
            target = null;
        }
     }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
