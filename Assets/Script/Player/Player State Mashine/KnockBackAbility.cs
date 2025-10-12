using System.Collections;
using UnityEngine;
using UnityEngine.Diagnostics;

public class KnockBackAbility : BaseAbilityPlayer
{
    private Coroutine currentKnockBack;

    public override void ExitAbility()
    {
        currentKnockBack = null;
    }

    public void StartKnockBack(float duration, Vector2 force, Transform enemyObject)
    {
        if (player.playerStats.GetCanTakeDamage() == false)
            return;


        if(currentKnockBack == null)
        {
            currentKnockBack = StartCoroutine(KnockBack(duration,force,enemyObject));
        }
        else
        {
            // do nothing
            // or 
            StopCoroutine(currentKnockBack);
            currentKnockBack = StartCoroutine(KnockBack(duration, force, enemyObject));

        }
    }

    public void StartSwingKnockBack(float duration, Vector2 force, int direction)
    {
        if (player.playerStats.GetCanTakeDamage() == false)
            return;


        if (currentKnockBack == null)
        {
            currentKnockBack = StartCoroutine(SwingKnockBack(duration, force, direction));
        }
        else
        {
            // do nothing
            // or 
            //StopCoroutine(currentKnockBack);
            //currentKnockBack = StartCoroutine(KnockBack(duration, force, enemyObject));

        }
    }

    private string knockBackAnimParamiterName = "KnockBack";
    private int knockBackParamiterID;

    protected override void Initialization()
    {
        base.Initialization();
        knockBackParamiterID = Animator.StringToHash(knockBackAnimParamiterName);
    }

    public override void EnterAbility()
    {
        entety.Flip();
    }

    public IEnumerator KnockBack(float duration, Vector2 force, Transform enemyObject)
    {
        linkedStateMachine.ChangeState((int)PlayerStates.State.KnockBack);
        linkedPhysics.ResetVelocity();

        if (transform.position.x >= enemyObject.transform.position.x)
        {
            linkedPhysics.rb.linearVelocity = force;
        }
        else
        {
            linkedPhysics.rb.linearVelocity = new Vector2(-force.x, force.y);
        }

        yield return new WaitForSeconds(duration);

        // return to othe states ==============

        if (player.playerStats.GetCurrentHealth() > 0)
        {
            if(linkedPhysics.grounded)
            {
                if(linkedInput.horizontalInput !=0)
                    linkedStateMachine.ChangeState((int)PlayerStates.State.Move);
                else
                    linkedStateMachine.ChangeState((int)PlayerStates.State.Idle);
            }
            else
            {
                linkedStateMachine.ChangeState((int)PlayerStates.State.Jump);
            }

        }
        else
        {
            linkedStateMachine.ChangeState((int)PlayerStates.State.Death);
        }

    }

    public IEnumerator SwingKnockBack(float duration, Vector2 force, int direction)
    {
        linkedStateMachine.ChangeState((int)PlayerStates.State.KnockBack);
        linkedPhysics.ResetVelocity();

        force.x *= direction;
        linkedPhysics.rb.linearVelocity = force;

        yield return new WaitForSeconds(duration);

        // return to othe states ==============

        if (player.playerStats.GetCurrentHealth() > 0)
        {
            if (linkedPhysics.grounded)
            {
                if (linkedInput.horizontalInput != 0)
                    linkedStateMachine.ChangeState((int)PlayerStates.State.Move);
                else
                    linkedStateMachine.ChangeState((int)PlayerStates.State.Idle);
            }
            else
            {
                linkedStateMachine.ChangeState((int)PlayerStates.State.Jump);
            }

        }
        else
        {
            linkedStateMachine.ChangeState((int)PlayerStates.State.Death);
        }

    }


    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool(knockBackParamiterID, linkedStateMachine.curentState == (int)PlayerStates.State.KnockBack);
    }
}
