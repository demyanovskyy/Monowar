using System.Globalization;
using System.IO;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour, IService
{
 
    [Header("Save folder")]
    public string folderName = "SaveFile";

    [Header("Spawn point")]
    public string fileNameSpawnPoint = "SpawnPoint.json";

    [Header("Check point")]
    public string fileNameCheckPoint = "checkPointPoint.json";

    [Header("Minimap")]
    public string fileNameMinimap = "Minimap.json";

    [Header("Player Data")]
    public string fileNamePlayerData = "PlayerData.json";

    public void Init()
    {
        DontDestroyOnLoad(gameObject);
    }


    public void SaveData<T>(T dataToSave, string folderName, string fileName)
    {
        string savePath = Path.Combine(Application.persistentDataPath, folderName, fileName);
        Directory.CreateDirectory(Path.GetDirectoryName(savePath));
        File.WriteAllText(savePath, JsonUtility.ToJson(dataToSave, true));
    }
    public void LoadData<T>(T dataToLoad, string folderName, string fileName)
    {
        string loadPath = Path.Combine(Application.persistentDataPath, folderName, fileName);
        if (File.Exists(loadPath))
        {
            string loadDataString = File.ReadAllText(loadPath);
            JsonUtility.FromJsonOverwrite(loadDataString, dataToLoad);
        }
    }

    public void DeleteSaveFile(string folderName, string fileName)
    {
        string filePath = Path.Combine(Application.persistentDataPath, folderName, fileName);
        if (File.Exists(filePath))
            File.Delete(filePath);
    }


    public void DelitFolder(string folderName)
    {
        string folderPath = Path.Combine(Application.persistentDataPath, folderName);
        if(Directory.Exists(folderPath))
        {
            Directory.Delete(folderPath, true);
        }
    }

}
