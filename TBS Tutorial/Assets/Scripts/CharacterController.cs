using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float moveSpeed;
    private Vector3 moveTarget;


    void Start()
    {
        moveTarget = transform.position;
    }

    void Update()
    {
        //move towards a target point
        if (transform.position != moveTarget)
        {
            
            transform.position = Vector3.MoveTowards(transform.position, moveTarget, moveSpeed * Time.deltaTime);
            
            //check if current unit/player is the active unit
            if (GameManager.instance.activePlayer == this)
            {
                //check to prevent minor misalignment error when switching players
                if (Vector3.Distance(transform.position, moveTarget) > .1f)
                {
                    //if current player is the active player move Camera to view that player
                    CameraController.instance.SetMoveTarget(transform.position);
                }
            }
        }
    }

    public void MoveToPoint(Vector3 movePoint)
    {
        moveTarget = movePoint;
    }
}
