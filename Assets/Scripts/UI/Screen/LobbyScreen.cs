using HaloKero.Gameplay;
using HaloKero.UI.Overlap;
using HaloKero.UI.Popup;
using Photon.Pun;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ExitGames.Client.Photon;
using Photon.Realtime;
using System;
using System.Linq;
using UnityEngine.TextCore.Text;



namespace HaloKero.UI
{
    public class LobbyScreen : BaseScreen
    {
        [Header("Widget")]
        [SerializeField] private Button _backToMainMenuBtn;
        [SerializeField] private Button _getReadyBtn;
        [SerializeField] private TMP_Text _getReadyBtnTxt;
        [SerializeField] private Button _openSettingBtn;
        [SerializeField] private GameObject[] _characters;
        private List<int> _playerIds = new List<int>();
        public override void Hide()
        {
            base.Hide();
            this.Unregister(EventID.OnPlayerEnter, SetUpPlayerSlot);
            this.Unregister(EventID.OnPlayerLeft, SetUpPlayerSlot);
            this.Unregister(EventID.StartGamePlay, StartGamePlayOnClick);

            _getReadyBtn.onClick.RemoveListener(StartGameBtnOnClick);
            _openSettingBtn.onClick.RemoveListener(OpenSettingPopupOnClick);
            _backToMainMenuBtn.onClick.RemoveListener(BackToMenuOnClick);

            foreach (var character in _characters)
            {
                foreach (Transform child in character.transform)
                {
                    Destroy(child.gameObject);
                }
            }
        }

        public override void Init()
        {
            base.Init();
        }

        public override void Show(object data)
        {
            base.Show(data);
            this.Register(EventID.OnPlayerEnter, SetUpPlayerSlot);
            this.Register(EventID.OnPlayerLeft, SetUpPlayerSlot);
            this.Register(EventID.StartGamePlay, StartGamePlayOnClick);

            _getReadyBtn.onClick.AddListener(StartGameBtnOnClick);
            _openSettingBtn.onClick.AddListener(OpenSettingPopupOnClick);
            _backToMainMenuBtn.onClick.AddListener(BackToMenuOnClick);
        }
        private void BackToMenuOnClick()
        {
            this.Broadcast(EventID.OnBtnClick);
            PhotonNetwork.LeaveRoom();
            this.Broadcast(EventID.BackToMenu);
        }

        private void StartGamePlayOnClick(object data)
        {
            _getReadyBtnTxt.text = _ready;
        }

        private void StartGameBtnOnClick()
        {
            this.Broadcast(EventID.OnBtnClick);
            CheckRequirementAndStartGame();
       
        }
        private void CheckRequirementAndStartGame()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                _getReadyBtnTxt.text = (_getReadyBtnTxt.text == _ready) ? _unready : _ready;
                Hashtable prop = new Hashtable() { { "ready", true } };
                PhotonNetwork.LocalPlayer.SetCustomProperties(prop);
            }
            else
            {
                int cnt = 0;
                foreach (var p in PhotonNetwork.PlayerList)
                {
                    if (p.CustomProperties.TryGetValue("ready", out object readyObj))
                    {
                        bool ready = (bool)readyObj;
                        if (!ready)
                        {
                            return;
                        }
                    }
                    else
                    {
                        // Handle the case where "ready" custom property is not found
                        Debug.LogWarning("Custom property 'ready' not found for player: " + p.ActorNumber);
                    }
                    cnt++;
                }

                if (cnt == PhotonNetwork.PlayerList.Length)
                {
                    // start game
                    PhotonNetwork.RaiseEvent((byte)EventID.StartGamePlay, null, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
                }
            }
        }

        private void OpenSettingPopupOnClick()
        {
            this.Broadcast(EventID.OnBtnClick);

            UIManager.Instance?.ShowPopup<SettingPopup>(data: GameSettingManager.Instance?.CurrentSettings, forceShowData: true);
        }


        private void SetUpPlayerSlot(object data)
        {
            Debug.Log("SET UP");
            foreach (var character in _characters)
            {
                foreach (Transform child in character.transform)
                {
                    Destroy(child.gameObject);
                }
            }
            int localId = GetPlayerIdFromActorNumber(PhotonNetwork.LocalPlayer);
            foreach (var p in PhotonNetwork.PlayerList)
            {
                int id = GetPlayerIdFromActorNumber(p);
                GameObject obj = Instantiate(GameflowManager.Instance?.PlayerDummyPrefabs[id]);

                if (id < localId)
                {
                    obj.transform.SetParent(_characters[id + 1].transform);
                }
                else if (id > localId)
                {
                    obj.transform.SetParent(_characters[id].transform);
                }
                else
                {
                    obj.transform.SetParent(_characters[0].transform);
                }
                RectTransform rectTransform = obj.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = Vector2.zero;
                rectTransform.localScale = Vector2.one;
            }

            
        }

        private int GetPlayerIdFromActorNumber(Player p)
        {
            Debug.Log("p: " + p.ActorNumber);
            int[] actorNumbers = PhotonNetwork.PlayerList.Select(x => x.ActorNumber).OrderBy(x => x).ToArray();
            for (int i = 0; i < actorNumbers.Length; ++i)
            {
                if (p.ActorNumber == actorNumbers[i])
                {
                    Debug.Log("p return: " + i);
                    return i;
                }
            }
            return -1;
        }


        

        private string _ready = "ready";
        private string _unready = "wait for host";
        



        
    }
}

