using UnityEngine;

public class BaseAbility: MonoBehaviour
{
    protected Entety entety;

    
    protected StateMachine linkedStateMachine;
    //protected PlayerPhysicsControl linkedPhysics;
    protected Animator linkedAnimator;

    [HideInInspector] public int  thisAbilityState;

    public bool isParamited = true;

    protected virtual void Start()
    {
        Initialization();
    }

    public virtual void EnterAbility()
    {

    }

    public virtual void ExitAbility()
    {

    }

    public virtual void ProcessAbility()
    {

    }

    public virtual void ProcessFixedAbility()
    {

    }

    public virtual void UpdateAnimator()
    {

    }

    protected virtual void Initialization()
    {
        entety = GetComponent<Entety>();

        linkedStateMachine = entety.stateMachine;
        //linkedPhysics = entety.physicsControl;
        linkedAnimator = entety.animator;

     }
 
}
