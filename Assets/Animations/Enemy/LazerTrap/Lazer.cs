using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazer : MonoBehaviour
{

    private float timeTilOn;
    public float startTimeOn;

    private float timeTilOff;
    public float startTimeOff;

    Animator anim;
    public bool on;
    public bool off;
    
    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        timeTilOn = startTimeOn;
        timeTilOff = startTimeOff;
    }

    private void OnEnable()
    {
 
        timeTilOn = startTimeOn;
        timeTilOff = startTimeOff;
    }
    private void FixedUpdate()
    {
        if (on)
        {
            anim.SetBool("On", true);
            anim.SetBool("Off", false);
        }

        if (off)
        {
            anim.SetBool("On", false);
            anim.SetBool("Off", true);
        }
    }
    // Update is called once per frame
    void Update()
    {
  
        if (on) timeTilOn -= Time.deltaTime;
        if (off) timeTilOff -= Time.deltaTime;

        if (timeTilOn <= 0)
        {
            on = false;
            off = true;

            timeTilOn = startTimeOn;
        }

        if (timeTilOff <= 0)
        {
            on = true;
            off = false;
            timeTilOff = startTimeOff;
        }

    }
}
