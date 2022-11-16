using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePoint : MonoBehaviour
{
    private void OnMouseDown()
    {
        
        //mouse down won't work on bottom tenth of the screen to prevent accidentally moving when clicking on menu buttons
        if (Input.mousePosition.y > Screen.height * .1f)
        {
            //move current player to clicked on point
            GameManager.instance.activePlayer.MoveToPoint(transform.position);
        
            MoveGrid.instance.HideMovePoints();
        
            PlayerInputMenu.instance.HideMenus();
        }
        
    }
}
