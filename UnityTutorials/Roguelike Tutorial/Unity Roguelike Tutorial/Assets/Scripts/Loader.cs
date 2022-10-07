using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// from Unity Rogue-like Tutorial

public class Loader : MonoBehaviour
{

    public GameObject gameManager;
    
    void Awake()
    {
        if (GameManager.instance == null)
        {
            Instantiate(gameManager);
        }
    }
}
