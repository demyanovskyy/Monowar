using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    [SerializeField] private float xParallaxValue;
    [SerializeField] private float yParallaxValue;
    private float spriteLenght;
    private Camera cam;
    private Vector3  deltaMovment;
    private Vector3 lastCameraPosition;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
        lastCameraPosition = cam.transform.position;
        spriteLenght = GetComponent<SpriteRenderer>().bounds.size.x;

    }

    private void LateUpdate()
    {
        deltaMovment = cam.transform.position - lastCameraPosition;
        transform.position += new Vector3(deltaMovment.x * xParallaxValue, deltaMovment.y * yParallaxValue);
        lastCameraPosition = cam.transform.position;

        if(cam.transform.position.x - transform.position.x >= spriteLenght)
        {
            transform.position = new Vector3(cam.transform.position.x + spriteLenght, transform.position.y);
        }
        else
        if (transform.position.x - cam.transform.position.x  >= spriteLenght)
        {
            transform.position = new Vector3(cam.transform.position.x - spriteLenght, transform.position.y);
        }


    }
}
