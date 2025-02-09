using System.Collections;
using UnityEngine;

public class MenuScene : MonoBehaviour
{
    [SerializeField] PlayerObj player;
    [SerializeField] EnemyObj enemy;
    [SerializeField] Animator playerAnim;
    [SerializeField] Animator enemyAnim;
    [SerializeField] Transform playerGoalPos;
    [SerializeField] Transform enemyGoalPos;

    Vector2 playerGoal;
    Vector2 enemyGoal;
    void Start()
    {
        playerGoal = new Vector2 (-1.8f, -2);
        enemyGoal = new Vector2 (1.8f, -2);
        player.SetMovePos(playerGoal);
        enemy.SetMovePos(enemyGoal);
    }

    void Update()
    {
        if (Mathf.Abs(player.transform.position.x - playerGoal.x) < 0.5f)
        {
            playerAnim.SetBool("isOnPosition", true);
        }

        if (Mathf.Abs(enemy.transform.position.x - enemyGoal.x) < 0.5f)
        {
            enemyAnim.SetBool("isOnPosition", true);
        }
    }

    private IEnumerator PlayAttackAnimation(Animator anim)
    {
        anim.SetBool("isOnPosition", true);

        anim.speed = 2f;
        yield return new WaitForSeconds(0.1f);

        anim.speed = 0.5f;
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length / anim.speed - 0.1f);

        //anim.speed = 1f;
    }

}
