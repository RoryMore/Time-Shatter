using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HealerEnemyScript : EnemyScript
{
    public float healRange;
    public int healMagnitude;
    turnManageScript turnManger;
    EnemyManager enemyManager;
    GameObject target;

    bool firstTargetAqquired = false;

    // Start is called before the first frame update
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


        enemyCooldown = 4.0f + Random.Range(1.0f, 2.0f);
        currentHealth = startingHealth;


        //Once the healer starts, pick a random enemy in the field
        
        


    }

    // Update is called once per frame
    void Update()
    {
        if (turnManger.state == turnManageScript.BattleState.BATTLE || turnManger.state == turnManageScript.BattleState.ACTION)
        {
            Movement();
            //Heal();
            //MeleeAttack();
            Turn();
        }
    }

    public void Movement()
    {
        // At first, set target to be a random enemy in the list
        if (firstTargetAqquired == false)
        {
            target = enemyManager.initiativeList[(int)Random.Range(0, enemyManager.initiativeList.Count)];
            firstTargetAqquired = true;

            Heal();
        }
        // When enemies take damage, 

        if (currentHealth > 0 && player.currentHealth > 0)
        {
            


            nav.SetDestination(target.transform.position);
            print("Going towards " + target.name);
        }
        else
        {
            nav.enabled = false;
        }
    }

    public void Turn()
    {
        enemyCooldown -= 1f * Time.deltaTime;
        //Debug.Log("Enemy Cooldown Counter: " + enemyCooldown);

    }

    void Heal()
    {
        //If the heal list isnt empty
        if (enemyManager.healList.Count != 0)
        {
            //Absurdly large number to remove
            float previousDistanceToAlly = 20000000000000000000000.0f;
            
            //Get distance between all enemies in the heal list and yourself, getting the closest one.
            foreach (GameObject enemy in enemyManager.healList)
            {
                float distanceToAlly = Vector3.Distance(transform.position, enemy.transform.position);
                if (distanceToAlly <= previousDistanceToAlly)
                {
                    //The closest one is now your target
                    target = enemy;
                 
                }

                //Make sure our heal
                if (Vector3.Distance(transform.position, target.transform.position) < healRange)
                {
                    //
                }
                
            }


            
            //enemyManager.healList[0].gameObject.transform
        }
    }
}










//TEMPORARY FUNCTION FOR WHEN JASMINE FINISHES HER TURN COUNTER


//public void MeleeAttack()
//{

//    float distance = Vector3.Distance(transform.position, player.transform.position);

//    //We are ready to make our attack, and we are in range. ATTACK!
//    if (distance <= meleeAttackRange && enemyCooldown <= 0.0f)
//    {
//        player.TakeDamage(meleeDamage);
//        //Play Animation
//        enemyCooldown = 6.0f;
//        //Debug.Log("ATTACK!");
//    }
//    //If its the melee enemy turn BUT we are out of range, we go into defence stance!
//    else if (meleeAttackRange <= distance && enemyCooldown <= 0.0f)
//    {
//        enemyCooldown = 6.0f;
//        //Debug.Log("She's too far!");
//    }
//    else if (meleeAttackRange <= distance && 0.0f <= enemyCooldown)
//    {
//        //Debug.Log("");
//    }
//}
