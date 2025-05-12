using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;

public class AbilityHolder : MonoBehaviour
{
    public Ability ability;
    float cooldownTime;
    float activeTime;
    public Image abilityBar;

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

                    if (ability is DashAbility) rb.linearVelocity = Vector2.zero;
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
