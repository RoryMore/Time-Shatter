using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemyScript : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;

    public AudioClip deathClip;


    Animator anim;
    AudioSource enemyAudio;
    ParticleSystem hitParticles;
    CapsuleCollider capsuleCollider;

    NavMeshAgent nav;

    Transform playerLocation;
    PlayerScript player;

    bool isDead;
    bool isSinking;


    void Awake()
    {
        anim = GetComponent<Animator>();
        enemyAudio = GetComponent<AudioSource>();
        hitParticles = GetComponentInChildren<ParticleSystem>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        nav = GetComponent<NavMeshAgent>();

        player = GameObject.Find("Player").GetComponent<PlayerScript>();
        playerLocation = GameObject.Find("Player").transform;

        currentHealth = startingHealth;
    }


    void Update()
    {
        if (player.isTakingAction == false)
        {
            nav.enabled = true;
            Movement();
        }
        else
        {
            nav.enabled = false;
        }



    }

    //Function that is called when the player deals damage to you
    //Default condition format
    public void TakeDamage(int amount, Vector3 hitPoint)
    {
        if (isDead)
            return;

        //Audio Cue
        enemyAudio.Play();

        currentHealth -= amount;

        hitParticles.transform.position = hitPoint;
        hitParticles.Play();

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

        enemyAudio.clip = deathClip;
        enemyAudio.Play();
    }




    public void Movement()
    {
        if(currentHealth > 0 && player.currentHealth > 0)
        {
        nav.SetDestination(playerLocation.position);
        }
        else
        {
            nav.enabled = false;
        }
    }
}
