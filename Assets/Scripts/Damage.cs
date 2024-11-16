using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    private PlayerObj player;
    void Start()
    {
        player = FindAnyObjectByType(typeof(PlayerObj)) as PlayerObj;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy") && player._playerState == PlayerObj.PlayerState.attack)
        {
            HpSystem enemy = collision.GetComponent<HpSystem>();
            if(enemy != null )
            {
                enemy.TakeDamage(50);
            }
        }
    }
}
