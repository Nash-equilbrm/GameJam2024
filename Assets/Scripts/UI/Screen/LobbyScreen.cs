using HaloKero.Gameplay;
using HaloKero.Lobby;
using HaloKero.UI.Overlap;
using HaloKero.UI.Popup;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



namespace HaloKero.UI
{
    public class LobbyScreen : BaseScreen
    {
        [Header("Widget")]
        [SerializeField] private TMP_Text _settingBtnTxt;
        [SerializeField] private TMP_Text _getReadyBtnTxt;
        [SerializeField] private Button _getReadyBtn;
        [SerializeField] private Button _openSettingBtn;
        [SerializeField] private GameObject[] _characters;

        public override void Hide()
        {
            base.Hide();
            this.Unregister(EventID.OnPlayerEnter, SetUpPlayerSlot);
            this.Unregister(EventID.SetPlayerID, SetUpPlayerSlot);
            this.Unregister(EventID.OnLanguageChange, Relocalize);
        }

        public override void Init()
        {
            base.Init();
        }

        public override void Show(object data)
        {
            base.Show(data);
            this.Register(EventID.OnPlayerEnter, SetUpPlayerSlot);
            this.Register(EventID.SetPlayerID, SetUpPlayerSlot);
            _getReadyBtn.onClick.AddListener(StartGame);
            _openSettingBtn.onClick.AddListener(OpenSettingPopup);


            this.Register(EventID.OnLanguageChange, Relocalize);



            Relocalize();
        }

        private void StartGame()
        {
            this.Broadcast(EventID.StartGamePlay);
            if (_getReadyBtnTxt.text == ready)
            {
                _getReadyBtnTxt.text = unready;
            }
            else
            {
                _getReadyBtnTxt.text = ready;
            }
        }

        private void OpenSettingPopup()
        {
            object settingData = GameSettingManager.Instance.CurrentSettings;
            UIManager.Instance.ShowPopup<SettingPopup>(data: settingData, forceShowData: true);
        }


        private void SetUpPlayerSlot(object data)
        {
            int actorNumber = (int)data;
            GameObject obj = Instantiate(GameflowManager.Instance.PlayerDummyPrefabs[actorNumber - 1]);
            if (actorNumber < PhotonNetwork.LocalPlayer.ActorNumber)
            {
                obj.transform.SetParent(_characters[actorNumber].transform);
            }
            else if (actorNumber > PhotonNetwork.LocalPlayer.ActorNumber)
            {
                obj.transform.SetParent(_characters[actorNumber - 1].transform);
            }
            else
            {
                obj.transform.SetParent(_characters[0].transform);
            }

            obj.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;



        }
        private string ready = "ready";
        private string unready = "unready";
        private void Relocalize(object data = null)
        {
            _settingBtnTxt.text = GameSettingManager.Instance.CurrentSettings.CurrentLanguage.SETTING_TITLE;
            //_getReadyBtnTxt.text = GameSettingManager.Instance.CurrentSettings.CurrentLanguage.GET_READY_BTN;
            _getReadyBtnTxt.text = ready;
        }
    }
}

