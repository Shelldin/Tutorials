using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputMenu : MonoBehaviour
{
    public static PlayerInputMenu instance;

    public GameObject inputMenu, moveMenu;

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
    

    public void HideMenus()
    {
        inputMenu.SetActive(false);
        moveMenu.SetActive(false);
    }

    public void ShowInputMenu()
    {
        inputMenu.SetActive(true);
    }

    public void ShowMoveMenu()
    {
        HideMenus();
        moveMenu.SetActive(true);
        
        ShowMove();
    }

    public void ShowMove()
    {
        if (GameManager.instance.turnPointsRemaining >= 1)
        {
            MoveGrid.instance.ShowPointsInRange
                (GameManager.instance.activePlayer.moveRange, GameManager.instance.activePlayer.transform.position);
            
        }
    }

    public void ShowRun()
    {
        if (GameManager.instance.turnPointsRemaining >= 1)
        {
            MoveGrid.instance.ShowPointsInRange
                (GameManager.instance.activePlayer.runRange, GameManager.instance.activePlayer.transform.position);
            
        }
    }
}
