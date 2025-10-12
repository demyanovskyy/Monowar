using UnityEngine;

public class Boss : Entety
{
    public BossPhysicsControl physicsControl;

 
    private void Awake()
    {
        stateMachine = new StateMachine();
        abilitys = GetComponents<BaseAbility>();
        stateMachine.arrayOfAbilities = abilitys;
    }


    private void Update()
    {
        foreach (BaseAbilityBoss ability in abilitys)
        {
            if (ability.thisAbilityState == stateMachine.curentState)
            {
                ability.ProcessAbility();
            }
            ability.UpdateAnimator();
        }
        Debug.Log("Curent state is:" + gameObject.name + ":" + stateMachine.curentState);
    }
    private void FixedUpdate()
    {
        foreach (BaseAbilityBoss ability in abilitys)
        {
            if (ability.thisAbilityState == stateMachine.curentState)
            {
                ability.ProcessFixedAbility();
            }
        }
    }
}
