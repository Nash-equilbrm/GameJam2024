using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Rigidbody2D), typeof(TouchDirection))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private TouchDirection touchDirection;
    public Rigidbody2D rb;
    [SerializeField]
    private float jumpForce = 3f;
    [SerializeField]
    private float speed = 3f;
    [SerializeField]
    private bool _isFacingRight = true;
    public GameObject[] Darts;

    private bool isSkillCasting = false;
    [SerializeField]
    private float skillCD = 5f;
    [SerializeField]
    private float skillTime = 3f;
    private float currentSkillCD = 0f;
    private float currentSkillTime = 0f;

    public PhotonView photonView;
    public bool canMove;
    public float time;
    private void Start()
    {
        if (photonView == null)
            photonView = GetComponent<PhotonView>();
        if (!photonView.IsMine)
        {
            Destroy(rb);
            return;
        }
        if (photonView.CreatorActorNr != PhotonNetwork.LocalPlayer.ActorNumber)
        {
            Destroy(rb);
            return;
        }
        canMove = true;
        tag = "LocalPlayer";
        Camera.main.GetComponent<CameraFollow>().SetupCamera(this.transform);
    }

    public bool isFacingRight
    {
        get
        {
            return _isFacingRight;
        }
        set
        {
            if (_isFacingRight != value)
            {
                transform.localScale *= new Vector2(-1, 1);
            }
            _isFacingRight = value;
        }
    }

    private void Update()
    {
        if (isSkillCasting)
        {
            if (currentSkillTime > 0)
            {
                currentSkillTime -= Time.timeScale * Time.deltaTime;
            }
            else
            {
                foreach (var dart in Darts)
                {
                    dart.SetActive(false);
                }
                isSkillCasting = false;
                currentSkillCD = skillCD;
            }
        }
        if (!photonView.IsMine) { return; }
        if (photonView.CreatorActorNr != PhotonNetwork.LocalPlayer.ActorNumber)
        {
            return;
        }
        time -= Time.deltaTime;
        if (time > 0)
        {
            canMove = false;
        }
        else canMove = true;
        PlayerMovement();
        DartSkill();
    }
    private void PlayerMovement()
    {
        if (!photonView.IsMine) { return; }
        if (photonView.CreatorActorNr != PhotonNetwork.LocalPlayer.ActorNumber)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space)&&canMove)
        {
            rb.velocity = new Vector2(isFacingRight ? speed : -speed, jumpForce);
        }
    }
    private void DartSkill()
    {
        if (currentSkillCD > 0 && !isSkillCasting)
        {
            currentSkillCD -= Time.timeScale * Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.E) && !isSkillCasting && currentSkillCD <= 0)
        {
            photonView.RPC("UseSkill", RpcTarget.All);
        }

    }
    [PunRPC]
    private void UseSkill()
    {
        foreach(var dart in Darts)
        {
            dart.SetActive(true);
        }
        isSkillCasting = true;
        currentSkillTime = skillTime;
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine) { return; }
        if (photonView.CreatorActorNr != PhotonNetwork.LocalPlayer.ActorNumber)
        {
            return;
        }
        if (touchDirection.isOnWall)
        {
            FlipPlayer();
            rb.velocity = new Vector2(isFacingRight ? speed * 0.8f : -speed * 0.8f, rb.velocity.y);
        }
    }

    private void FlipPlayer()
    {
        isFacingRight = !isFacingRight;
    }


    public void SetActiveObject(bool active)
    {
        photonView.RPC("SetActiveObject_RPC", RpcTarget.All, active);
    }


    [PunRPC]
    public void SetActiveObject_RPC(bool active)
    {
        gameObject.SetActive(false);
    }
}
