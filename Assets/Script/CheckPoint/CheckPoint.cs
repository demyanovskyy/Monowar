using System;
using System.IO;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRender;
    [SerializeField] private Sprite spriteDisabled;
    [SerializeField] private Sprite spriteEnabled;
    [SerializeField] private Collider2D collider;
    [SerializeField] private CheckPointData checkPointData;
    public bool checkPointActiv = false;

    

    private void Start()
    {
        string loadPath = Path.Combine(Application.persistentDataPath,
            ServiceLocator.Current.Get<SaveLoadManager>().folderName, 
            ServiceLocator.Current.Get<SaveLoadManager>().fileNameCheckPoint);

        if(File.Exists(loadPath))
        {
            CheckPointData tempCheckPointData = new CheckPointData();
            ServiceLocator.Current.Get<SaveLoadManager>().LoadData(tempCheckPointData,
                ServiceLocator.Current.Get<SaveLoadManager>().folderName,
                ServiceLocator.Current.Get<SaveLoadManager>().fileNameCheckPoint);
       
            if(tempCheckPointData.checkPointKey == checkPointData.checkPointKey)
            {
                spriteRender.sprite = spriteEnabled;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<ActivateCheckPoint>().checkPoint = this;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<ActivateCheckPoint>().checkPoint = null;
        }
    }

    public void ActivateCheckPoint()
    {
        spriteRender.sprite = spriteEnabled;
        checkPointActiv = true;
        // save data
        ServiceLocator.Current.Get<SaveLoadManager>().SaveData(checkPointData,
            ServiceLocator.Current.Get<SaveLoadManager>().folderName,
            ServiceLocator.Current.Get<SaveLoadManager>().fileNameCheckPoint);
    }

    public void DeActivatedChecpoint()
    {
        spriteRender.sprite = spriteDisabled;
        checkPointActiv = false;
    }

}
