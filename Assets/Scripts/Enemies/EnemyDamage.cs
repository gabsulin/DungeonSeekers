using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    private PlayerObj player;
    [SerializeField] ParticleSystem boomParticles;
    void Start()
    {
        player = GetComponentInParent<PlayerObj>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    //zkusit udelat misto ontriggerenter oncollisionenter aby enemies nemuseli mit ontrigger a tim padem by se vyresila kolize mezi nima


    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        //enemy ma taky PlayerObj a tim padem je playerstate na attack a nefunguje podminka tak jak ma
        if (collision.CompareTag("Enemy") && player.objType == PlayerObj.ObjType.Player && player._playerState == PlayerObj.PlayerState.attack)
        {
            EnemyHpSystem enemyHp = collision.GetComponent<EnemyHpSystem>();
            if (enemyHp != null)
            {
                enemyHp.TakeDamage(50);
                boomParticles.transform.position = enemyHp.transform.position;
                boomParticles.Play();
            }
        }
    }*/

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy") && player._playerState == PlayerObj.PlayerState.attack)
        {
            EnemyHpSystem enemyHp = collision.collider.GetComponent<EnemyHpSystem>();
            if (enemyHp != null)
            {
                enemyHp.TakeDamage(50);
                boomParticles.transform.position = enemyHp.transform.position;
                boomParticles.Play();
            }
        }
    }
}
