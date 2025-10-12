

public class StateMachine 
{
    public int previousState;
    public int curentState;
    public BaseAbility[] arrayOfAbilities;



    public void ChangeState(int newState)
    {
        foreach (BaseAbility ability in arrayOfAbilities)
        {
            if (ability.thisAbilityState == newState)
            {
                if (!ability.isParamited)
                {
                    return;
                }

            }
        }



        foreach (BaseAbility ability in arrayOfAbilities)
        {
            if (ability.thisAbilityState == curentState)
            {
                ability.ExitAbility();
                previousState = curentState;
            }
        }


        foreach (BaseAbility ability in arrayOfAbilities)
        {
            if (ability.thisAbilityState == newState)
            {
                if (ability.isParamited)
                {
                    curentState = newState;
                    ability.EnterAbility();
                }
                break;
            }
        }
    }


    public void ForceChange(int newState)
    {
        previousState = curentState;
        curentState = newState;
    }

}
