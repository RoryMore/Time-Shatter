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
    public float timeLeftUntilAction = 6;
    public bool playerTakingAction = false;



    //Animator anim;
    //AudioSource playerAudio;

    bool isDead;
    bool damaged;
    //For animation
    private bool running = false;

    private NavMeshAgent navmeshAgent;
    private Transform self;
    // Start is called before the first frame update
    void Start()
    {
        navmeshAgent = GetComponent<NavMeshAgent>();
        self = GetComponentInParent<Transform>();
    }

    void Awake()
    {
        //anim = GetComponent <Animator> ();
        //playerAudio = GetComponent <AudioSource> ();
        currentHealth = startingHealth;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckDamage();
        //Movement();
        //NetherSwap();
        
        CountdownToTurn();
       


    }

    void CountdownToTurn()
    {
        if (playerTakingAction == false)
        {
            //Iterate on player timer
            timeLeftUntilAction -= 1f * Time.deltaTime;
            Mathf.Round(timeLeftUntilAction);
            print(timeLeftUntilAction);
            if (timeLeftUntilAction <= 0)
            {
                print("Player Turn!");
                playerTakingAction = true;
                TakeAction();
                //Do player action
            }
        }

    }

    void TakeAction()
    {
        //Script to determine how the player actually does their turn 
        //This one is gonna be complex!
        navmeshAgent.enabled = false;
        print("Were gonna pretend you did something!");
        navmeshAgent.enabled = true;



       
           
           ResetTurn(6);

       
        


        //Once the function is  complete reset the bool and timer to enter back into movement mode


    }

    void ResetTurn(float calcualtedTimeToNextAction)
    {
        playerTakingAction = false;
        timeLeftUntilAction = calcualtedTimeToNextAction;
    }

    void CheckDamage()
    {
        //Damage related aspect
        //Any visual damage cues here
    }

    void Movement()
    {
        if (playerTakingAction == false)
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

    void Attack()
    {

    }

    void Defend()
    {

    }

    void Item()
    {

    }

    void Haste()
    {

    }

    void Slow()
    {

    }

    void Blink()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit, 100))
            {
                self.localPosition = hit.point;
            }
        }
    }

    void SwapInitiatives()
    {

    }

    void NetherSwap()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        //Picks first target
        print("Select Target 1");
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit, 100))
            {
                
                GameObject tar1 = GameObject.Find(hit.collider.gameObject.name);
 
                //Pick Second target
                print("Select Target 2");
                if (Input.GetMouseButtonDown(0))
                {
                    if (Physics.Raycast(ray, out hit, 100))
                    {
                        GameObject tar2 = GameObject.Find(hit.collider.gameObject.name); //hit.point.
                        //Now we have both targets, we can 
                        Transform tempLocation = tar1.GetComponent<Transform>();
                        tar1.GetComponent<Transform>().localPosition = tar2.GetComponent<Transform>().localPosition;
                        tar2.GetComponent<Transform>().localPosition = tempLocation.localPosition;


                        


                    }
                }

            }
        }

        //Vector3 posA = tar1.GetComponent<Transform>().localPosition;
        //tar1.GetComponent<Transform>().localPosition = tar2.GetComponent<Transform>().localPosition;
        //tar2.GetComponent<Transform>().localPosition = posA;
        

    }


    public void RestartLevel()
    {
        SceneManager.LoadScene(0);
    }

}
