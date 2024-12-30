using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    PlayerObj player;
    PlayerHpSystem playerHp;

    [SerializeField] ParticleSystem particles;
    void Start()
    {
        player = FindAnyObjectByType<PlayerObj>();
        playerHp = FindFirstObjectByType<PlayerHpSystem>();
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
            playerHpCollision.TakeHit(1);
            Destroy(gameObject);
            //particles.transform.position = player.transform.position;
            //particles.Play();
        }
    }
}
