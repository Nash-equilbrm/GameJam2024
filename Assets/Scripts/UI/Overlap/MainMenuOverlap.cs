using HaloKero.Gameplay;
using HaloKero.UI.Popup;
using HaloKero.UI.Screen;
using Photon.Pun;
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
            _playBtn.onClick.RemoveListener(StartGameOnClick);
            _openSettingBtn.onClick.RemoveListener(OpenSettingPopupOnClick);
            this.Unregister(EventID.OnJoinRoomSuccess, OnJoinRoomSuccess);
        }

        public override void Init()
        {
            base.Init();
            
        }

       
        public override void Show(object data)
        {
            base.Show(data);
            _playBtn.onClick.AddListener(StartGameOnClick);
            _openSettingBtn.onClick.AddListener(OpenSettingPopupOnClick);
            this.Register(EventID.OnJoinRoomSuccess, OnJoinRoomSuccess);

        }

        private void OnJoinRoomSuccess(object data = null)
        {
            Hide();
        }



        private void StartGameOnClick()
        {
            this.Broadcast(EventID.OnBtnClick);
            if (PhotonNetwork.IsConnected)
            {
                UIManager.Instance?.ShowPopup<CreateOrJoinRoomPopup>(forceShowData: true);
            }
            else
            {
                this.Broadcast(EventID.OnConnectToServer);
            }
        }

        private void OpenSettingPopupOnClick()
        {
            this.Broadcast(EventID.OnBtnClick);

            UIManager.Instance?.ShowPopup<SettingPopup>(data: GameSettingManager.Instance?.CurrentSettings, forceShowData:true);
        }

        
    }
}


