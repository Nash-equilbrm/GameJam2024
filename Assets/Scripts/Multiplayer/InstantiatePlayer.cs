using HaloKero.Gameplay;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InstantiatePlayer : MonoBehaviour
{
    public GameObject[] players;
    public Transform[] posSpawn;

    private void Start()
    {
        ResetSpawnPlayer();
    }

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

    private void ResetSpawnPlayer(object obj = null)
    {
        if (GameflowManager.Instance.Player != null) { 
            PhotonNetwork.Destroy(GameflowManager.Instance.Player);
            GameflowManager.Instance.Player = null;
        }
    }

    private void SpawnPlayer(object data)
    {
        if (GameflowManager.Instance.Player == null)
        {
            int playerID = GetPlayerIdFromActorNumber(PhotonNetwork.LocalPlayer);
            GameflowManager.Instance.Player = PhotonNetwork.Instantiate(players[playerID].name, posSpawn[playerID].position, Quaternion.identity);
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

}
