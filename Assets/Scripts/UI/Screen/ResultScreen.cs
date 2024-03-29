using ExitGames.Client.Photon;
using Photon.Pun;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



namespace HaloKero.UI
{
    public class ResultScreen : BaseScreen
    {
        [SerializeField] private TMP_Text _resultTxt;
        [SerializeField] private Button _exitBtn;

        public override void Hide()
        {
            base.Hide();
            this.Unregister(EventID.WonGame, ShowWonResult);
            this.Unregister(EventID.LostGame, ShowLostResult);

            _exitBtn.onClick.RemoveListener(ExitButton_OnClick);
            _resultTxt.text = "";

        }

        public override void Init()
        {
            base.Init();
        }

        public override void Show(object data)
        {
            base.Show(data);
            this.Register(EventID.WonGame, ShowWonResult);
            this.Register(EventID.LostGame, ShowLostResult);
            _exitBtn.onClick.AddListener(ExitButton_OnClick);
        }


        private void ShowWonResult(object obj)
        {
            _resultTxt.text = "you win";
        }

        private void ShowLostResult(object obj)
        {
            _resultTxt.text = "back to the well you go";
        }


        public void ExitButton_OnClick()
        {
            this.Broadcast(EventID.BackToMenu);
        }

    }
}

