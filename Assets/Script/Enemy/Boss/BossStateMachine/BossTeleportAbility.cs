using UnityEngine;

public class BossTeleportAbility : BaseAbilityBoss
{
    private Player player;

    private string teleportOutAnimParamiterName = "TeleportOut";
    private int teleportOutParamiterID;
    private string teleportInAnimParamiterName = "TeleportIn";
    private int teleportInParamiterID;

    private string idleAnimParamiterName = "Idle";
    private int idleParamiterID;

    [SerializeField] private float minTeleportTime;
    [SerializeField] private float maxTeleportTime;
    [SerializeField] private Transform[] teleportPoints;
    private int teleportIndex;
    private int lastTeleportIndex;
    private bool canChecTeleportInfo;
    private float teleportStartTime;

    private enum AnimStats
    {
        Idle,
        In,
        Out
    }

    private AnimStats currentAnim;

    protected override void Initialization()
    {
        base.Initialization();
        teleportOutParamiterID = Animator.StringToHash(teleportOutAnimParamiterName);
        teleportInParamiterID = Animator.StringToHash(teleportInAnimParamiterName);

        idleParamiterID = Animator.StringToHash(idleAnimParamiterName);

        teleportStartTime = Random.Range(minTeleportTime, maxTeleportTime);

        player = FindAnyObjectByType<Player>();
    }

    public override void EnterAbility()
    {
        linkedPhysics.DesableStateCollider();

        teleportIndex = Random.Range(0, teleportPoints.Length);
        while (teleportIndex == lastTeleportIndex)
        {
            teleportIndex = Random.Range(0, teleportPoints.Length);
        }
        lastTeleportIndex = teleportIndex;


        currentAnim = AnimStats.Out;

    }

    public override void ExitAbility()
    {
        canChecTeleportInfo = false;
    }

    public override void ProcessAbility()
    {
        if (!isParamited)
            return;

        if (!canChecTeleportInfo)
            return;

        teleportStartTime -= Time.deltaTime;

        if (linkedPhysics.inAttackRange)
        {
            // do melee attack
            linkedStateMachine.ChangeState((int)BossStates.State.MeleeAttack);
        }
        else
        if (teleportStartTime <= 0)
        {

            // do range attack or else
            linkedStateMachine.ChangeState((int)BossStates.State.RangeAttack);
        }

    }

    public void Teleport()
    {
        int randomChance = Random.Range(0, 2); //50/50
        if (randomChance == 0)
        {
            transform.position = teleportPoints[teleportIndex].position;
        }
        else
        {
            if (player != null)
                transform.position = player.transform.position + Vector3.up * 1.6f;
            else
                transform.position = teleportPoints[teleportIndex].position;
        }
        currentAnim = AnimStats.In; 
    }

    public void EnableCheclTeleport()
    {
        canChecTeleportInfo = true;
        teleportStartTime = Random.Range(minTeleportTime, maxTeleportTime);

        linkedPhysics.EnableStateCollider();
        linkedPhysics.EnableAttackDetectionCollider();

        currentAnim = AnimStats.Idle; 

    }

    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool(idleParamiterID, linkedStateMachine.curentState == (int)BossStates.State.Teleport && currentAnim == AnimStats.Idle);
        linkedAnimator.SetBool(teleportInParamiterID, linkedStateMachine.curentState == (int)BossStates.State.Teleport && currentAnim == AnimStats.In);
        linkedAnimator.SetBool(teleportOutParamiterID, linkedStateMachine.curentState == (int)BossStates.State.Teleport && currentAnim == AnimStats.Out);
    }
}
