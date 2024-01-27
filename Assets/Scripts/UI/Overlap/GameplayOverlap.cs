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
            this.Unregister(EventID.WonGame, (data) => ShowWinResult());
            this.Unregister(EventID.LostGame, (data) => ShowLostResult());
        }

        public override void Init()
        {
            base.Init();
            _resultTxt.gameObject.SetActive(false);
        }

        public override void Show(object data)
        {
            base.Show(data);
            this.Register(EventID.WonGame, (data) => ShowWinResult());
            this.Register(EventID.LostGame, (data) => ShowLostResult());
        }

        private void ShowWinResult()
        {
            _resultTxt.gameObject.SetActive(true);
            _resultTxt.text = "CONGRATULATION!!!";
            this.Broadcast(EventID.EndGamePlay, EventID.WonGame);
        }

        private void ShowLostResult()
        {
            _resultTxt.gameObject.SetActive(true);
            _resultTxt.text = "YOU FALL";
            this.Broadcast(EventID.EndGamePlay, EventID.LostGame);
        }
    }
}

