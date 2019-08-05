using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangedEnemyScript : EnemyScript
{
    // Start is called before the first frame update


    public float rangedAttackRange;
    public int rangedDamage;

    public float followSharpness = 0.1f;

    Vector3 _followOffest;

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

        _followOffest = transform.position - player.transform.position;

        //Set initial position choice
        
    }


    void Update()
    {
        //While the player isn't in the "take turn" stage, follow the player but otherwise stop
        if (player.playerTakingAction == false)
        {
            nav.enabled = true;
            Movement();
            RangedAttack();
            Turn();
        }
        else
        {

            nav.enabled = false;

            //Pause animation
        }



    }


    public void Movement()
    {
        if (currentHealth > 0 && player.currentHealth > 0)
        {
            nav.SetDestination(player.transform.position);

            float distance = Vector3.Distance(transform.position, player.transform.position);

            if (distance <= rangedAttackRange)
            {


                nav.SetDestination(transform.position);
            }
            else if (distance >= rangedAttackRange/2)
            {
                


                ////Get angle between current position and player position
                //float xDiff = transform.position.x - player.transform.position.x;
                //float zDiff = transform.position.z - player.transform.position.z;
                ////Angle!
                //float angle = Mathf.Atan2(zDiff, xDiff) * 180 / Mathf.PI;



                //float newX = player.transform.position.x + Mathf.Cos(angle) * rangedAttackRange;
                //float newZ = player.transform.position.z + Mathf.Sin(angle) * rangedAttackRange;

                //Vector3 newTar = new Vector3(newX, transform.position.y, newZ);
                //nav.SetDestination(newTar);

                //nav.SetDestination(player.transform.position)
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
            //player.TakeDamage(meleeDamage);
            nav.enabled = false;
            //Play Animation
            enemyCooldown = 6.0f;
            Debug.Log("FIRE!");

        }
        //If its the melee enemy turn BUT we are out of range, we go into defence stance!
        else if (rangedAttackRange <= distance && enemyCooldown <= 0.0f)
        {
            enemyCooldown = 6.0f;
            Debug.Log("Somehow I am actually out of ranged");
        }
        else if (rangedAttackRange <= distance && 0.0f <= enemyCooldown)
        {
            Debug.Log("It isn't the turn yet...");
        }
        else
        {
            Debug.Log("Should never trigger ya dumbo");
        }
    }
}
