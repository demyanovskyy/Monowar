using UnityEngine;

public class BossDeathAbility : BaseAbilityBoss
{
    private string deathAnimParamiterName = "Death";
    private int deathParamiterID;

    [SerializeField] private GameObject headPrefab;
    [SerializeField] private Transform headPosition;



    public void DeathAnimationEvent()
    {
        Instantiate(headPrefab, headPosition.position, transform.rotation);
        gameObject.SetActive(false);
    }

    protected override void Initialization()
    {
        base.Initialization();
        deathParamiterID = Animator.StringToHash(deathAnimParamiterName);
        
    }

    public override void EnterAbility()
    {
        linkedPhysics.DesableAllColliders();
    }
    public override void ProcessAbility()
    {
        if (!isParamited)
            return;

    }

    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool(deathParamiterID, linkedStateMachine.curentState == (int)BossStates.State.Death);
    }
}
