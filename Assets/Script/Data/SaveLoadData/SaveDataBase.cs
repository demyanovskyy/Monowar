
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnData
{
    public string spawnPointKey;
    public bool facingRight;

    public SpawnData()
    {
        spawnPointKey = "Start";
        facingRight = true;
    }
}

[System.Serializable]

public class CheckPointData
{
    public string sceneToLoad;
    public string checkPointKey;
    public bool facingRight;

    public CheckPointData()
    {
        sceneToLoad = "Level1";
        checkPointKey = "Check1";
        facingRight = true;

    }
}

[System.Serializable]

public class MinimapData
{
     public List<string> mapKeys = new List<string>();

    public void AddToListWithCheck(string keyToAdd)
    {
        if (mapKeys.Contains(keyToAdd))
            return;

        mapKeys.Add(keyToAdd);
    }
}

[System.Serializable]

public class WeaponData
{
    public string ID;
    public int currentAmmo;
    public int storageAmmo;
}

[System.Serializable]

public class PlayerParamiters
{
    public float currentHealth;
}