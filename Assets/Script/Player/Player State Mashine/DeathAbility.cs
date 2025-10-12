using System.IO;
using UnityEngine;

public class DeathAbility : BaseAbilityPlayer
{

    // it clasic metod===========
    //private string deathAnimParamiterName = "Death";
    //private int deathParamiterID;

    //protected override void Initialization()
    //{
    //    base.Initialization();
    //    deathParamiterID = Animator.StringToHash(deathAnimParamiterName);
    //}


    public override void EnterAbility()
    {
        //player.DeactivateCurrentWeapon();
        player.GetComponent<WeaponManager>().DeactivateAllWeapon();

        SpawnMode.spawnFromCheckPoint = true;

        player.gatherInput.DisablePlayerMap();

        linkedPhysics.ResetVelocity();

        if (linkedPhysics.grounded)
        {
            linkedAnimator.SetBool("Death", true);
        }
        else
        {
            // air death animation
            linkedAnimator.SetBool("Death", true);
        }

    }

    // or use Animator apdate
    //public override void UpdateAnimator()
    //{
    //    linkedAnimator.SetBool(deathParamiterID, linkedStateMachine.curentState == PlayerStates.State.Idle);
    //    // or 
    //    //linkedAnimator.SetBool("Death", linkedStateMachine.curentState == PlayerStates.State.Idle);
    //}

    public void ResetGame()
    {
        //Debug.Log("Game reset");
        string loadPath = Path.Combine(Application.persistentDataPath,
            ServiceLocator.Current.Get<SaveLoadManager>().folderName,
            ServiceLocator.Current.Get<SaveLoadManager>().fileNameCheckPoint);

        if (File.Exists(loadPath))
        {
            // load data
            CheckPointData checkPointData = new CheckPointData();
            ServiceLocator.Current.Get<SaveLoadManager>().LoadData(checkPointData,
                ServiceLocator.Current.Get<SaveLoadManager>().folderName,
                ServiceLocator.Current.Get<SaveLoadManager>().fileNameCheckPoint);
            ServiceLocator.Current.Get<LevelManager>().LoadLevelString(checkPointData.sceneToLoad);
        }
        else
        {
            ServiceLocator.Current.Get<LevelManager>().RestartLevel();
        }
    }
}
