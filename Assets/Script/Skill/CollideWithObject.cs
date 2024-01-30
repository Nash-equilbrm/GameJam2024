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
    public PlayerController parentController;
    private AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManage").GetComponent<AudioManager>();
    }
    private void Start()
    {
        if (parentController == null)
        {
            parentController = GetComponentInParent<PlayerController>();
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("LocalPlayer"))
        {
            Debug.Log("onLocalPlayer");
            PlayerController playerController = collision.GetComponent<PlayerController>();
            
            if (parentController==playerController ) { return; }
            this.Broadcast(EventID.PlayerHitDart);
            Vector3 collisionPosition = collision.transform.position - transform.position;
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            rb.AddForce(new Vector2((collisionPosition.x>0?1:-1) * impactForce,-1*impactForce), ForceMode2D.Impulse);
            Debug.Log("localPlayer");
            playerController.cantMoveTime = 3;
            playerController.canMove = false;
            photonView.RPC("SetActiveObject_RPC", RpcTarget.Others, false);
            SetActiveObject_RPC(false);
        }
        if (collision.CompareTag("Player"))
        {
            PlayerController playerController = collision.GetComponent<PlayerController>();

            if (parentController == playerController) { return; }
            Debug.Log("Player");
            photonView.RPC("SetActiveObject_RPC", RpcTarget.Others, false);
            SetActiveObject_RPC(false);
        }
        if (collision.CompareTag("Darts"))
        {
            CollideWithObject col = collision.GetComponent<CollideWithObject>();
            if (col != null)
            {
                if (parentController.Equals(col.parentController)) return;
            }
            
            this.Broadcast(EventID.DartHitDart);
            photonView.RPC("SetActiveObject_RPC", RpcTarget.Others, false);
            SetActiveObject_RPC(false);
        }

    }
    [PunRPC]
    public void SetActiveObject_RPC(bool active)
    {
         gameObject.SetActive(active);
    }
}
