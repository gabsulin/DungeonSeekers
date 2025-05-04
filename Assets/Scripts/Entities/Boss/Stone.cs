using UnityEngine;

public class Stone : MonoBehaviour
{
    PlayerController player;
    SPUM_Prefabs anim;
    Animator animator;

    public bool isPetrified;

    private void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
        anim = FindFirstObjectByType<SPUM_Prefabs>();
        animator = anim.GetComponentInChildren<Animator>();
    }
    public void EnableActions()
    {
        player.canMove = true;
        player.canAttack = true;
        isPetrified = false;
    }

    public void DisableActions()
    {
        player.canMove = false;
        animator.SetFloat("RunState", 0);
        player.canAttack= false;
        animator.ResetTrigger("Attack");
        isPetrified = true;
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}
