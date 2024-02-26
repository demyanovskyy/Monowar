using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float sawSpeed = 300;
    private Vector3 rotatevector;

    private void Update()
    {
        rotatevector = new Vector3(0, 0, sawSpeed * Time.deltaTime);

        transform.Rotate(rotatevector);
    }

}
