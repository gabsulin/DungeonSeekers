using UnityEngine;

public class AttackController : MonoBehaviour
{
    Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    public void StartAttack()
    {
        anim.SetTrigger("Attack");
        PlayerController.Instance.isAttacking = true;
    }

    public void ResetAttack()
    {
        anim.ResetTrigger("Attack");
        PlayerController.Instance.isAttacking = false;
    }
}
