using System;
using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [Header("------Audio Source-----")]
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _sFXSource;


    [Header("-------Audio Clip------")]
    [SerializeField] private AudioClip _music;
    [SerializeField] private AudioClip _jump;
    [SerializeField] private AudioClip _dartHit;
    [SerializeField] private AudioClip _playerFlip;
    [SerializeField] private AudioClip playerHit;
    [SerializeField] private AudioClip _groundHit;
    [SerializeField] private AudioClip _startSound;
    [SerializeField] private AudioClip _finishSound;
    [SerializeField] private AudioClip _summonStart;
    [SerializeField] private AudioClip _summonActive;
    [SerializeField] private AudioClip _popup;

    private void Start()
    {
        _musicSource.clip = _music; 
        _musicSource.Play();
    }

    private void OnEnable()
    {
        this.Register(EventID.BackToMenu, (data) => _musicSource.Play());
        this.Register(EventID.StartGamePlay, (data) => PlaySFX(_startSound));
        this.Register(EventID.TimeUp, (data) => {
            PlaySFX(_finishSound);
            _musicSource.Stop();
        });
        this.Register(EventID.PlayerJump, (data) => PlaySFX(_jump));
        this.Register(EventID.PlayerHitDart, (data) => PlaySFX(playerHit));
        this.Register(EventID.PlayerHitGround, (data) => PlaySFX(_groundHit));
        this.Register(EventID.PlayerFlip, (data) => PlaySFX(_playerFlip));
        this.Register(EventID.DartHitDart, (data) => PlaySFX(_dartHit));
        this.Register(EventID.StartSummonSkill, (data) => PlaySFX(_summonStart));
        this.Register(EventID.SkillActive, (data) => PlaySFX(_summonActive));
        this.Register(EventID.OnBtnClick, (data) => PlaySFX(_popup));

        this.Register(EventID.OnMusicVolumeChanged, ChangeMusicVolume);
        this.Register(EventID.OnSFXVolumeChanged, ChangeSFXVolume);


    }


    private void OnDisable()
    {
        this.Unregister(EventID.BackToMenu, (data) => _musicSource.Play());
        this.Unregister(EventID.StartGamePlay, (data) => PlaySFX(_startSound));
        this.Unregister(EventID.TimeUp, (data) => {
            PlaySFX(_finishSound);
            _musicSource.Stop();
        });

        this.Unregister(EventID.PlayerJump, (data) => PlaySFX(_jump));
        this.Unregister(EventID.PlayerHitDart, (data) => PlaySFX(playerHit));
        this.Unregister(EventID.PlayerHitGround, (data) => PlaySFX(_groundHit));
        this.Unregister(EventID.PlayerFlip, (data) => PlaySFX(_playerFlip));
        this.Unregister(EventID.DartHitDart, (data) => PlaySFX(_dartHit));
        this.Unregister(EventID.StartSummonSkill, (data) => PlaySFX(_summonStart));
        this.Unregister(EventID.SkillActive, (data) => PlaySFX(_summonActive));
        this.Unregister(EventID.OnBtnClick, (data) => PlaySFX(_popup));

        this.Unregister(EventID.OnMusicVolumeChanged, ChangeMusicVolume);
        this.Unregister(EventID.OnSFXVolumeChanged, ChangeSFXVolume);

    }



    private void PlaySFX(AudioClip sfx)
    {
        _sFXSource.PlayOneShot(sfx);
    }


    private void ChangeSFXVolume(object obj)
    {
        float value = (float)obj;
        if (value < 0f || value > 1f)
        {
            value = Mathf.Clamp01(value);
        }
        _sFXSource.volume = value;
    }

    private void ChangeMusicVolume(object obj)
    {
        float value = (float)obj;
        if (value < 0f || value > 1f)
        {
            value = Mathf.Clamp01(value);
        }
        _musicSource.volume = value;
    }
}
