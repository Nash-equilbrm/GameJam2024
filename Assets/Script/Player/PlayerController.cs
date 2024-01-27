using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Rigidbody2D), typeof(TouchDirection))]
public class PlayerController : MonoBehaviour
{
    private TouchDirection touchDirection;
    public Rigidbody2D rb;
    [SerializeField]
    private float jumpForce = 3f;
    [SerializeField]
    private float speed = 3f;
    [SerializeField]
    private bool _isFacingRight = true;
    [SerializeField]
    private GameObject darts;
    private GameObject spawnedDart;

    private bool isSkillCasting = false;
    [SerializeField]
    private float skillCD = 5f;
    [SerializeField]
    private float skillTime = 3f;
    private float currentSkillCD = 0f;
    private float currentSkillTime = 0f;

    public PhotonView photonView;
    private void Start()
    {
        if (photonView == null)
            photonView = GetComponent<PhotonView>();
        if (!photonView.IsMine)
        {
            return;
        }
        if (photonView.CreatorActorNr != PhotonNetwork.LocalPlayer.ActorNumber)
        {
            return;
        }

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
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchDirection = GetComponent<TouchDirection>();
    }

    private void Update()
    {
        if (!photonView.IsMine) { return; }
        if (photonView.CreatorActorNr != PhotonNetwork.LocalPlayer.ActorNumber)
        {
            return;
        }
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector2(isFacingRight ? speed : -speed, jumpForce);
        }
    }
    private void DartSkill()
    {
        if (currentSkillCD > 0 && !isSkillCasting)
        {
            currentSkillCD -= Time.timeScale * Time.deltaTime;
            //Debug.Log("current skillCD: " + currentSkillCD);
        }
        if (Input.GetKeyDown(KeyCode.E) && !isSkillCasting && currentSkillCD <= 0)
        {
            spawnedDart = Instantiate(darts, transform.position, transform.rotation);
            spawnedDart.transform.parent = transform;
            isSkillCasting = true;
            currentSkillTime = skillTime;
        }
        if (isSkillCasting)
        {
            if (currentSkillTime > 0)
            {
                currentSkillTime -= Time.timeScale * Time.deltaTime;
            }
            else
            {
                //Debug.Log("Skill ended");
                Destroy(spawnedDart);
                isSkillCasting = false;
                currentSkillCD = skillCD;
            }
        }
    }

    private void FixedUpdate()
    {
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
}
