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
        //move towards a point
        transform.position = Vector3.MoveTowards(transform.position, moveTarget, moveSpeed * Time.deltaTime);
    }

    public void MoveToPoint(Vector3 movePoint)
    {
        moveTarget = movePoint;
    }
}
