using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScene : MonoBehaviour
{
    [SerializeField] EnemyObj player;
    [SerializeField] EnemyObj enemy;
    [SerializeField] Animator playerAnim;
    [SerializeField] Animator enemyAnim;
    [SerializeField] Transform playerGoalPos;
    [SerializeField] Transform enemyGoalPos;

    Vector2 playerGoal;
    Vector2 enemyGoal;

    int nextScene = 1;
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
            StartCoroutine(PlayAttackAnimation(playerAnim));
        }

        if (Mathf.Abs(enemy.transform.position.x - enemyGoal.x) < 0.5f)
        {
            StartCoroutine(PlayAttackAnimation(enemyAnim));
        }
    }

    private IEnumerator PlayAttackAnimation(Animator anim)
    {
        anim.SetBool("isOnPosition", true);

        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length / 2);

        anim.speed = 0.2f;

        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length * 3.15f);
        SceneManager.LoadScene(nextScene);
    }

}
