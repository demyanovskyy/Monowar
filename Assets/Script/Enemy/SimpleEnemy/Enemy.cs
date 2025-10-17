using UnityEngine;

public class Enemy : Entety
{
    public EnemyPhysicsControl physicsControl;

    public FieldOfView2D fieldOfViev;
    public RotateToTargetWithProperFlipAndGizmos rotateObject;


    private void Awake()
    {
        stateMachine = new StateMachine();
        abilitys = GetComponents<BaseAbility>();
        stateMachine.arrayOfAbilities = abilitys;
    }



    private void Update()
    {
        foreach (BaseAbilityEnemy ability in abilitys)
        {
            if (ability.thisAbilityState == stateMachine.curentState)
            {
                ability.ProcessAbility();
            }
            ability.UpdateAnimator();
        }
        // Debug.Log("Curent state is:" + gameObject.name + ":" + stateMachine.curentState);



        if (fieldOfViev != null && rotateObject != null)
        {

            if (fieldOfViev.targetInSight)
            {
                rotateObject.SetIsRotate(true);
            }
            else
            {
                rotateObject.SetIsRotate(false);
            }
        }
    }

    public void DeactivateRotateobject()
    {
        if (rotateObject != null)
            rotateObject.SetIsRotate(false);
    }

    public void DeactivateFoV()
    {
        if (fieldOfViev != null)
            fieldOfViev.SetActive(false);

    }

    private void FixedUpdate()
    {
        foreach (BaseAbilityEnemy ability in abilitys)
        {
            if (ability.thisAbilityState == stateMachine.curentState)
            {
                ability.ProcessFixedAbility();
            }
        }
    }

    public override void ForceFlip()
    {
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
    }

    public bool GetFacingDerection()
    {
        return facingRight;
    }
}
