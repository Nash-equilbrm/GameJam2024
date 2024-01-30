using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollide : MonoBehaviour
{
    public Animator animator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Darts"))
        {
            Debug.Log("On Trigger: " + collision.tag);
            animator.SetTrigger("Hurt");
            animator.SetBool("IsHurt", true);
        }
    }

    public void EndHurt()
    {
        animator.SetBool("IsHurt", false);
    }
}
