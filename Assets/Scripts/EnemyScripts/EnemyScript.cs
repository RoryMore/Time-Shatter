using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//The base enemy script from which all enemies derrive 

public class EnemyScript : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;

    public float enemyCooldown;
    public bool enemyTakingAction;

    //public AudioClip deathClip;


    public Animator anim;
    //AudioSource enemyAudio;
    public ParticleSystem hitParticles;
    public CapsuleCollider capsuleCollider;

    public NavMeshAgent nav;

    public PlayerScript player;

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

    //Function that is called when the player deals damage to you
    //Default condition format
    public void TakeDamage(int amount, Vector3 hitPoint)
    {
        if (isDead)
            return;

        //Audio Cue
        //enemyAudio.Play();

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

        //enemyAudio.clip = deathClip;
        //enemyAudio.Play();
    }
}

