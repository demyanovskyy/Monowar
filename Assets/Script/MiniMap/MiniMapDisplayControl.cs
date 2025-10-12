using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MiniMapDisplayControl : MonoBehaviour
{
    [SerializeField] private string firstMinimapToReveal;
    [SerializeField] private List<MiniMapID> minimapIDs = new List<MiniMapID>();
    public MinimapData minimapData = new MinimapData();
    private string loadPath;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadMinimapData();
        DisplayUnlockMiniMAp();
    }

    public void LoadMinimapData()
    {
        loadPath = Path.Combine(Application.persistentDataPath,
            ServiceLocator.Current.Get<SaveLoadManager>().folderName,
            ServiceLocator.Current.Get<SaveLoadManager>().fileNameMinimap);

        if (File.Exists(loadPath))
        {
            ServiceLocator.Current.Get<SaveLoadManager>().LoadData(minimapData,
                ServiceLocator.Current.Get<SaveLoadManager>().folderName,
                ServiceLocator.Current.Get<SaveLoadManager>().fileNameMinimap);
        }
    }

    public void DisplayUnlockMiniMAp()
    {
        if (minimapData.mapKeys.Count == 0)
        {
            // decide what to do if ther is no sav file
            foreach (MiniMapID mapID in minimapIDs)
            {
                if (mapID.mapKey == firstMinimapToReveal)
                {
                    mapID.gameObject.SetActive(true);
                    minimapData.AddToListWithCheck(firstMinimapToReveal);
                    break;
                }
            }
        }
        else
        {
            foreach (string key in minimapData.mapKeys)
            {
                foreach (MiniMapID mapID in minimapIDs)
                {
                    if (key == mapID.mapKey)
                    {
                        mapID.gameObject.SetActive(true);
                        break;
                    }
                }
            }
        }

    }

    private void OnDisable()
    {
        ServiceLocator.Current.Get<SaveLoadManager>().SaveData(minimapData,
            ServiceLocator.Current.Get<SaveLoadManager>().folderName,
            ServiceLocator.Current.Get<SaveLoadManager>().fileNameMinimap);
    }

}
