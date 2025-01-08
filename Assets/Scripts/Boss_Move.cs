using UnityEngine;

public class Boss_Move : StateMachineBehaviour
{
    public float speed = 2.5f;
    public float attackRange = 5f;

    Transform player;
    Rigidbody2D rb;
    BossFlip boss;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        boss = animator.GetComponent<BossFlip>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss.LookAtPlayer();
        float distance = Vector2.Distance(player.position, rb.position);

        Vector2 target = new Vector2(player.position.x, player.position.y);
        if(distance > attackRange)
        {
            Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
            rb.MovePosition(newPos);
        }
        else if(distance <= attackRange)
        {
            animator.SetTrigger("Attack");
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
    }


}
