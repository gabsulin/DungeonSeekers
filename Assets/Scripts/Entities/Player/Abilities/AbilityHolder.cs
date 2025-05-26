using UnityEngine;
using UnityEngine.UI;

public class AbilityHolder : MonoBehaviour
{
    public Ability ability;
    float cooldownTime;
    float activeTime;
    [SerializeField] Image abilityBar;

    Rigidbody2D rb;
    PlayerHpSystem playerHp;
    enum AbilityState
    {
        ready,
        active,
        cooldown
    }

    AbilityState state = AbilityState.ready;

    public KeyCode key;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerHp = GetComponent<PlayerHpSystem>();
    }

    public void AssignBar()
    {
        GameObject canvas = GameObject.Find("Canvas");
        if (canvas != null) abilityBar = canvas.transform.Find("AbilityBar/BarHolder/Bar").GetComponent<Image>();
        else Debug.Log("Smula");
    }
    public void UpdateBar()
    {
        if (abilityBar != null) abilityBar.fillAmount = 1f;
    }
    void Update()
    {
        switch(state)
        {
            case AbilityState.ready:
                if (Input.GetKeyDown(key))
                {
                    ability.Activate(gameObject);
                    state = AbilityState.active;
                    activeTime = ability.activeTime;
                }
                break;
            case AbilityState.active:
                if (activeTime > 0)
                {
                    activeTime -= Time.deltaTime;
                    abilityBar.fillAmount = activeTime / ability.activeTime;
                }
                else
                {
                    state = AbilityState.cooldown;
                    cooldownTime = ability.coolDownTime;
                }
                break;
            case AbilityState.cooldown:
                if (cooldownTime > 0)
                {
                    cooldownTime -= Time.deltaTime;

                    float elapsedCooldown = ability.coolDownTime - cooldownTime;
                    abilityBar.fillAmount = elapsedCooldown / ability.coolDownTime;

                    if (ability is DashAbility)
                    {
                        rb.linearVelocity = Vector2.zero;
                        playerHp.isImmune = false;
                    }
                    if (ability is ImmuneAbility) playerHp.isImmune = false;
                    //faster fire rate, faster movement, healing, homing bullets, 
                }
                else
                {
                    state = AbilityState.ready;
                    abilityBar.fillAmount = 1f;
                }
                break;
        }
    }
}
