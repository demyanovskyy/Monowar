using Assets.Scripts.Core.ObjectPooling;
using System;
using System.Collections;
using UnityEngine;


public class Shell : MonoBehaviour
{
    private float vericalForce;
    private float horizontalForce;

    [SerializeField] private float minEjectForce;
    [SerializeField] private float maxEjectForce;
    [SerializeField] private float minHorizontalForce;
    [SerializeField] private float maxHorizontalForce;
    [SerializeField] Rigidbody2D rb;
   // [SerializeField] private float lifespan;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        vericalForce = UnityEngine.Random.Range(minEjectForce, maxEjectForce);
        horizontalForce = UnityEngine.Random.Range(minHorizontalForce, maxHorizontalForce);
        Vector2 force = (Vector2)transform.up * vericalForce + (Vector2)transform.right * (-1) * horizontalForce;
        rb.AddForce(force, ForceMode2D.Impulse);
        //Destroy(gameObject, lifespan);
    }
    public void SetParamiter(Vector3 bTransform, Quaternion bRotate)
    {
        rb.transform.position = bTransform;
        rb.transform.rotation = bRotate;

        vericalForce = UnityEngine.Random.Range(minEjectForce, maxEjectForce);
        horizontalForce = UnityEngine.Random.Range(minHorizontalForce, maxHorizontalForce);
        Vector2 force = (Vector2)transform.up * vericalForce + (Vector2)transform.right * (-1) * horizontalForce;
        rb.AddForce(force, ForceMode2D.Impulse);
        rb.AddTorque(UnityEngine.Random.Range(minEjectForce, maxEjectForce));
    }
 
}
