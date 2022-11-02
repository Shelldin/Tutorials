using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    public float moveSpeed,
        manualMoveSpeed = 5f;
    private Vector3 moveTarget;
    
    private Vector2 moveInput;

    private float targetRotation;
    public float rotateSpeed = 5f;

    private int currentAngle;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        moveTarget = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //move the camera towards a target point
        if (moveTarget != transform.position)
        {
            transform.position = 
                Vector3.MoveTowards(transform.position, moveTarget, moveSpeed * Time.deltaTime);
        }

        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");
        //normalize prevents moving faster when moving diagonally
        moveInput.Normalize();

        //manually move camera
        if (moveInput != Vector2.zero)
        {
            //transform.forward & transform.right make sure we're moving relative to the way the camera is currently facing
            transform.position += ((transform.forward * (moveInput.y * manualMoveSpeed))
                                  + (transform.right * (moveInput.x * manualMoveSpeed))) * Time.deltaTime;
            
            //prevent camera from snapping back to current active player while trying to move manually
            moveTarget = transform.position;
        }
        
        //snap camera back to current active player
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SetMoveTarget(GameManager.instance.activePlayer.transform.position);
        }

        //rotate camera clockwise
        if (Input.GetKeyDown(KeyCode.Q))
        {
            currentAngle++;

            //reset to initial angle if we rotate a full circle
            if (currentAngle >= 4)
            {
                currentAngle = 0;
            }
        }

        //rotate camera counter clockwise
        if (Input.GetKeyDown(KeyCode.E))
        {
            currentAngle--;

            //set current angle to max when rotating a full cricle
            if (currentAngle < 0)
            {
                currentAngle = 3;
            }
        }

        //set rotation target to to a multiple of 90 based on currentAngle (with a 45 degree offset)
        targetRotation = (90f * currentAngle) + 45f;
        
        //rotate the camera
        transform.rotation = Quaternion.Slerp(transform.rotation, 
            Quaternion.Euler(0f, targetRotation, 0f), rotateSpeed * Time.deltaTime );
    }

    public void SetMoveTarget(Vector3 newTarget)
    {
        moveTarget = newTarget;
    }
}
