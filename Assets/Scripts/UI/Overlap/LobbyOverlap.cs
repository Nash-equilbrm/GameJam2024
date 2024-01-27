//using HaloKero.Gameplay;
//using HaloKero.UI.Popup;
//using HaloKero.UI.Screen;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using TMPro;
//using UnityEngine;
//using UnityEngine.UI;


//namespace HaloKero.UI.Overlap
//{
//    public class LobbyOverlap : BaseOverlap
//    {
//        [Header("Widget")]
//        [SerializeField] private TMP_Text _settingBtnTxt;
//        [SerializeField] private TMP_Text _getReadyBtnTxt;
//        [SerializeField] private Button _getReadyBtn;
//        [SerializeField] private Button _openSettingBtn;

//        public override void Hide()
//        {
//            base.Hide();
//            this.Unregister(EventID.OnLanguageChange, Relocalize);
//        }

//        public override void Init()
//        {
//            base.Init();
           
//        }

//        public override void Show(object data)
//        {
//            base.Show(data);
//            _getReadyBtn.onClick.AddListener(StartGame);
//            _openSettingBtn.onClick.AddListener(OpenSettingPopup);

            
//            this.Register(EventID.OnLanguageChange, Relocalize);



//            Relocalize();
//        }

//        // test
//        private string ready = "ready";
//        private string unready = "unready";


//        private void StartGame()
//        {
//            this.Broadcast(EventID.StartGamePlay);
//            if(_getReadyBtnTxt.text == ready)
//            {
//                _getReadyBtnTxt.text = unready;
//            }
//            else
//            {
//                _getReadyBtnTxt.text = ready;
//            }
//        }

//        private void OpenSettingPopup()
//        {
//            object settingData = GameSettingManager.Instance.CurrentSettings;
//            UIManager.Instance.ShowPopup<SettingPopup>(data: settingData, forceShowData: true);
//        }


//        private void Relocalize(object data = null)
//        {
//            _settingBtnTxt.text = GameSettingManager.Instance.CurrentSettings.CurrentLanguage.SETTING_TITLE;
//            //_getReadyBtnTxt.text = GameSettingManager.Instance.CurrentSettings.CurrentLanguage.GET_READY_BTN;
//            _getReadyBtnTxt.text = ready;
//        }
//    }
//}

