﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangedEnemyScript : EnemyScript
{
    // Start is called before the first frame update

    public float rangedAttackRange;
    public float rangedDamage;

    public GameObject bolt;
    PlayerAttack ourAttack;




    void Awake()
    {
        anim = GetComponent<Animator>();
        //enemyAudio = GetComponent<AudioSource>();
        hitParticles = GetComponentInChildren<ParticleSystem>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        nav = GetComponent<NavMeshAgent>();


        player = GameObject.Find("Player").GetComponent<PlayerScript>();
        turnManger = GameObject.Find("TurnManager").GetComponent<turnManageScript>();
        enemyManager = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();

        //There own "turn start" number
        enemyCooldown = 6.0f;
        //enemyCooldown = 3.0f + Random.Range(1.0f, 3.0f);
        //Speed of movement
        initiativeSpeed = 1.2f;




        currentHealth = startingHealth;

        ourAttack = GetComponent<PlayerAttack>();

    }


    void Update()
    {
        if (turnManger.state == turnManageScript.BattleState.BATTLE || turnManger.state == turnManageScript.BattleState.ACTION)
        {

            Turn();
            Movement();
            RangedAttack();

        }

    }


    public void Movement()
    {
        if (currentHealth > 0 && player.currentHealth > 0)
        {

            float distance = Vector3.Distance(transform.position, player.transform.position);
            
            //If we are too close, run away
            if (distance < rangedAttackRange -5)
            {
                nav.SetDestination(Vector3.MoveTowards(transform.position, player.transform.position, -nav.speed));
            }
            else if (distance > nav.stoppingDistance)
            {
                nav.SetDestination(Vector3.MoveTowards(transform.position, player.transform.position, nav.speed));
            }
            else if (distance <= nav.stoppingDistance && distance >= rangedAttackRange)
            {
                nav.SetDestination(transform.position);
            }
            else
            {
                nav.SetDestination(player.transform.position);
            }


        }
        else
        {
            nav.enabled = false;
        }
    }

    //TEMPORARY FUNCTION FOR WHEN JASMINE FINISHES HER TURN COUNTER
    public void Turn()
    {
        enemyCooldown -= 1f * Time.deltaTime;
        //Debug.Log("Enemy Cooldown Counter: " + enemyCooldown);

    }


    public void RangedAttack()
    {
        

        float distance = Vector3.Distance(transform.position, player.transform.position);

        //We are ready to make our attack, and we are in range. ATTACK!
        if (distance <= rangedAttackRange && enemyCooldown <= 0.0f)
        {
            //Use the more complicated but nicer turn method that means that slow time will affect it as well.
            //FaceTarget(player.transform);
            transform.LookAt(player.transform);
            nav.enabled = false;
            timeSpentDoingAction += Time.fixedDeltaTime;

            ourAttack.DrawCastTimeRangeIndicator(timeSpentDoingAction);

            if (timeSpentDoingAction >= ourAttack.actionSpeed)
            {
                Instantiate(bolt, transform.position, Quaternion.identity);
                player.TakeDamage(rangedDamage);
                Debug.Log("FIRE!");

                //Play Animation
                enemyCooldown = 6.0f;
                timeSpentDoingAction = 0.0f;
                nav.enabled = true;
            }
            //Debug.Log("ATTACK!");
          

            //player.TakeDamage(meleeDamage);
           
            //Play Animation
            //Face towards player!

            
           
           
            
            

        }
        //If its the range enemy turn BUT we are out of range, we go into defence stance!
        else if (rangedAttackRange <= distance && enemyCooldown <= 0.0f)
        {
            //If they have an another ability it can activate here given enough time has gone by for a cooldown
            HoldTurn();
            //Debug.Log("Somehow I am actually out of ranged");
        }
        else if (rangedAttackRange <= distance && 0.0f <= enemyCooldown)
        {
           // Debug.Log("It isn't the turn yet...");
        }
        else
        {
           // Debug.Log("Should never trigger ya dumbo");
        }
    }
}
