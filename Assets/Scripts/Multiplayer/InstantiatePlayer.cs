using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiatePlayer : MonoBehaviour
{
    public GameObject[] players;
    public Transform[] posSpawn;
    private GameObject _spawnedPlayer = null;

    private void OnEnable()
    {
        this.Register(EventID.StartGamePlay, SpawnPlayer);
        this.Register(EventID.BackToMenu, ResetSpawnPlayer);
    }

    private void OnDisable()
    {
        this.Unregister(EventID.StartGamePlay, SpawnPlayer);
        this.Unregister(EventID.BackToMenu, ResetSpawnPlayer);
    }

    private void ResetSpawnPlayer(object obj)
    {
        PhotonNetwork.Destroy(_spawnedPlayer);
        _spawnedPlayer = null;
    }

    private void SpawnPlayer(object data)
    {
        if (_spawnedPlayer == null)
        {
            int playerID = (PhotonNetwork.LocalPlayer.ActorNumber - 1) % 4;
            _spawnedPlayer = PhotonNetwork.Instantiate(players[playerID].name, posSpawn[playerID].position, Quaternion.identity);
        }

    }
}
