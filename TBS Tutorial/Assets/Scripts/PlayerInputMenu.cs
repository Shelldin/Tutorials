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
        meleeMenu,
        shootMenu;

    public TMP_Text turnPointText,
        errorText;
    
    public float actionWaitTime = 1f,
        errorDisplayTime = 3f;

    private float errorCounter;

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

    private void Update()
    {
        if (errorCounter>0)
        {
            errorCounter -= Time.deltaTime;

            if (errorCounter<=0)
            {
                errorText.gameObject.SetActive(false);
            }
        }
    }


    public void HideMenus()
    {
        inputMenu.SetActive(false);
        moveMenu.SetActive(false);
        meleeMenu.SetActive(false);
        shootMenu.SetActive(false);
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
            ShowErrorText("No targets in melee range.");
        }
    }

    public void HitButton()
    {
        GameManager.instance.activePlayer.MeleeAttack();
        GameManager.instance.currentActionCost = 1;
        
        HideMenus();
        
        GameManager.instance.targetIndicatorObj.SetActive(false);
        
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

    public void ShowErrorText(string errorMessageString)
    {
        errorText.text = errorMessageString;
        errorText.gameObject.SetActive(true);

        errorCounter = errorDisplayTime;
    }

    public void ShowShootMenu()
    {
        HideMenus();
        shootMenu.SetActive(true);
    }

    public void HideShootMenu()
    {
        HideMenus();
        ShowInputMenu();
        
        GameManager.instance.targetIndicatorObj.SetActive(false);
    }

    public void CheckShoot()
    {
        GameManager.instance.activePlayer.GetShootTargets();

        if (GameManager.instance.activePlayer.shootTargets.Count > 0)
        {
            ShowShootMenu();
            
            GameManager.instance.targetIndicatorObj.SetActive(true);
            GameManager.instance.targetIndicatorObj.transform.position =
                GameManager.instance.activePlayer.shootTargets[GameManager.instance.activePlayer.currentShootTarget].transform.position;
        }
        else
        {
            ShowErrorText("No Enemies in Range");
        }
    }

    public void NextShootTarget()
    {
        GameManager.instance.activePlayer.currentShootTarget++;
        if (GameManager.instance.activePlayer.currentShootTarget >= GameManager.instance.activePlayer.shootTargets.Count)
        {
            GameManager.instance.activePlayer.currentShootTarget = 0;
        }
        GameManager.instance.targetIndicatorObj.transform.position =
            GameManager.instance.activePlayer.shootTargets[GameManager.instance.activePlayer.currentShootTarget].transform.position;
    }

    public void FireShotButton()
    {
        GameManager.instance.activePlayer.FireShot();

        GameManager.instance.currentActionCost = 1;
        HideMenus();
        StartCoroutine(WaitToEndActionCoroutine(1f));
    }
}
