using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class CharacterController : MonoBehaviour
{
    public float moveSpeed;
    private Vector3 moveTarget;

    public NavMeshAgent navAgent;
    private bool isMoving;

    public bool isEnemy;

    public float moveRange = 3.5f;


    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        moveTarget = transform.position;

        navAgent.speed = moveSpeed;
    }

    void Update()
    {
        //move towards a target point
        if (isMoving)
        {
            
            //transform.position = Vector3.MoveTowards(transform.position, moveTarget, moveSpeed * Time.deltaTime);
            
            //check if current unit/player is the active unit
            if (GameManager.instance.activePlayer == this)
            {
                //if current player is the active player move Camera to view that player
                CameraController.instance.SetMoveTarget(transform.position);
                
                //check to prevent minor misalignment error when switching players
                if (Vector3.Distance(transform.position, moveTarget) < .2f)
                {
                    isMoving = false;
                    
                    GameManager.instance.FinishedMovement();
                }
            }
        }
    }

    public void MoveToPoint(Vector3 movePoint)
    {
        moveTarget = movePoint;

        navAgent.SetDestination(moveTarget);
        isMoving = true;
    }
}
