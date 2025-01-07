using UnityEngine;

public class Destructible : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Melee"))
        {
            Destroy(gameObject);
            Debug.Log("Hit");
        }
    }
}
