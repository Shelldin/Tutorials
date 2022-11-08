using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public CharacterController activePlayer;

    public List<CharacterController> allCharacters = new List<CharacterController>();
    public List<CharacterController> playerTeam = new List<CharacterController>();
    public List<CharacterController> enemyTeam = new List<CharacterController>();

    private int currentChar;

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
        //assign the characters to the list in a random order to create more variance in the turn order
        List<CharacterController> tempList = new List<CharacterController>();

       tempList.AddRange(FindObjectsOfType<CharacterController>());

       int iterations = tempList.Count + 50;
       while (tempList.Count > 0 && iterations > 0)
       {
           int randomPick = Random.Range(0, tempList.Count);
           
           allCharacters.Add(tempList[randomPick]);

           tempList.RemoveAt(randomPick);

           iterations--;
       }
        
       //sort the characters into their teams of player and enemy
       foreach (CharacterController charCon in allCharacters)
       {
           if (!charCon.isEnemy)
           {
               playerTeam.Add(charCon);
           }
           else
           {
               enemyTeam.Add(charCon);
           }
       }
       
       //clear allCharacters so we can put them into correct order
       allCharacters.Clear();
       
       //Randomly choose either the player team to go first or
       if (Random.value >= .5f)
       {
           allCharacters.AddRange(playerTeam);
           allCharacters.AddRange(enemyTeam);
       }
       //enemy team to go first
       else
       {
           allCharacters.AddRange(enemyTeam);
           allCharacters.AddRange(playerTeam);
       }

       //makes sure first character in list starts as active character
       activePlayer = allCharacters[0];
       
       //make sure camera starts focused on the active player
       CameraController.instance.SetMoveTarget(activePlayer.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //what happens when a character is finished moving
    public void FinishedMovement()
    {
        EndTurn();
    }

    //what happens when a character ends its turn
    public void EndTurn()
    {
        //choose the next character in the list to become the active character
        //or loop to first if not further characters remain
        currentChar++;

        if (currentChar >= allCharacters.Count)
        {
            currentChar = 0;
        }

        activePlayer = allCharacters[currentChar];
        
        //make sure camera starts focused on the active player
        CameraController.instance.SetMoveTarget(activePlayer.transform.position);
        
        
    }
}
