using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePoint : MonoBehaviour
{
    private void OnMouseDown()
    {
        //move current player to clicked on point
        
        GameManager.instance.activePlayer.MoveToPoint(transform.position);
    }
}
