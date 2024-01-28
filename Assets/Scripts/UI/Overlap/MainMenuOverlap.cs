using HaloKero.Gameplay;
using HaloKero.UI.Popup;
using HaloKero.UI.Screen;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HaloKero.UI.Overlap
{
    public class MainMenuOverlap : BaseOverlap
    {
        [Header("Widgets")]
        [SerializeField] private Button _playBtn;
        [SerializeField] private Button _openSettingBtn;

        public override void Hide()
        {
            base.Hide();
            _playBtn.onClick.RemoveListener(StartGame);
            _openSettingBtn.onClick.RemoveListener(OpenSettingPopup);
            this.Unregister(EventID.OnJoinRoomSuccess, OnJoinRoomSuccess);
        }

        public override void Init()
        {
            base.Init();
            
        }

       
        public override void Show(object data)
        {
            base.Show(data);
            _playBtn.onClick.AddListener(StartGame);
            _openSettingBtn.onClick.AddListener(OpenSettingPopup);
            this.Register(EventID.OnJoinRoomSuccess, OnJoinRoomSuccess);

        }

        private void OnJoinRoomSuccess(object data = null)
        {
            Hide();
        }



        private void StartGame()
        {
            this.Broadcast(EventID.OnConnectToServer);
            //this.Broadcast(EventID.StartLoadingGameplay);
            //Hide();
        }

        private void OpenSettingPopup()
        {
            object settingData = GameSettingManager.Instance.CurrentSettings;
            UIManager.Instance.ShowPopup<SettingPopup>(data: settingData, forceShowData:true);
        }

        
    }
}


