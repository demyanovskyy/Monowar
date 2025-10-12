using UnityEngine;

public class SimpleEnemyStats : EnemyStats
{
    [SerializeField] protected Enemy enemy;

    protected override void DamageProcess()
    {
        
    }

    protected override void DeathProcess()
    {
        enemy.stateMachine.ChangeState((int)EnemyStates.State.Death);
    }
}
