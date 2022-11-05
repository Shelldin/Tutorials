using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public CharacterController activePlayer;

    public List<CharacterController> allCharacters = new List<CharacterController>();
    public List<CharacterController> playerTeam = new List<CharacterController>();
    public List<CharacterController> enemyTeam = new List<CharacterController>();

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
       allCharacters.AddRange(FindObjectsOfType<CharacterController>());

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
       
       allCharacters.Clear();
       
       allCharacters.AddRange(playerTeam);
       allCharacters.AddRange(enemyTeam);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
