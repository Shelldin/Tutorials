using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class CharacterController : MonoBehaviour
{
    public float moveSpeed;
    private Vector3 moveTarget;

    public NavMeshAgent navAgent;
    private bool isMoving;

    public bool isEnemy;

    public float moveRange = 3.5f,
        runRange = 8f;

    public float meleeRange = 1.5f;
    [HideInInspector]
    public List<CharacterController> meleeTargets = new List<CharacterController>();
    [HideInInspector]
    public int currentMeleeTarget;

    public float maxHealth = 10f;
    [HideInInspector]
    public float currentHealth;

    public TMP_Text healthText;
    public Slider healthSlider;

    public float meleeDamage = 5f;

    public float shootRange,
        shootDamage;

    [HideInInspector]
    public List<CharacterController> shootTargets = new List<CharacterController>();
    [HideInInspector]
    public int currentShootTarget;

    public Transform shootPoint;


    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        moveTarget = transform.position;

        navAgent.speed = moveSpeed;

        currentHealth = maxHealth;
        
        UpdateHealthUI();
    }

    void Update()
    {
        //move towards a target point
        if (isMoving)
        {
            
            //transform.position = Vector3.MoveTowards(transform.position, moveTarget, moveSpeed * Time.deltaTime);
            
            //check if current unit/player is the active unit
            if (GameManager.instance.activePlayer == this)
            {
                //if current player is the active player move Camera to view that player
                CameraController.instance.SetMoveTarget(transform.position);
                
                //check to prevent minor misalignment error when switching players
                if (Vector3.Distance(transform.position, moveTarget) < .2f)
                {
                    isMoving = false;
                    
                    GameManager.instance.FinishedMovement();
                }
            }
        }
    }

    public void MoveToPoint(Vector3 movePoint)
    {
        moveTarget = movePoint;

        navAgent.SetDestination(moveTarget);
        isMoving = true;
    }

    public void GetMeleeTargets()
    {
        meleeTargets.Clear();

        if (!isEnemy)
        {
            foreach (CharacterController charCon in GameManager.instance.enemyTeam)
            {
                if (Vector3.Distance(transform.position, charCon.transform.position) < meleeRange)
                {
                    meleeTargets.Add(charCon);
                }
            }
        }
        else
        {
            foreach (CharacterController charCon in GameManager.instance.playerTeam)
            {
                if (Vector3.Distance(transform.position, charCon.transform.position) < meleeRange)
                {
                    meleeTargets.Add(charCon);
                }
            }
        }

        if (currentMeleeTarget >= meleeTargets.Count)
        {
            currentMeleeTarget = 0;
        }
    }

    public void MeleeAttack()
    {
        meleeTargets[currentMeleeTarget].TakeDamage(meleeDamage);
        
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            navAgent.enabled = false;
            transform.rotation = Quaternion.Euler(-70f, transform.rotation.eulerAngles.y, 0);

            GameManager.instance.allCharacters.Remove(this);
            if (GameManager.instance.playerTeam.Contains(this))
            {
                GameManager.instance.playerTeam.Remove(this);
            }

            if (GameManager.instance.enemyTeam.Contains(this))
            {
                GameManager.instance.enemyTeam.Remove(this);
            }
        }
        
        UpdateHealthUI();
    }

    public void UpdateHealthUI()
    {
        healthText.text = "HP: " + currentHealth + "/" + maxHealth;

        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    public void GetShootTargets()
    {
        shootTargets.Clear();

        if (!isEnemy)
        {
            foreach (CharacterController charCon in GameManager.instance.enemyTeam)
            {
                if (Vector3.Distance(transform.position, charCon.transform.position) < shootRange)
                {
                    shootTargets.Add(charCon);
                }
            }
        }
        else
        {
            foreach (CharacterController charCon in GameManager.instance.playerTeam)
            {
                if (Vector3.Distance(transform.position, charCon.transform.position) < shootRange)
                {
                    shootTargets.Add(charCon);
                }
            }
        }

        if (currentShootTarget >= shootTargets.Count)
        {
            currentShootTarget = 0;
        }
    }

    public void FireShot()
    {
        Vector3 targetPoint = shootTargets[currentShootTarget].transform.position;

        Vector3 shootDirection = targetPoint - shootPoint.position;
    }
}
