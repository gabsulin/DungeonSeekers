using UnityEngine;

public class BossMove : StateMachineBehaviour
{
    public float speed = 2.5f;
    public float attackRange = 15f;

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
        float x = rb.position.x;
        float y = rb.position.y;

        boss.LookAtPlayer();
        float distance = Vector2.Distance(player.position, rb.position);

        Vector2 target = new Vector2(player.position.x, player.position.y);
        if (distance > attackRange)
        {
            Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
            rb.MovePosition(newPos);
        }
        else if (distance <= attackRange)
        {
            animator.SetTrigger("Attack");
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
    }


}
