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
        int playerID = PhotonNetwork.LocalPlayer.ActorNumber % 4;
        PhotonNetwork.Instantiate(players[playerID].name, posSpawn[playerID].position,Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
