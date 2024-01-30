using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [Header("------Audio Source-----")]
    [SerializeField] public AudioSource musicSource;
    [SerializeField] public AudioSource SFXSource;

    [Header("-------Audio Clip------")]
    [SerializeField] public AudioClip Music;
    [SerializeField] public AudioClip Jump;
    [SerializeField] public AudioClip DartHit;
    [SerializeField] public AudioClip PlayerFlip;
    [SerializeField] public AudioClip PlayerHit;
    [SerializeField] public AudioClip GroundHit;
    [SerializeField] public AudioClip StartSound;
    [SerializeField] public AudioClip FinishSound;
    [SerializeField] public AudioClip SummonStart;
    [SerializeField] public AudioClip SummonActive;

    private void Start()
    {
        musicSource.clip = Music; 
        musicSource.Play();
    }

    public void PlaySFX(AudioClip sfx)
    {
        SFXSource.PlayOneShot(sfx);
    }
}
