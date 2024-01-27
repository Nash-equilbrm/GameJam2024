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
            photonView.RPC("SetActiveObject_RPC", RpcTarget.All, false);

        }
        if (collision.CompareTag("Darts"))
        {
            photonView.RPC("SetActiveObject_RPC", RpcTarget.All, false);
        }

    }
    [PunRPC]
    public void SetActiveObject_RPC(bool active)
    {
         gameObject.SetActive(active);
    }
}
