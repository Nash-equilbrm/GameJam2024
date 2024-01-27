using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideWithObject : MonoBehaviour
{
    [SerializeField]
    private float impactForce;
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            Vector3 collisionPosition = collision.transform.position - transform.position;
            //Debug.Log("Collision Position: " + collisionPosition.normalized);
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            rb.AddForce(collisionPosition * impactForce, ForceMode2D.Impulse);
            Destroy(gameObject);
        }

        if (collision.CompareTag("Darts"))
        {
            if (collision != null)
            {
                Destroy(collision);
                Destroy(gameObject);
            }
        }
    }
}
