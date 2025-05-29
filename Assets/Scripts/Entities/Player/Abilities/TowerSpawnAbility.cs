using UnityEngine;
[CreateAssetMenu]
public class TowerSpawnAbility : Ability
{
    [SerializeField] GameObject tower;
    float randX = 0;
    float randY = 0;
    public override void Activate(GameObject parent)
    {
        AbilityHolder abilityHolder = parent.GetComponent<AbilityHolder>();

        Instantiate(tower, RandomSpawnPos(), Quaternion.identity);
        abilityHolder.isReset = false;
    }

    Vector2 RandomSpawnPos()
    {
        while (randX == 0 || randY == 0 || (randX == 0 && randY == 0))
        {
            randX = Random.Range(-4, 4);
            randY = Random.Range(-4, 4);
        }
        return new Vector2(randX, randY);
    }
}
