using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading;
using UnityEditor.SceneManagement;

public class PlayerHpSystem : MonoBehaviour
{
    PlayerObj player;
    SPUM_Prefabs anim;
    PlayerController playerController;
    public CharacterData characterData;

    [SerializeField] Image hpBar;
    [SerializeField] Image shieldsBar;
    [SerializeField] TMP_Text hpTMP;
    [SerializeField] TMP_Text shieldsTMP;
    [SerializeField] TMP_Text damageNumber;
    GameObject deathScreen;
    GameObject cam;

    [HideInInspector] public float currentHp;
    [HideInInspector] public float currentShields;
    [HideInInspector] public float maxHp;
    [HideInInspector] public float maxShields;

    private float wasntHit = 0f;
    private bool isRegeneratingShields = false;

    private float startShieldRegenTime = 5f;
    private float shieldRegenTime = 2f;

    public bool isDead;
    public bool isImmune;

    //poison
    public float poisonDamagePerSecond = 1f;
    public float poisonDuration = 5f;
    private Coroutine poisonCoroutine;

    private void Awake()
    {
        maxHp = characterData.health;
        maxShields = characterData.shields;
        currentHp = maxHp;
        currentShields = maxShields;

        isDead = false;
    }
    void Start()
    {
        player = GetComponent<PlayerObj>();
        anim = GetComponentInChildren<SPUM_Prefabs>();
        playerController = GetComponent<PlayerController>();
    }

    public void AssignUIElements()
    {
        cam = GameObject.FindGameObjectWithTag("Camera").gameObject;
        cam.SetActive(false);
        GameObject canvas = GameObject.Find("Canvas");

        if (canvas != null)
        {
            hpBar = canvas.transform.Find("HealthBar/Health").GetComponent<Image>();
            hpTMP = canvas.transform.Find("HealthBar/HpAmount").GetComponent<TMP_Text>();
            shieldsBar = canvas.transform.Find("Shieldbar/Shields").GetComponent<Image>();
            shieldsTMP = canvas.transform.Find("Shieldbar/ShieldsAmount").GetComponent<TMP_Text>();
            deathScreen = canvas.transform.Find("DeathScreen").gameObject;
            deathScreen.SetActive(false);
            Debug.Log("nasel se canvas a priradily se gameobjecty");
        }
        else
        {
            Debug.LogError("Canvas not found! Make sure your Canvas is named correctly.");
        }
    }

    public void UpdateUI()
    {
        if (hpBar != null && shieldsBar != null && hpTMP != null && shieldsTMP != null)
        {
            hpBar.fillAmount = currentHp / maxHp;
            hpTMP.text = $"{currentHp}/{maxHp}";
            shieldsBar.fillAmount = currentShields / maxShields;
            shieldsTMP.text = $"{currentShields}/{maxShields}";
            Debug.Log("UI elements updated");
        }
        else
        {
            Debug.LogError("UI Elements are not assigned in PlayerHpSystem!");
        }
    }

    private void Update()
    {
        wasntHit += Time.deltaTime;

        if (currentShields < maxShields && wasntHit >= startShieldRegenTime && !isRegeneratingShields && !isDead)
        {
            StartCoroutine(RegenerateShields());
        }
    }

    public void TakeHit(int damage)
    {
        AudioManager.Instance.PlaySFX("Hit");
        wasntHit = 0;
        if (!isImmune)
        {
            if (currentShields > 0)
            {
                int overflowDmg = damage - (int)currentShields;

                currentShields -= damage;
                /*damageNumber.text = damage.ToString();

                float randX = Random.Range(-0.5f, 0.5f);
                float randY = Random.Range(-0.5f, 0.5f);
                Vector2 offSet = new Vector2(randX, randY);
                Instantiate(damageNumber, (Vector2)transform.position + offSet, Quaternion.identity);*/

                if (currentShields <= 0) currentShields = 0;

                shieldsBar.fillAmount = currentShields / maxShields;
                shieldsTMP.text = $"{currentShields.ToString()}/{maxShields.ToString()}";

                if (overflowDmg > 0)
                {
                    currentHp -= overflowDmg;

                    hpBar.fillAmount = currentHp / maxHp;
                    hpTMP.text = $"{currentHp.ToString()}/{maxHp.ToString()}";

                    if (currentHp <= 0)
                    {
                        currentHp = 0;
                        hpTMP.text = $"{currentHp.ToString()}/{maxHp.ToString()}";
                        Die();
                    }
                }
            }
            else
            {
                currentHp -= damage;
                hpBar.fillAmount = currentHp / maxHp;
                hpTMP.text = $"{currentHp.ToString()}/{maxHp.ToString()}";
                if (currentHp <= 0)
                {
                    currentHp = 0;
                    hpTMP.text = $"{currentHp.ToString()}/{maxHp.ToString()}";
                    Die();
                }
            }
        }
        //Debug.Log(wasntHit);
    }

    private void Die()
    {
        if (player._playerState != PlayerObj.PlayerState.death)
        {
            anim._anim.ResetTrigger("Attack");
            anim._anim.SetFloat("RunState", 0f);
            anim._anim.SetFloat("AttackState", 0f);
            anim._anim.SetFloat("SkillState", 0f);

            player._playerState = PlayerObj.PlayerState.death;

            StartCoroutine(PlayDeathAnimation());

            anim._anim.SetTrigger("Die");
            anim._anim.SetBool("EditChk", anim.EditChk);
        }
        isDead = true;
        playerController.canMove = false;
        playerController.canAttack = false;

        UpgradeManager.Instance.ResetUpgrades();

        cam.SetActive(true);
        deathScreen.SetActive(true);

        Destroy(GameObject.Find("Player"));
        var persistance = GameObject.Find("Persistence");
        var inventory = persistance.transform.Find("InventoryCanvas(Clone)");
        Destroy(inventory.gameObject);
        //GameStats.Instance.ResetStats();
    }

    private IEnumerator PlayDeathAnimation()
    {
        anim._anim.speed = 1;
        anim.PlayAnimation(2);
        float animationLength = anim._anim.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animationLength / 2f / anim._anim.speed);
        anim._anim.speed = 1;
        yield return new WaitForSeconds(animationLength / 2f);
    }

    private IEnumerator RegenerateShields()
    {
        isRegeneratingShields = true;

        while (currentShields < maxShields && !isDead && wasntHit >= startShieldRegenTime)
        {
            currentShields += 1;
            shieldsBar.fillAmount = currentShields / maxShields;
            shieldsTMP.text = $"{currentShields.ToString()}/{maxShields.ToString()}";

            yield return new WaitForSeconds(shieldRegenTime);
        }

        currentShields = Mathf.Min(currentShields, maxShields);
        isRegeneratingShields = false;
    }
    public void ApplyShieldRechargeUpgrade(float multiplier)
    {
        shieldRegenTime *= 1f / multiplier;
        startShieldRegenTime *= 1f / multiplier * 2.5f;
    }
}