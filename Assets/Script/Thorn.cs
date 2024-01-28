using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Thorn : MonoBehaviour
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
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            rb.AddForce(new Vector2((collisionPosition.x > 0 ? 1 : -1) * impactForce, -1 * impactForce), ForceMode2D.Impulse);
            Debug.Log(rb.velocity);
            PlayerController playerController = collision.GetComponent<PlayerController>();
            playerController.canMove = false;
            playerController.time = 3f;
            SetActiveObstacle_RPC(false);
        }
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player");
            photonView.RPC("SetActiveObstacle_RPC", RpcTarget.All, false);

        }
    }
    [PunRPC]
    public void SetActiveObstacle_RPC(bool active)
    {
        gameObject.SetActive(active);
    }
    
}
