using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CoinController : MonoBehaviour
{
    void Start()
    {
        DOTween.Init();

        transform.LookAt(Camera.main.transform.position);
        transform.DOMoveY(9.0f, 1.25f).OnComplete(() => Destroy(gameObject));
    }
}
