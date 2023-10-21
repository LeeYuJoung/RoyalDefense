using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
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
    public GameObject[] towerHead;
    public GameObject target;
    public GameObject dieEffect;

    public GameObject coinPrefab;
    public Transform coinPos;

    public GameObject levelupEffect;

    public string buildingName;
    public int level = 1;
    public int health;
    public int maxHealth;
    public int createPrice;
    public int upgradePrice;
    public int sellPrice;
    public int power;

    public float currentTme;
    public float attackCoolTime;
    public float attackRange;

    public float goldCoolTime;
    public int goldPercent;
    public int diaPercent;

    public bool isDead = false;

    void Start()
    {
        health = maxHealth;
        attackController = GetComponentInChildren<AttackController>();
    }

    void Update()
    {
        if (!GameManager.Instance().isNight)
        {
            return;
        }
        TargetCheck();

        switch (buildingState)
        {
            case BUILDINGSTATE.IDLE:

                if ((buildingType == BUILDINGTYPE.BALISTA || buildingType == BUILDINGTYPE.TOWER || buildingType == BUILDINGTYPE.BARICADE))
                {
                    buildingState = BUILDINGSTATE.ATTACK;
                }
                else if(buildingType == BUILDINGTYPE.GOLDMINE)
                {
                    currentTme += Time.deltaTime;
                    UIManager.Instance().BuildingSlider(GetComponentInChildren<Slider>(), currentTme, goldCoolTime);

                    if(currentTme > goldCoolTime)
                    {
                        int rdmNum = Random.Range(1, 100);
                        currentTme = 0;

                        if (rdmNum > 15)
                        {
                            GameManager.Instance().gold += goldPercent;
                        }
                        else
                        {
                            GameManager.Instance().diamond += diaPercent;
                        }

                        Instantiate(coinPrefab, coinPos.position, coinPos.rotation);
                        AudioManager.Instance().SoundPlay(AudioManager.Instance().coinSpawnSound);
                    }
                }

                break;
            case BUILDINGSTATE.ATTACK:
                if(buildingType != BUILDINGTYPE.BARICADE && GameManager.Instance().isNight)
                {
                    currentTme += Time.deltaTime;
                    UIManager.Instance().BuildingSlider(GetComponentInChildren<Slider>(), currentTme, attackCoolTime);
                }

                if (target != null && GameManager.Instance().isNight)
                {
                    if(buildingType == BUILDINGTYPE.BALISTA)
                    {
                        transform.LookAt(target.transform.position);
                        Vector3 dir = transform.localRotation.eulerAngles;
                        dir.x = 0;
                        transform.localRotation = Quaternion.Euler(dir);

                        if (currentTme > attackCoolTime)
                        {
                            AudioManager.Instance().SoundPlay(AudioManager.Instance().ballistaAttackSound);
                            currentTme = 0;
                            Shot();
                        }
                    }
                    else if(buildingType == BUILDINGTYPE.TOWER)
                    {
                        for(int i = 0; i < towerHead.Length; i++)
                        {
                            towerHead[i].transform.LookAt(target.transform.position);
                        }

                        if (currentTme > attackCoolTime)
                        {
                            AudioManager.Instance().SoundPlay(AudioManager.Instance().towerAttackSound);
                            currentTme = 0;
                            Shot();
                        }
                    }
                    else
                    {
                        if (currentTme > attackCoolTime)
                        {
                            currentTme = 0;
                            target.GetComponent<EnemyController>().OnDamage(power);
                        }
                    }
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
            dieEffect.SetActive(true);
            Destroy(gameObject, 0.5f);
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

    public void LevelUP()
    {
        StartCoroutine(LevelUPEffect());

        level += 1;
        maxHealth += 30;
        power += 5;
        attackCoolTime -= 0.5f;
        attackRange += 3.0f;
        goldCoolTime -= 5.0f;

        health = maxHealth;
        upgradePrice += 3;
        sellPrice += 2;

        if (bulletPrefab != null)
        {
            bulletPrefab.GetComponent<BulletController>().power = this.power;
        }
    }

    public IEnumerator LevelUPEffect()
    {
        levelupEffect.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        levelupEffect.SetActive(false);
    }

    public void Sell()
    {
        Instantiate(coinPrefab, coinPos.position, coinPos.rotation);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
