using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    private PlayerObj player;
    private EnemyMovement enemy;
    [SerializeField] ParticleSystem boomParticles;
    void Start()
    {
        player = FindAnyObjectByType<PlayerObj>();
        enemy = FindAnyObjectByType<EnemyMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //enemy ma taky PlayerObj a tim padem je playerstate na attack a nefunguje podminka tak jak ma
        if(collision.CompareTag("Enemy") && player._playerState == PlayerObj.PlayerState.attack && player.objType == PlayerObj.ObjType.Player) 
        {
            HpSystem enemy = collision.GetComponent<HpSystem>();
            if(enemy != null )
            {
                enemy.TakeDamage(50);
                boomParticles.transform.position = enemy.transform.position;
                boomParticles.Play();
            }
        }
    }
}
