using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Collision"))
        {
            anim.SetBool("Hit", true);
            gameObject.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0, 0);
            Destroy(gameObject, 1);
        }
    }
}