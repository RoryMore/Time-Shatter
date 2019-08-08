using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemyScript : EnemyScript
{

    public float meleeAttackRange;
    public int meleeDamage;
    
    
    

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


        enemyCooldown = 2.0f + Random.Range(1.0f, 4.0f);
        currentHealth = startingHealth;
    }


    void Update()
    {
        if (turnManger.state == turnManageScript.BattleState.BATTLE || turnManger.state == turnManageScript.BattleState.ACTION)
        {
            Movement();
            MeleeAttack();
            Turn();
        }
        if (Input.GetKeyDown("g") == true)
        {
            print("YAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAY");
            TakeDamage(10, this.gameObject.transform.position);
        }


    }


    public void Movement()
    {
        if(currentHealth > 0 && player.currentHealth > 0)
        {
            //If we're close enough to smack, stop moving
            if (Vector3.Distance(transform.position, player.gameObject.transform.position) < meleeAttackRange)
            {
                nav.SetDestination(transform.position);
                transform.LookAt(player.transform);

            }
            else
            {
                nav.SetDestination(GameObject.Find("Player").transform.position);
                
            }
            //Assuming we arent, get closer
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
    
    public void MeleeAttack()
    {

        float distance = Vector3.Distance(transform.position, player.transform.position);

        //We are ready to make our attack, and we are in range. ATTACK!
        if (distance <= meleeAttackRange && enemyCooldown <= 0.0f)
        {
            player.TakeDamage(meleeDamage);
            //Play Animation
            enemyCooldown = 6.0f;
            //Debug.Log("ATTACK!");
        }
        //If its the melee enemy turn BUT we are out of range, we go into defence stance!
        else if (meleeAttackRange <= distance && enemyCooldown <= 0.0f)
        {
            HoldTurn();
            //Debug.Log("She's too far!");
        }
        else if (meleeAttackRange <= distance && 0.0f <= enemyCooldown)
        {
            //Debug.Log("");
        }
    }

}
