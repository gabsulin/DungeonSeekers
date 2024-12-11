using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    private PlayerObj player;
    [SerializeField] ParticleSystem boomParticles;
    bool isHit = false;
    void Start()
    {
        player = FindAnyObjectByType<PlayerObj>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //enemy ma taky PlayerObj a tim padem je playerstate na attack a nefunguje podminka tak jak ma
        if (collision.CompareTag("Enemy") && player.objType == PlayerObj.ObjType.Player && player._playerState == PlayerObj.PlayerState.attack)
        {
            EnemyHpSystem enemyHp = collision.GetComponent<EnemyHpSystem>();
            if (enemyHp != null)
            {
                if (isHit == false)
                {
                    enemyHp.TakeDamage(50);
                    boomParticles.transform.position = enemyHp.transform.position;
                    boomParticles.Play();
                    isHit = true;
                }
                isHit = false;
            }
        }
    }
}
