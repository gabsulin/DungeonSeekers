using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] GameObject troopPrefab;
    [SerializeField] Transform troopSpawn;

    Animator animator;
    GameObject spawnedTroopInstance;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SpawnTroop()
    {
        if (troopPrefab != null && troopSpawn != null)
        {
            spawnedTroopInstance = Instantiate(troopPrefab, troopSpawn.position, Quaternion.identity);
            Debug.Log("spawned");
        }
    }

    public void DestroyTower() //will be called from ability script when time runs out
    {
        if (animator != null)
        {
            animator.SetTrigger("Destroy");
        }
        else
        {
            DestroyComplete();
        }

        if (spawnedTroopInstance != null)
        {
            Destroy(spawnedTroopInstance);
        }
    }

    public void DestroyComplete()
    {
        Destroy(gameObject);
    }
}