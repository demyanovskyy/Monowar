using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnController : MonoBehaviour
{
    private Transform player;

    private List<SpawnIdentifier> spawnGatePoint = new List<SpawnIdentifier>();
    private List<SpawnIdentifier> spawnCheckPoint = new List<SpawnIdentifier>();
    private SpawnIdentifier startPoint;

    private SpawnData spawnData = new SpawnData();
    private CheckPointData checkPointData = new CheckPointData();
    private bool canLoadFromCheckPoint = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        SpawnIdentifier[] spawnID = FindObjectsOfType<SpawnIdentifier>();
        foreach(SpawnIdentifier sID in spawnID)
        {
            if(sID.type == SpawnTypes.SpawnType.Gate)
            {
                spawnGatePoint.Add(sID);
            }
            else
            if (sID.type == SpawnTypes.SpawnType.CheckPoint)
            {
                spawnCheckPoint.Add(sID);
            }

            if(sID.type == SpawnTypes.SpawnType.Start)
                startPoint = sID;

        }

    }
    void Start()
    {
        // only for start game
        //ServiceLocator.Current.Get<SaveLoadManager>().DelitFolder(ServiceLocator.Current.Get<SaveLoadManager>().folderName);

        player = FindAnyObjectByType<Player>().transform;

        string loadPath = Path.Combine(Application.persistentDataPath, 
            ServiceLocator.Current.Get<SaveLoadManager>().folderName,
            ServiceLocator.Current.Get<SaveLoadManager>().fileNameCheckPoint);

        if(File.Exists(loadPath))
        {
            // load data
            ServiceLocator.Current.Get<SaveLoadManager>().LoadData(checkPointData,
                ServiceLocator.Current.Get<SaveLoadManager>().folderName,
                ServiceLocator.Current.Get<SaveLoadManager>().fileNameCheckPoint);
        
            if(checkPointData.sceneToLoad == SceneManager.GetActiveScene().name)
            {
                canLoadFromCheckPoint = true;
            }
        }

        //only for start game
        //if (startPoint != null)
        //    {
        //        player.transform.position = startPoint.transform.position;
        //        return;
        //    }

        if (SpawnMode.spawnFromCheckPoint == true && canLoadFromCheckPoint == true)
        {

            foreach (SpawnIdentifier spawnChekPointID in spawnCheckPoint)
            {
                if (spawnChekPointID.spawnKey == checkPointData.checkPointKey)
                {
                    player.transform.position = spawnChekPointID.transform.position;
                    break;
                }
            }

            if (checkPointData.facingRight == false)
            {
                player.GetComponent<Player>().ForceFlip();
            }
            SpawnMode.spawnFromCheckPoint = false;
        }
        else
        {
            ServiceLocator.Current.Get<SaveLoadManager>().LoadData(spawnData,
                ServiceLocator.Current.Get<SaveLoadManager>().folderName,
                ServiceLocator.Current.Get<SaveLoadManager>().fileNameSpawnPoint);

            foreach (SpawnIdentifier spawnID in spawnGatePoint)
            {
                if (spawnID.spawnKey == spawnData.spawnPointKey)
                {
                    player.transform.position = spawnID.transform.position;
                    break;
                }
            }

            if (spawnData.facingRight == false)
            {
                player.GetComponent<Player>().ForceFlip();
            }
        }

    }
}
