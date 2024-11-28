using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    PlayerObj player;
    [SerializeField] ParticleSystem particles;
    void Start()
    {
        player = FindAnyObjectByType<PlayerObj>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (player.objType == PlayerObj.ObjType.Player)
        {
            PlayerHpSystem playerHp = collision.GetComponent<PlayerHpSystem>();
            if (playerHp != null)
            {
                playerHp.TakeHit(1);
                Destroy(gameObject);
                particles.transform.position = player.transform.position;
                particles.Play();
            }
        }
    }
}
