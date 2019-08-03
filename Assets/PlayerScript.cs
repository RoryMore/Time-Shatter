using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{

    public int startingHealth = 100;
    public int currentHealth;
    //public Slider healthSlider;
    //public Image damageImage;
    //public AudioClip deathClip;
    //public float flashSpeed = 5f;
    // public Color flashColour = new Color(1f, 0f, 0f, 0.1f);


    //Animator anim;
    //AudioSource playerAudio;
    //PlayerMovement playerMovement;
    //PlayerShooting playerShooting;
    bool isDead;
    bool damaged;
    //For animation
    private bool running = false;

    private NavMeshAgent navmeshAgent;
    // Start is called before the first frame update
    void Start()
    {
        navmeshAgent = GetComponent<NavMeshAgent>();
    }

    void Awake()
    {
        //anim = GetComponent <Animator> ();
        //playerAudio = GetComponent <AudioSource> ();
        currentHealth = startingHealth;
    }

    // Update is called once per frame
    void Update()
    {
        CheckDamage();
        Movement();

       


    }

    void CheckDamage()
    {
        //Damage related aspect
        if (damaged)
        {
            //damageImage.color = flashColour;
        }
        else
        {
            //damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        damaged = false;
    }

    void Movement()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit, 100))
            {
                navmeshAgent.destination = hit.point;
            }
        }

        //Animation stuff
        if (navmeshAgent.remainingDistance <= navmeshAgent.stoppingDistance)
        {
            running = false;
        }
        else
        {
            running = true;
        }

        //animator.SetBool("running", running);
    }



    public void TakeDamage(int amount)
    {
        damaged = true;

        currentHealth -= amount;

        //healthSlider.value = currentHealth;

        //playerAudio.Play ();

        if (currentHealth <= 0 && !isDead)
        {
            Death();
        }
    }


    void Death()
    {
        isDead = true;

        

        //anim.SetTrigger ("Die");

        //playerAudio.clip = deathClip;
        //playerAudio.Play ();

    }


    public void RestartLevel()
    {
        SceneManager.LoadScene(0);
    }

}
