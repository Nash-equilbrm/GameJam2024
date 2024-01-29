using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollide : MonoBehaviour
{
    public Animator animator;
    [SerializeField]
    private TouchDirection touchDirection;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Darts"))
        {
            Debug.Log("On Trigger: " + collision.tag);
            animator.SetBool("IsHurt", true);
            animator.SetTrigger("Hurt");

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Darts"))
        {
            animator.SetBool("IsHurt", false);
        }
    }
}
