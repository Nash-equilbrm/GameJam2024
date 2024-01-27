using HaloKero.Lobby;
using HaloKero.UI.Overlap;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace HaloKero.UI
{
    public class LobbyScreen : BaseScreen
    {
        [SerializeField] private GameObject[] _characters;

        public override void Hide()
        {
            base.Hide();
            this.Unregister(EventID.OnPlayerEnter, SetUpPlayerSlot);
            this.Unregister(EventID.SetPlayerID, SetUpPlayerSlot);
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

        }



        private void SetUpPlayerSlot(object data)
        {
            int actorNumber = (int)data;
            GameObject obj = Instantiate(LobbyManager.Instance.PlayerDummyPrefabs[actorNumber - 1]);
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

            obj.transform.position = Vector3.zero;


        }
    }
}

