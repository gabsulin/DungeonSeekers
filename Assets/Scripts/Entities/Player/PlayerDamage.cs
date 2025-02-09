using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    PlayerObj player;
    PlayerHpSystem playerHp;
    Animator anim;

    [SerializeField] int damage;

    [SerializeField] ParticleSystem particles;
    void Start()
    {
        player = FindAnyObjectByType<PlayerObj>();
        playerHp = FindFirstObjectByType<PlayerHpSystem>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (playerHp.isDead)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerHpSystem playerHpCollision = collision.GetComponent<PlayerHpSystem>();
        if (playerHpCollision != null)
        {
            playerHpCollision.TakeHit(damage);
            anim.SetBool("Hit", true);
            gameObject.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0, 0);
            Destroy(gameObject, 0.3f);
        }
    }
}
