using UnityEngine;

public class FlyEnemy : Entety
{
    public FlyEnemyPhysicControl physicsControl;

    private void Awake()
    {
        stateMachine = new StateMachine();
        abilitys = GetComponents<BaseAbility>();
        stateMachine.arrayOfAbilities = abilitys;
    }


    private void Update()
    {
        foreach (BaseAbilityFlyEnemy ability in abilitys)
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
        foreach (BaseAbilityFlyEnemy ability in abilitys)
        {
            if (ability.thisAbilityState == stateMachine.curentState)
            {
                ability.ProcessFixedAbility();
            }
        }
    }

    public override void ForceFlip()
    {
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
    }
}
