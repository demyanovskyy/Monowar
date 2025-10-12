using System.Collections;
using UnityEngine;

public class BossStats : EnemyStats
{
    [SerializeField] protected Boss boss;
    [SerializeField] private HealthBarControl bossHeathBar;
    protected override void DamageProcess()
    {
        bossHeathBar.SetSliderValue(health, maxHealth);
    }

    protected override void DeathProcess()
    {
        boss.stateMachine.ChangeState((int)BossStates.State.Death);
    }

}
