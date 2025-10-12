using UnityEngine;

public class BaseAbilityBoss : BaseAbility
{
    protected Boss boss;

    protected BossPhysicsControl linkedPhysics;

    public BossStates.State abilityID;

    private void Awake()
    {
        thisAbilityState = (int)abilityID;
    }
    protected override void Initialization()
    {
        base.Initialization();

        boss = GetComponent<Boss>();

        linkedPhysics = boss.physicsControl;

    }
}
