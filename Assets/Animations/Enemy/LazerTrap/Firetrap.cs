using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firetrap : MonoBehaviour
{

    private float timeTilOn;
    public float startTimeOn;

    private float timeTilOff;
    public float startTimeOff;

    public GameObject particle;
    public BoxCollider2D colider;
    public bool on = true;
    public bool off = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (timeTilOn <= 0)
        {
            on = false;
            off = true;

            timeTilOn = startTimeOn;
        }
        else
        {

            if (on) timeTilOn -= Time.deltaTime;
        }

        if (timeTilOff <= 0)
        {
            on = true;
            off = false;
            timeTilOff = startTimeOff;
        }
        else
        {

            if (off) timeTilOff -= Time.deltaTime;
        }



        if (on)
        {
            particle.SetActive(true);
            colider.SetActive(true);
            
        }

        if (off)
        {
            particle.SetActive(false);
            colider.SetActive(false);

        }
    }
}
