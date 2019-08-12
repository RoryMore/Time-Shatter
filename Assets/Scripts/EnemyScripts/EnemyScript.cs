﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//The base enemy script from which all enemies derrive 

public class EnemyScript : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;

    //Value required before they take their action
    public float enemyCooldown;
    //Value of their initiative speed
    public float initiativeSpeed;
    
    public bool enemyTakingAction;

    public float turnsHeld;

    public float timeSpentDoingAction = 0.0f;

    //public AudioClip deathClip;


    public Animator anim;
    //AudioSource enemyAudio;
    public ParticleSystem hitParticles;
    public CapsuleCollider capsuleCollider;

    public NavMeshAgent nav;

    public PlayerScript player;
    public EnemyManager enemyManager;
    public turnManageScript turnManger;

    public bool isDead;
    public bool isSinking;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Every time the enemy is unable to act on their turn, simply reset their initiative with a slight stacking advantage
    public void HoldTurn()
    {
        turnsHeld += 1;
        enemyCooldown = 6.0f - (0.5f * turnsHeld);
    }


    //Function that is called when the player deals damage to you
    //Default condition format
    public void TakeDamage(int amount, Vector3 hitPoint)
    {
        

        if (isDead)
            return;

        //Audio Cue
        //enemyAudio.Play();

        currentHealth -= amount;
        enemyManager.healList.Add(this.gameObject);

        //hitParticles.transform.position = hitPoint;
        //hitParticles.Play();

        

        if (currentHealth <= 0)
        {
            Death();
        }
    }



    void Death()
    {
        isDead = true;

        capsuleCollider.isTrigger = true;

        anim.SetTrigger("Dead");
        enemyManager.healList.Remove(this.gameObject);

        //enemyAudio.clip = deathClip;
        //enemyAudio.Play();
    }


}

