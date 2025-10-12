using UnityEngine;

public class BaseAbilityEnemy : BaseAbility
{

    protected Enemy enemy;

    protected EnemyPhysicsControl linkedPhysics;

    public EnemyStates.State abilityID;

    private void Awake()
    {
        thisAbilityState = (int)abilityID;
    }
    protected override void Initialization()
    {
        base.Initialization();
        
        enemy = GetComponent<Enemy>();

        linkedPhysics = enemy.physicsControl;

    }
}
