using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    PlayerObj player;
    PlayerHpSystem playerHp;
    Animator anim;
    Stone stone;

    [SerializeField] int damage;
    [SerializeField] bool destroy = true;

    [SerializeField] ParticleSystem particles;
    void Start()
    {
        player = FindAnyObjectByType<PlayerObj>();
        playerHp = FindFirstObjectByType<PlayerHpSystem>();
        anim = GetComponent<Animator>();
        stone = FindFirstObjectByType<Stone>();
    }

    private void Update()
    {
        if (playerHp != null && playerHp.isDead && destroy)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerHp = collision.GetComponent<PlayerHpSystem>();
        if (playerHp != null)
        {
            if (stone != null && stone.isPetrified)
            {
                playerHp.TakeHit(Mathf.RoundToInt(damage / 2));
                Debug.Log(Mathf.RoundToInt(damage / 2));
            }
            playerHp.TakeHit(damage);
            if (anim != null)
            {
                if (anim.GetBool("Hit") == true)
                {
                    anim.SetBool("Hit", true);
                    gameObject.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0, 0);
                    if (destroy) Destroy(gameObject, 0.3f);
                }
                else if (destroy) Destroy(gameObject);
            }
            else if (destroy) Destroy(gameObject);
            if (particles != null)
            {
                var spawnedParticles = Instantiate(particles, player.transform.position, Quaternion.identity);
            }
        }
    }
}
