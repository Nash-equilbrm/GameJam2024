using ExitGames.Client.Photon;
using Photon.Pun;
using System;
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

    public bool isSkillCasting = false;
    public float skillCD = 5f;
    [SerializeField]
    private float skillTime = 3f;
    public float currentSkillCD = 0f;
    private float currentSkillTime = 0f;

    public PhotonView photonView;
    public bool canMove;
    public float time;
    public Animator animator;
    public LayerMask groundLayer;


    private Transform jumpEffect;
    private ParticleSystem JumpParticle;
    private Vector3 lastJumpPosition;
    private void OnDestroy()
    {
        Debug.Log(gameObject.name);
        this.Unregister(EventID.TimeUp, OnTimeUp);
    }

    private void Awake()
    {
        jumpEffect = GameObject.Find("JumpParticle").transform;
        JumpParticle = jumpEffect.GetComponent<ParticleSystem>();
    }
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


        // when time's up
        this.Register(EventID.TimeUp, OnTimeUp);
    }

    public bool isFacingRight
    {
        get
        {
            return _isFacingRight;
        }
        private set
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
        else
        {
            canMove = true;
        }

        PlayerMovement();
        DartSkill();
        float point = transform.position.y;

        if (point < 0) point = 0;

        this.Broadcast(EventID.OnHeightChanged, point);
    }
    private void PlayerMovement()
    {
        if (!photonView.IsMine) { return; }
        if (photonView.CreatorActorNr != PhotonNetwork.LocalPlayer.ActorNumber)
        {
            return;
        }

        float rbSpeed = rb.velocity.magnitude;
        animator.SetFloat("Speed", Mathf.Abs(rbSpeed));
        animator.SetFloat("VY", rb.velocity.y);

        Debug.Log(touchDirection.isGrounded);
        animator.SetBool("IsGrounded", touchDirection.isGrounded);

        if (Input.GetKeyDown(KeyCode.Space) && canMove)
        {
            animator.SetTrigger("IsJumping");
            //Adio and GFX
            lastJumpPosition = transform.position;

            jumpEffect.position = new Vector3(lastJumpPosition.x, lastJumpPosition.y - 0.556f, lastJumpPosition.z);
            AudioManager.Instance.PlaySFX(AudioManager.Instance.Jump);
            JumpParticle.Play();
            //Physic
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
        foreach (var dart in Darts)
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
        AudioManager.Instance.PlaySFX(AudioManager.Instance.PlayerFlip);
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



    private void OnTimeUp(object obj = null)
    {
        Debug.Log("OnTimeUp, set new hash " + PhotonNetwork.LocalPlayer.ActorNumber.ToString() + " y: " + this.transform.position.y);
        Hashtable prop = new Hashtable() { { "p" + PhotonNetwork.LocalPlayer.ActorNumber.ToString(), transform.position.y } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(prop);
    }
}
