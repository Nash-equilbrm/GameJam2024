using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchDirection : MonoBehaviour
{
    public ContactFilter2D castFilter;

    public float groundDistance = 0.05f;
    public float wallDistance = 0.2f;

    private CircleCollider2D touchingCol;
    private BoxCollider2D boxCollider2D;
    RaycastHit2D[] groundHits = new RaycastHit2D[5];
    RaycastHit2D[] wallHits = new RaycastHit2D[5];

    [SerializeField]
    private bool _isGrounded;
    public bool isGrounded
    {
        get
        {
            return _isGrounded;
        }
        private set
        {
            _isGrounded = value;
            //animator.SetBool(AnimationStrings.isGrounded, value);
        }
    }

    private bool _isOnWall;
    public bool isOnWall
    {
        get
        {
            return _isOnWall;
        }
        private set
        {
            _isOnWall = value;
            //animator.SetBool(AnimationStrings.isOnWall, value);
        }
    }
    private Vector2 wallCheckDirection => gameObject.transform.localScale.x > 0 ? Vector2.right : Vector2.left;

    private void Awake()
    {
        touchingCol = GetComponent<CircleCollider2D>();
    }

    private void FixedUpdate()
    {
        isGrounded = touchingCol.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0;
        isOnWall = touchingCol.Cast(wallCheckDirection, castFilter, wallHits, wallDistance) > 0;
    }
}
