using UnityEngine;

public class FlyEnemyStat : EnemyStats
{
    [SerializeField] protected FlyEnemy flyEnemy;
    [SerializeField] protected GameObject destroyGameObject;
    
 
    protected override void DamageProcess()
    {
        
    }

    protected override void DeathProcess()
    {
        
        flyEnemy.stateMachine.ChangeState((int)FlyEnemyStates.State.Death);

        Destroy(destroyGameObject, 2f);
    }
}
