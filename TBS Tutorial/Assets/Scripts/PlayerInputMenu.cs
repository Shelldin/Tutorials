using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInputMenu : MonoBehaviour
{
    public static PlayerInputMenu instance;

    public GameObject inputMenu, moveMenu;

    public TMP_Text turnPointText;

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

            GameManager.instance.currentActionCost = 1;

        }
    }

    public void ShowRun()
    {
        if (GameManager.instance.turnPointsRemaining >= 2)
        {
            MoveGrid.instance.ShowPointsInRange
                (GameManager.instance.activePlayer.runRange, GameManager.instance.activePlayer.transform.position);

            GameManager.instance.currentActionCost = 2;

        }
    }

    public void UpdateTurnPointText(int turnPoints)
    {
        turnPointText.text = "Turn Points Remaining: " + turnPoints;
    }
}
