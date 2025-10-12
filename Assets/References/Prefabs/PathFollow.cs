using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollow : MonoBehaviour
{
  
    
    [Header("Settings")] 
 
    [SerializeField] private bool _enemy = true;
    [SerializeField] private float _otheObjectsSpeed = 0.1f;

    [SerializeField] private float minDistanceToPoint = 0.1f;

   // public float MoveSpeed => moveSpeed;
    private float moveSpeed;

    public bool _directionRight;    
    public List<Vector3> points = new List<Vector3>();

    private bool _playing;
    private bool _moved;
    private int _currentPoint = 0;
    private Vector3 _currentPosition;
    private Vector3 _previousPosition;

    public float waitTime =0.1f;
    float nextMoveTime;

    public bool _wait;

    public bool _moveStart = true;

    private void Start()
    {
        _playing = true;

        _previousPosition = transform.position;
        _currentPosition = transform.position;
        transform.position = _currentPosition + points[0];
        if (_enemy)
        {
           // moveSpeed = GetComponent<Enemy>().speed;
        }
        else
        {
            moveSpeed = _otheObjectsSpeed;
        }
    }

    private void FixedUpdate()
    {
        if (_moveStart)
        {
            Move();
        }

    }

    private void Move()
    {
        if (Time.time < nextMoveTime)
        {
            _wait = true;
            return;
        }
        else
        {
            _wait = false;
        }
            // Set first position
            if (!_moved)
        {
            transform.position = _currentPosition + points[0];
            _currentPoint++;
            _moved = true;
            nextMoveTime = Time.time + waitTime;

        } 
        
        // Move to next point
        transform.position = Vector3.MoveTowards(transform.position, _currentPosition + points[_currentPoint], Time.deltaTime * moveSpeed);
        
        // Evaluate move to next point
        float distanceToNextPoint = Vector3.Distance(_currentPosition + points[_currentPoint], transform.position);
        if (distanceToNextPoint < minDistanceToPoint)
        {
            _previousPosition = transform.position;
            _currentPoint++;
            nextMoveTime = Time.time + waitTime;
           
        }
        
        // Define move direction
        if (_previousPosition != Vector3.zero)
        {
            if (transform.position.x > _previousPosition.x)
            {
                _directionRight = true;
            }
            else if (transform.position.x < _previousPosition.x)
            {
                _directionRight = false;
            }
        }

        // If we are on the last point, reset our position to the first one
        if (_currentPoint == points.Count)
        {
            _currentPoint = 0;
        }
    }
    
    private void OnDrawGizmos()
    {
        if (transform.hasChanged && !_playing)
        {
            _currentPosition = transform.position;
        }
        
        if (points != null)
        {
            for (int i = 0; i < points.Count; i++)
            {
                if (i < points.Count)
                {
                    // Draw points
                    Gizmos.color = Color.red;
                    Gizmos.DrawWireSphere(_currentPosition + points[i], 0.4f);
                    
                    // Draw lines
                    Gizmos.color = Color.black;
                    if (i < points.Count - 1)
                    {
                        Gizmos.DrawLine(_currentPosition + points[i], _currentPosition +  points[i + 1]);
                    }
                    
                    // Draw line from last point to first point
                    if (i == points.Count - 1)
                    {
                        Gizmos.DrawLine(_currentPosition + points[i], _currentPosition + points[0]);
                    }
                }
            }
        }
    }
}
