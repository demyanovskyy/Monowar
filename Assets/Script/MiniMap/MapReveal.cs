using UnityEngine;

public class MapReveal : MonoBehaviour
{
    [SerializeField] private string mapKey;
    private MiniMapDisplayControl mapDisplayControl;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mapDisplayControl = FindAnyObjectByType<MiniMapDisplayControl>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            mapDisplayControl.LoadMinimapData();
            mapDisplayControl.minimapData.AddToListWithCheck(mapKey);
            mapDisplayControl.DisplayUnlockMiniMAp();
        }
    }
}
