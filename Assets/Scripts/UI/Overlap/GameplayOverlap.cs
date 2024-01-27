using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace HaloKero.UI.Overlap
{
    public class GameplayOverlap : BaseOverlap
    {
        [Header("Widget")]
        [SerializeField] private TMP_Text _timerTxt;
        [SerializeField] private TMP_Text _heightTxt;
        [SerializeField] private TMP_Text _resultTxt;

        public override void Hide()
        {
            base.Hide();
            _resultTxt.gameObject.SetActive(false);
            this.Unregister(EventID.WonGame, ShowWinResult);
            this.Unregister(EventID.LostGame, ShowLostResult);
        }

        public override void Init()
        {
            base.Init();
            _resultTxt.gameObject.SetActive(false);
        }

        public override void Show(object data)
        {
            base.Show(data);
            this.Register(EventID.WonGame, ShowWinResult);
            this.Register(EventID.LostGame, ShowLostResult);
            this.Register(EventID.OnTimeChanged, SetTimer);
        }

        private void ShowWinResult(object data)
        {
            _resultTxt.gameObject.SetActive(true);
            _resultTxt.text = "CONGRATULATION!!!";
            this.Broadcast(EventID.EndGamePlay, EventID.WonGame);
        }

        private void ShowLostResult(object data)
        {
            _resultTxt.gameObject.SetActive(true);
            _resultTxt.text = "YOU FALL";
            this.Broadcast(EventID.EndGamePlay, EventID.LostGame);
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
                _timerTxt.text = TimeSpan.FromSeconds(time).ToString(@"ss\,ff");
            }
            else
            {
                _timerTxt.text = "00:00";
            }
        }
    }
}

