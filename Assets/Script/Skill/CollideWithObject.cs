using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideWithObject : MonoBehaviour
{
    [SerializeField]
    private float impactForce;
    [SerializeField]
    private PhotonView photonView;
    [SerializeField]
    private Collider2D colli;
    private void Start()
    {
        colli = GetComponent<Collider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("LocalPlayer"))
        {
            Vector3 collisionPosition = collision.transform.position - transform.position;
            //Debug.Log("Collision Position: " + collisionPosition.normalized);
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            rb.AddForce(collisionPosition * impactForce, ForceMode2D.Impulse);
        }
        if (collision.CompareTag("Player"))
        {
            PhotonNetwork.RemoveBufferedRPCs(photonView.ViewID, "DartCollide");
            photonView.RPC("DartCollide", RpcTarget.AllBuffered, this.gameObject as object);
            PhotonNetwork.SendAllOutgoingCommands();
        }
        if (collision.CompareTag("Darts"))
        {
            PhotonNetwork.RemoveBufferedRPCs(photonView.ViewID, "DartCollide");
            photonView.RPC("DartCollide", RpcTarget.AllBuffered, collision.gameObject as object);
            PhotonNetwork.SendAllOutgoingCommands();
        }

    }
    [PunRPC]
    public void DartCollide(object obj)
    {
        if (obj is GameObject o)
        {
            Collider2D collider = o.GetComponent<Collider2D>();
            if (collider != null)
            {
                collider.gameObject.SetActive(false);
            }
        }
        
    }
}
