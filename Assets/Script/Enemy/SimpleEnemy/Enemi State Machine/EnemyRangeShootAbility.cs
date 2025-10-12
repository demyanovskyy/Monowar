using System.Collections;
using UnityEngine;

public class EnemyRangeShootAbility : BaseAbilityEnemy
{
    private string shootAnimParamiterName = "Shoot";
    private int shootParamiterID;

    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float rayLength;
    [SerializeField] private float damage;
    [SerializeField] private float shootCooldown;
    [SerializeField] private float visibleLineTime;
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private GameObject hitEffectPrefab;
    [SerializeField] private LayerMask whatToHit;

    public void EndOfAttack()
    {
        if (linkedPhysics.playerAhead)
        {
            linkedAnimator.Play(shootParamiterID, 0, 0);
        }
        else
        {
            linkedStateMachine.ChangeState((int)EnemyStates.State.Idle);
        }

        StartCoroutine(CheckBehindDelay());

    }

    public void Shoot()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(shootingPoint.position, transform.right, rayLength, whatToHit);
        lineRenderer.positionCount = 2;
        if (hitInfo)
        {
            lineRenderer.SetPosition(0, shootingPoint.position);
            lineRenderer.SetPosition(1, hitInfo.point);

            
            Vector2 normal = hitInfo.normal;
            float angle = Mathf.Atan2(normal.y, normal.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Instantiate(hitEffectPrefab, hitInfo.point, rotation);


            PlayerStats playerStats = hitInfo.collider.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.DamagePlayer(damage);
            }
        }
        else
        {
            lineRenderer.SetPosition(0, shootingPoint.position);
            lineRenderer.SetPosition(1, shootingPoint.position + transform.right * 20);
        }

        StartCoroutine(ResetShootLine());
    }

    private IEnumerator ResetShootLine()
    {
        yield return new WaitForSeconds(visibleLineTime);
        lineRenderer.positionCount = 0;
    }


    IEnumerator CheckBehindDelay()
    {
        yield return new WaitForSeconds(linkedPhysics.behindDelay);
        linkedPhysics.canCheckBehind = true;
    }

    protected override void Initialization()
    {
        base.Initialization();
        shootParamiterID = Animator.StringToHash(shootAnimParamiterName);

    }


    public override void EnterAbility()
    {
        linkedPhysics.ResetVelocity();
        linkedPhysics.canCheckBehind = false;
    }

    public override void ProcessFixedAbility()
    {
    }


    public override void ProcessAbility()
    {
        if (!isParamited)
            return;

    }

    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool(shootParamiterID, linkedStateMachine.curentState == (int)EnemyStates.State.Shoot);
    }
}
