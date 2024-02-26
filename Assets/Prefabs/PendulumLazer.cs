using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PendulumLaser : MonoBehaviour
{
    float timer = 0f;
    public float speed = 1f;
    int phase = 0;
    public Laser _obj;

    public bool Left_or_right = true;

    private void Start()
    {
        _obj = GetComponentInChildren<Laser>();
    }
    void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        if (timer > 1f)
        {
            phase++;
            phase %= 4;            //Keep the phase between 0 to 3.
            timer = 0f;
        }
        if (Left_or_right)
        {
            switch (phase)
            {
                case 0:
                    _obj.transform.Rotate(0f, 0f, speed * (1 - timer));  //Speed, from maximum to zero.
                    _obj.SetActive(false);
                    break;
                case 1:
                    _obj.transform.Rotate(0f, 0f, -speed * timer);       //Speed, from zero to maximum.
                    _obj.SetActive(false);
                    break;
                case 2:
                    _obj.transform.Rotate(0f, 0f, -speed * (1 - timer)); //Speed, from maximum to zero.
                    _obj.SetActive(true);
                    break;
                case 3:
                    _obj.transform.Rotate(0f, 0f, speed * timer);        //Speed, from zero to maximum.
                    _obj.SetActive(true);
                    break;
            }
        }
        else
        {
            switch (phase)
            {
                case 0: 
                    _obj.transform.Rotate(0f, 0f, speed * timer);        //Speed, from zero to maximum.
                   
                    _obj.SetActive(true);
                    break;
                case 1:
                   _obj.transform.Rotate(0f, 0f, -speed * (1 - timer)); //Speed, from maximum to zero.
                    _obj.SetActive(true);
                    break;
                case 2:
                     _obj.transform.Rotate(0f, 0f, -speed * timer);       //Speed, from zero to maximum.
                    _obj.SetActive(false);
                    break;
                case 3:
                    _obj.transform.Rotate(0f, 0f, speed * (1 - timer));  //Speed, from maximum to zero.
                    _obj.SetActive(false);
                    break;
            }
        }
 
    }
}
