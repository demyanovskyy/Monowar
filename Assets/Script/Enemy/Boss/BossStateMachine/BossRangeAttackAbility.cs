using UnityEngine;

public class BossRangeAttackAbility : BaseAbilityBoss
{
    private string rangeAttackAnimParamiterName = "RangeAttack";
    private int rangeAttackParamiterID;

    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform shootingPoint;

    private Player player;


    public void SpawnProjectile()
    {
        BossProjectile projectile = Instantiate(projectilePrefab, shootingPoint.position, transform.rotation).GetComponent<BossProjectile>();

        if (player != null)
        {
            projectile.MoveProjectile(player.transform);
        }
        else
        {
            Destroy(projectile.gameObject);
        }

    }

    protected override void Initialization()
    {
        base.Initialization();
        rangeAttackParamiterID = Animator.StringToHash(rangeAttackAnimParamiterName);

        player = FindAnyObjectByType<Player>();
    }

    public override void EnterAbility()
    {

    }


    public override void ProcessAbility()
    {
        if (!isParamited)
            return;


    }

    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool(rangeAttackParamiterID, linkedStateMachine.curentState == (int)BossStates.State.RangeAttack);
    }

 
}
