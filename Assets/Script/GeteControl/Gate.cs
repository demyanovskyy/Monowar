using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] private string levelToLoad;

    public SpawnData spawnDataForOtheLevel;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            ServiceLocator.Current.Get<SaveLoadManager>().SaveData(spawnDataForOtheLevel,
                ServiceLocator.Current.Get<SaveLoadManager>().folderName,
                ServiceLocator.Current.Get<SaveLoadManager>().fileNameSpawnPoint);

            Player player = collision.GetComponent<Player>();// get player
            player.gatherInput.DisablePlayerMap(); ;// stop input dot work
            player.physicsControl.ResetVelocity();// stop liner velocity
            player.GetComponentInChildren<PlayerStats>().SavePlayerHealth(); 

            ServiceLocator.Current.Get<LevelManager>().LoadLevelString(levelToLoad);

            GetComponent<Collider2D>().enabled = false;// gate collider2d disable
        }
    }
}
