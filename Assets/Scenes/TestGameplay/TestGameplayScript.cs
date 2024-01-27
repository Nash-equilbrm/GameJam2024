using HaloKero.Lobby;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGameplayScript : MonoBehaviour
{
    [SerializeField] private GameObject[] _playerDummyPrefabs;

    void Start()
    {
        if(PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            PhotonNetwork.Instantiate(_playerDummyPrefabs[PhotonNetwork.LocalPlayer.ActorNumber].name, Vector3.zero, Quaternion.identity);
        }
        else
        {
            var position = new Vector3(Random.Range(-10.0f, 10.0f), 0, Random.Range(-10.0f, 10.0f));

        }
    }
}
