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


        enemyCooldown = 4.0f;
        currentHealth = startingHealth;
    }


    void Update()
    {

            Movement();
            MeleeAttack();
            Turn();




    }


    public void Movement()
    {
        if(currentHealth > 0 && player.currentHealth > 0)
        {
        nav.SetDestination(GameObject.Find("Player").transform.position);
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
            enemyCooldown = 6.0f;
            //Debug.Log("She's too far!");
        }
        else if (meleeAttackRange <= distance && 0.0f <= enemyCooldown)
        {
            //Debug.Log("");
        }
    }

}
