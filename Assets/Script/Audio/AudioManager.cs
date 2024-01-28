using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
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
