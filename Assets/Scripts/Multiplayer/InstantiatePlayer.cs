using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiatePlayer : MonoBehaviour
{
    public GameObject[] players;
    public Transform[] posSpawn;
    void Start()
    {
        int playerID = (PhotonNetwork.LocalPlayer.ActorNumber + 2) % 4;
        PhotonNetwork.Instantiate(players[playerID].name, posSpawn[playerID].position,Quaternion.identity);
    }
}
