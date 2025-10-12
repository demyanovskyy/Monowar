using UnityEngine;

public class Entety : MonoBehaviour
{

    public StateMachine stateMachine;

    //public PlayerPhysicsControl physicsControl;

    public Animator animator;

    protected BaseAbility[] abilitys;

    public bool facingRight = true;


    public virtual void Flip()
    {
    }

    public virtual void ForceFlip()
    {
 
    }



}
