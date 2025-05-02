using UnityEngine;
using System.Collections.Generic;

public class PlayerController : Singleton<PlayerController>
{
    private PlayerObj playerObj;
    private Rigidbody2D rb;
    private SPUM_Prefabs anim;
    private Animator animator;

    public float moveSpeed = 8f;

    private bool isAttacking = false;
    private float attackCooldown = 0.5f;
    private float attackTimer = 0f;

    private Vector2 lastMovementDirection = Vector2.right;

    private Weapon currentWeapon;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerObj = GetComponent<PlayerObj>();
        anim = GetComponentInChildren<SPUM_Prefabs>();
        animator = anim.GetComponentInChildren<Animator>();
        UpdateCurrentWeapon();
        Debug.Log(currentWeapon);
    }

    private void Update()
    {
        HandleMovement();
        UpdateCurrentWeapon();
        HandleActions();
        HandleAttackCooldown();
        RotateGunTowardsMouse();

        if (currentWeapon != null)
            currentWeapon.UpdateWeapon();
    }

    private void HandleMovement()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        if (input.magnitude > 0 && !isAttacking)
        {
            playerObj._playerState = PlayerObj.PlayerState.move;
            transform.position += (Vector3)(input * moveSpeed * Time.deltaTime);
            lastMovementDirection = input;
            FlipCharacter(input.x);
            anim.PlayAnimation(1);
        }
        else if (!isAttacking)
        {
            playerObj._playerState = PlayerObj.PlayerState.idle;
            FlipCharacter(lastMovementDirection.x);
            anim.PlayAnimation(0);
        }
    }

    private void UpdateCurrentWeapon()
    {
        Transform weaponParent = GameObject.Find("P_WeaponP")?.transform;
        if (weaponParent != null)
        {
            foreach (Transform child in weaponParent)
            {
                if (child.gameObject.activeSelf)
                {
                    currentWeapon = child.GetComponent<Weapon>();
                    return;
                }
            }
            currentWeapon = null;
        }
    }
    private void HandleActions()
    {
        bool isPrimaryFireHeld = Input.GetMouseButton(0);
        bool isPrimaryFireJustPressed = Input.GetMouseButtonDown(0);

        if (currentWeapon != null)
        {
            bool shouldTryFire = currentWeapon is Gun ? isPrimaryFireHeld : isPrimaryFireJustPressed;
            currentWeapon.TryAttack(shouldTryFire);

            if (currentWeapon.IsAttacking)
            {
                playerObj._playerState = PlayerObj.PlayerState.attack;

                if (currentWeapon is Melee)
                    anim.PlayAnimation(4);
                else if (currentWeapon is Staff)
                    anim.PlayAnimation(6);
            }
            else
            {
                animator.ResetTrigger("Attack");
            }
        }

        if (Input.GetMouseButtonDown(1) && currentWeapon is Melee)
        {
            playerObj._playerState = PlayerObj.PlayerState.stun;
            anim.PlayAnimation(7);
            AudioManager.Instance.PlaySFX("SwordSpecialAttack");
            //stun bude fungovat pouze na enemies v nejake oblasti

            EnemyObj[] enemiesArray = FindObjectsByType<EnemyObj>(FindObjectsSortMode.None);
            List<EnemyObj> enemiesList = new List<EnemyObj>(enemiesArray);

            foreach (EnemyObj enemy in enemiesList)
            {
                var enemyHealth = enemy.GetComponent<EnemyHpSystem>();
                enemyHealth.Stun();
            }
        }
    }
    private void HandleAttackCooldown()
    {
        if (isAttacking)
        {
            attackTimer -= Time.deltaTime;

            if (attackTimer <= 0f)
            {
                isAttacking = false;

                if (rb.linearVelocity.magnitude == 0)
                {
                    playerObj._playerState = PlayerObj.PlayerState.idle;
                    FlipCharacter(lastMovementDirection.x);
                    anim.PlayAnimation(0);
                }
            }
        }
    }

    private void FlipCharacter(float directionX)
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (mouseWorldPos.x > transform.position.x)
        {
            anim.transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (mouseWorldPos.x < transform.position.x)
        {
            anim.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    private void RotateGunTowardsMouse()
    {
        if (currentWeapon is Gun)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = mousePos - currentWeapon.transform.position;
            direction.z = 0;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            currentWeapon.transform.rotation = Quaternion.Euler(0, 0, angle);

            Vector3 gunScale = currentWeapon.transform.localScale;
            gunScale.y = (angle > 90 || angle < -90) ? -1 : 1;
            currentWeapon.transform.localScale = gunScale;

            if (mousePos.x > transform.position.x)
            {
                currentWeapon.transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (mousePos.x < transform.position.x)
            {
                currentWeapon.transform.localScale = new Vector3(1, -1, 1);
            }
        }
    }

    public Weapon GetCurrentWeapon()
    {
        return currentWeapon;
    }
}
