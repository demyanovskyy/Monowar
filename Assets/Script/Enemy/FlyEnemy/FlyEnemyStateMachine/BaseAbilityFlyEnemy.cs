using UnityEngine;

public class BaseAbilityFlyEnemy : BaseAbility
{
    protected FlyEnemy flyEnemy;

    protected FlyEnemyPhysicControl linkedPhysics;

    public FlyEnemyStates.State abilityID;



    private void Awake()
    {
        thisAbilityState = (int)abilityID;
    }
    protected override void Initialization()
    {
        base.Initialization();

        flyEnemy = GetComponent<FlyEnemy>();

        linkedPhysics = flyEnemy.physicsControl;

    }
}
