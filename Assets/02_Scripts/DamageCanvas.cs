using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 데미지 텍스트 생성
public class DamageCanvas : MonoBehaviour
{
    public Text damageText;
    public int damage;
    public float destroyTime = 2.0f;

    void Start()
    {
        damageText.fontSize = 300;
    }

    void Update()
    {
        damageText.text = string.Format("- {0}", damage);
        transform.Translate(Vector3.up * 1.5f * Time.deltaTime);
        damageText.fontSize -= 2;

        destroyTime -= Time.deltaTime;
        if(destroyTime <= 0 || !GameManager.Instance().isNight)
        {
            Destroy(gameObject);
        }
    }
}
