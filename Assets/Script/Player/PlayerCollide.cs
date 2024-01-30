using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollide : MonoBehaviour
{
    public Animator animator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Darts") && collision.gameObject == gameObject)
        {
            Debug.Log("On Trigger: " + collision.tag);
            animator.SetBool("IsHurt", true);
            animator.SetTrigger("Hurt");
        }
    }
    public void EndHurt()
    {
        animator.SetBool("IsHurt", false);
    }
}
