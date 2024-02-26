using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoteGun : MonoBehaviour
{
    public Transform _parent;
    public float offsetMultiplierX = 1f;
    public float offsetMultiplierY = 1f;
    public float smoothTime = .3f;
    public Vector2 _delta;

    private Vector2 startPosition;
    private Vector3 velocity;

    private void Start()
    {
        startPosition = transform.position;
        if (_parent.localScale.x < 0)
            startPosition = new Vector2(startPosition.x - _delta.x, startPosition.y + _delta.y);
    }

    private void Update()
    {
        Vector2 offset = Camera.main.ScreenToViewportPoint(Input.mousePosition);

        if(_parent.localScale.x <0)
            transform.position = Vector3.SmoothDamp(transform.position, new Vector2(startPosition.x - _delta.x + offset.x * offsetMultiplierX, startPosition.y+_delta.y+ offset.y * offsetMultiplierY) , ref velocity, smoothTime);
        else
            transform.position = Vector3.SmoothDamp(transform.position, new Vector2(startPosition.x + offset.x * offsetMultiplierX, startPosition.y + _delta.y + offset.y * offsetMultiplierY), ref velocity, smoothTime);
    }
}