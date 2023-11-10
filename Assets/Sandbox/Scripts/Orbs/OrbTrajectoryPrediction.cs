using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbTrajectoryPrediction : MonoBehaviour
{
    [Header("Renderer")]
    [SerializeField] LineRenderer _line;
    [SerializeField] GameObject _firePoint;
    [SerializeField] LayerMask _collisionMask;
    [Header("DisplayControls")]
    [SerializeField] 
    [Range(1, 30)] int _length = 10;
    [SerializeField] 
    [Range(.01f, .25f)]float _timeBetweenPoints = .1f;
    
    float _gravity;
    Vector3 _initialVel;
    int totalPointsCount;

    public void DrawPrediction(float _gravity, Vector3 _initialVel)
    {
        this._gravity = _gravity;
        this._initialVel = _initialVel;

        totalPointsCount = Mathf.CeilToInt(_length / _timeBetweenPoints) + 1;
        
        _line.enabled = true;
        _line.positionCount = totalPointsCount;
        _line.SetPositions(CalculateLineArray());
    }

    public bool HidePrediction()
    {
        bool wasEnabled = _line.enabled;
        _line.enabled = false;
        return wasEnabled;
    }

    private Vector3[] CalculateLineArray()
    {
        Vector3[] lineArray = new Vector3[totalPointsCount];
        float t = 0;

        
        for (int i = 0; i < lineArray.Length; i++, t += _timeBetweenPoints)
        {
            lineArray[i] = CalculateLinePoint(t);

            //Skip first iteration, as we still don't have 2 points to compare
            if(i > 0)
            {
                Vector3 currentPoint = lineArray[i];
                Vector3 lastPoint = lineArray[i - 1];

                // If there is a collision between current point and last point, set the end of the line at the intersection
                if(Physics.Linecast(lastPoint, currentPoint, out RaycastHit hitInfo, _collisionMask, QueryTriggerInteraction.Ignore))
                {
                    lineArray[i] = hitInfo.point;
                    _line.positionCount = i + 1;
                    break;
                }
            }
            

             

        }

        return lineArray;
    }

    private Vector3 CalculateLinePoint(float t)
    {
        float x = _firePoint.transform.position.x + (_initialVel.x * t);
        float z = _firePoint.transform.position.z + (_initialVel.z * t);
        float y = _firePoint.transform.position.y + (_initialVel.y * t) + (_gravity / 2f * t * t);

        return new(x, y, z);
    }
}
