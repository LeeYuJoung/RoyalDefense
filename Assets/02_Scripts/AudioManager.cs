using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager Instance()
    {
        return instance;
    }

    public AudioSource _audio;

    public AudioClip clickSound;
    public AudioClip towerAttackSound;
    public AudioClip towerExplotionSound;
    public AudioClip ballistaAttackSound;
    public AudioClip[] pawnAttackSound;
    public AudioClip pawnDieSound;
    public AudioClip[] enemyAttackSound;
    public AudioClip enemyDieSound;
    public AudioClip enemySpawnSound;

    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }

        _audio = GetComponent<AudioSource>();
    }

    public void SoundPlay(AudioClip _sound)
    {
        _audio.PlayOneShot(_sound);
    }
}
