using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public GameObject building;
    private Rigidbody rb;
    public Transform dir;
    public GameObject explosionEffect;

    public float moveSpeed;
    public float balistaSpeed = 15.0f;

    public float destroyTime;
    public int power;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if(building.GetComponent<BuildingController>().buildingType == BuildingController.BUILDINGTYPE.BALISTA)
        {
            moveSpeed = balistaSpeed;
        }
        else
        {
            moveSpeed = 5.0f;
        }

        rb.AddForce(dir.forward * moveSpeed, ForceMode.Impulse);
    }

    public void Attack(Collider _other)
    {
        if(building.GetComponent<BuildingController>().buildingType == BuildingController.BUILDINGTYPE.BALISTA)
        {
            EnemyController _enemy = _other.GetComponent<EnemyController>();
            if(_enemy != null)
            {
                _enemy.OnDamage(power);
            }
        }
        else if(building.GetComponent<BuildingController>().buildingType == BuildingController.BUILDINGTYPE.TOWER)
        {
            RangeAttack(_other);
        }
    }

    public void RangeAttack(Collider _other)
    {
        Collider[] _coll = Physics.OverlapSphere(_other.transform.position, building.GetComponent<BuildingController>().attackRange, 1 << 6);

        if(_coll.Length != 0)
        {
            for (int i = 0; i < _coll.Length; i++)
            {
                _coll[i].GetComponent<EnemyController>().OnDamage(power);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        AudioManager.Instance().SoundPlay(AudioManager.Instance().towerExplotionSound);
        Attack(other);

        explosionEffect.SetActive(true);
        Destroy(gameObject, destroyTime);
    }
}
