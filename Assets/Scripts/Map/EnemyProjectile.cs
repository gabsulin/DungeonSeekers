using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    Animator anim;
    [SerializeField] ParticleSystem destroyParticles;
    void Start()
    {
        anim = GetComponent<Animator>();
        if(gameObject != null) Destroy(gameObject, 3);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Collision"))
        {
            if (anim != null)
            {
                if (anim.GetBool("Hit") == true)
                {
                    //anim.SetBool("Hit", true);
                    gameObject.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0, 0);
                    Destroy(gameObject, 1);
                }
                //var spawnedParticles = Instantiate(destroyParticles, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
            else
            {
                if (destroyParticles != null)
                {
                    ParticleSystem spawnedParticles = Instantiate(destroyParticles, transform.position, Quaternion.identity);
                    spawnedParticles.Play();
                    Destroy(spawnedParticles.gameObject, spawnedParticles.main.duration);
                }
                Destroy(gameObject);
            }

        }
    }
}
