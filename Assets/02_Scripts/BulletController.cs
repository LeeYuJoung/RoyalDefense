using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private Rigidbody rb;
    public Transform dir;
    public GameObject explosionEffect;

    public float moveSpeed = 800f;
    public int power;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        moveSpeed = 15.0f;

        rb.AddForce(dir.forward * moveSpeed, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyController>().OnDamage(power);
        }
        Destroy(gameObject);
    }
}
