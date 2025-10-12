using System;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointControll : MonoBehaviour
{
    CheckPoint[] checkPointArray;
    
    private void Awake()
    {
        checkPointArray = FindObjectsOfType<CheckPoint>();
    }

    public void ChecPointDeActivated()
    {
        foreach (CheckPoint chekPoint in checkPointArray)
        {
            chekPoint.DeActivatedChecpoint();
        }
    }
}
