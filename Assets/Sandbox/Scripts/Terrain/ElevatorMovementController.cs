using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorMovementController : MonoBehaviour
{
    [SerializeField] float _elevatoMoveSpeed;
    [SerializeField] GameObject _topPosition;
    [SerializeField] GameObject _midPosition;
    [SerializeField] GameObject _bottomPosition;
    
    GameObject _actualPosition;

    GameObject _destinyPosition;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        _actualPosition = _midPosition;
        _destinyPosition = _actualPosition;  
    }

    private void FixedUpdate()
    {
        this.moveToPosition();
    }


    public void WeigthOrbImpact(bool gravityAugment){
        if(gravityAugment){
            if(_actualPosition.Equals(_topPosition)){
                _destinyPosition = _midPosition;
            }else{
                _destinyPosition = _bottomPosition;
            }
        } else {
            if(_actualPosition.Equals(_bottomPosition)){
                _destinyPosition = _midPosition;
            }else{
                _destinyPosition = _topPosition;             
            }
        }
    }

    void moveToPosition(){
        transform.position = Vector3.MoveTowards(transform.position, _destinyPosition.transform.position, _elevatoMoveSpeed * Time.deltaTime);
        _actualPosition = this._destinyPosition;
    }
    
}
