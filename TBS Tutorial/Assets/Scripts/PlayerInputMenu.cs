using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInputMenu : MonoBehaviour
{
    public static PlayerInputMenu instance;

    public GameObject inputMenu,
        moveMenu,
        meleeMenu;

    public TMP_Text turnPointText;
    
    public float actionWaitTime = 1f;

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
        meleeMenu.SetActive(false);
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

    public void HideMoveMenu()
    {
        HideMenus();
        MoveGrid.instance.HideMovePoints();
        ShowInputMenu();
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

    public void SkipTurn()
    {
        GameManager.instance.EndTurn();
    }

    public void ShowMeleeMenu()
    {
        HideMenus();
        meleeMenu.SetActive(true);
    }

    public void HideMeleeMenu()
    {
        HideMenus();
        ShowInputMenu();
        
        GameManager.instance.targetIndicatorObj.SetActive(false);
    }

    public void CheckMelee()
    {
        GameManager.instance.activePlayer.GetMeleeTargets();

        if (GameManager.instance.activePlayer.meleeTargets.Count > 0)
        {
            ShowMeleeMenu();
            
            GameManager.instance.targetIndicatorObj.SetActive(true);
            GameManager.instance.targetIndicatorObj.transform.position =
                GameManager.instance.activePlayer.meleeTargets[GameManager.instance.activePlayer.currentMeleeTarget].transform.position;
        }
        else
        {
            
        }
    }

    public void HitButton()
    {
        GameManager.instance.activePlayer.MeleeAttack();
        GameManager.instance.currentActionCost = 1;
        
        HideMenus();
        StartCoroutine(WaitToEndActionCoroutine(actionWaitTime));
    }

    public IEnumerator WaitToEndActionCoroutine(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        
        GameManager.instance.SpendTurnPoints();
    }

    public void NextMeleeTargetButton()
    {
        GameManager.instance.activePlayer.currentMeleeTarget++;
        if (GameManager.instance.activePlayer.currentMeleeTarget >= GameManager.instance.activePlayer.meleeTargets.Count)
        {
            GameManager.instance.activePlayer.currentMeleeTarget = 0;
        }
        GameManager.instance.targetIndicatorObj.transform.position =
            GameManager.instance.activePlayer.meleeTargets[GameManager.instance.activePlayer.currentMeleeTarget].transform.position;
    }
}
