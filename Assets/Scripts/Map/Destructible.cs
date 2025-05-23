using UnityEngine;

public class Destructible : MonoBehaviour
{
    PlayerObj player;
    [SerializeField] ParticleSystem destroyParticles;

    private void Start()
    {
        player = FindFirstObjectByType<PlayerObj>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Melee") && player._playerState == PlayerObj.PlayerState.attack)
        {
            destroyParticles.transform.position = gameObject.transform.position;
            destroyParticles.Play();
            Destroy(gameObject, 0.1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            destroyParticles.transform.position = transform.position;
            destroyParticles.Play();
            Destroy(gameObject, 0.1f);
        }
    }
}
