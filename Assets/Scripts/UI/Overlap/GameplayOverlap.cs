using HaloKero.Gameplay;
using HaloKero.UI.Popup;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace HaloKero.UI.Overlap
{
    public class GameplayOverlap : BaseOverlap
    {
        [Header("Widget")]
        [SerializeField] private TMP_Text _timerTxt;
        [SerializeField] private TMP_Text _heightTxt;
        [SerializeField] private Button _openSettingBtn;
        [SerializeField] private GameObject _tutorial;



        public override void Hide()
        {
            base.Hide();
            this.Unregister(EventID.OnTimeChanged, SetTimer);
            this.Unregister(EventID.OnHeightChanged, SetHeight);
            _openSettingBtn.onClick.RemoveListener(OpenSettingPopupOnClick);
        }

        public override void Init()
        {
            base.Init();
        }

        public override void Show(object data)
        {
            base.Show(data);
            this.Register(EventID.OnTimeChanged, SetTimer);
            this.Register(EventID.OnHeightChanged, SetHeight);
            _openSettingBtn.onClick.AddListener(OpenSettingPopupOnClick);
            _tutorial.SetActive(true);
            _animTimer = 0f;
            StartCoroutine(TutorialCoroutine());
        }

        private void SetHeight(object data)
        {
            float h = (float)data;
            _heightTxt.text = string.Format("{0:#0.00}", h);
        }



        private void SetTimer(object data)
        {
            float time = (float)data;
            if(time > 60)
            {
                _timerTxt.text = TimeSpan.FromSeconds(time).ToString(@"mm\:ss");
            }
            else if(time <= 60f && time > 0)
            {
                _timerTxt.text = TimeSpan.FromSeconds(time).ToString(@"ss\.ff");
            }
            else
            {
                _timerTxt.text = "00:00";
            }
        }


        private void OpenSettingPopupOnClick()
        {
            this.Broadcast(EventID.OnBtnClick);

            UIManager.Instance?.ShowPopup<SettingPopup>(data: GameSettingManager.Instance?.CurrentSettings, forceShowData: true);
        }


        private float _tutorialDuration = 5f;
        private float _animTimer = 0f;

        private IEnumerator TutorialCoroutine()
        {
            while(_animTimer < _tutorialDuration)
            {
                _animTimer += Time.deltaTime;
                yield return null;
            }

            _tutorial.SetActive(false);
            _animTimer = 0f;

        }
    }
}

