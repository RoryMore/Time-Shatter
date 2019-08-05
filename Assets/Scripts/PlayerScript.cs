using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public float maxHealth = 100.0f;
    public float currentHealth;

    //public float timeLeftUntilAction = 6.0f;
    float timeSpentDoingAction = 0.0f;
    [HideInInspector] public bool isTakingAction = false;
    bool actionSelection = false;

    float oldInitiativeSpeed = 2.0f;
    public float initiativeSpeed = 2.0f;    // If turns are to change to a different speed system
                                            // How long it takes for the player to reach their action
    public float initiativeEntrySpeed = 3.0f;

    //Animator anim;
    //AudioSource playerAudio;

    // -Abilities -------------------------------------------
    public List<Ability> abilities;
    [HideInInspector] public Ability selectedAbility = null;

    public int attackID;
    public int defendID;
    public int itemID;
    public int hasteID;
    public int slowID;
    public int blinkID;
    public int netherSwapID;
    public int initiativeSwapID;

    NetherSwap netherSwapAbility;
    // -------------------------------------------------------

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
        PopulateAbilitiesList();
        //anim = GetComponent <Animator> ();
        //playerAudio = GetComponent <AudioSource> ();
        currentHealth = maxHealth;

        initiativeSpeed = oldInitiativeSpeed;

        //  isTakingAction = true;
        actionSelection = true;
        SelectAbility(attackID);
    }

    private void Update()
    {
        if (isDead)
        {
            Death();
        }
        else
        {
            if (isTakingAction)
            {
                //timeSpentDoingAction += (Time.deltaTime * (1.0f + (1.0f - Time.timeScale)));
                //if (timeSpentDoingAction >= 0.9f)
                //{
                //EndAction();
                //}
                DoAction();
            }
            else
            {
                Movement();
            }

            CheckDamage();
        }
    
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }
    //void CountdownToTurn()
    //{
    //    if (playerTakingAction == false)
    //    {
    //        //Iterate on player timer
    //        timeLeftUntilAction -= 1f * Time.deltaTime;
    //        Mathf.Round(timeLeftUntilAction);
    //        print(timeLeftUntilAction);
    //        if (timeLeftUntilAction <= 0)
    //        {
    //            print("Player Turn!");
    //            playerTakingAction = true;
    //            TakeAction();
    //            //Do player action
    //        }
    //    }

    //}

    //void ResetTurn(float calcualtedTimeToNextAction)
    //{
    //    playerTakingAction = false;
    //    timeLeftUntilAction = calcualtedTimeToNextAction;
    //}

    void CheckDamage()
    {
        //Damage related aspect
        //Any visual damage cues here
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



    public void TakeDamage(float amount)
    {
        damaged = true;

        float damageToTake = amount;

        if (selectedAbility.type == Ability.Type.Defend)
        {
            damageToTake = amount - selectedAbility.magnitude;
            if (damageToTake < 0.0f)
            {
                damageToTake = 0.0f;
            }
        }

        currentHealth -= damageToTake;

        //healthSlider.value = currentHealth;

        //playerAudio.Play ();

        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
        }
    }


    void Death()
    {
        //anim.SetTrigger ("Die");

        //playerAudio.clip = deathClip;
        //playerAudio.Play ();

    }

    void Attack()
    {
        if (isTakingAction)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isTakingAction = false;
            }

            navmeshAgent.enabled = false;

            // Rotate player towards point
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                Vector3 dir = (hit.point - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(dir);
                lookRotation.x = transform.rotation.x;
                lookRotation.z = transform.rotation.z;
                
                transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, ((Time.deltaTime / 0.05f) * (1.0f + (1.0f - Time.timeScale))));
            }
            
        }
        else
        {
            //navmeshAgent.enabled = true;
            timeSpentDoingAction += Time.deltaTime;

            // Set player to attack animate
        }

        if (timeSpentDoingAction >= selectedAbility.actionSpeed)
        {
            // Check if an enemy is standing in front of player
            // Deal damage to them if they got hit

            // Stop player attack animation
            EndAction();
        }
    }

    void Defend()
    {
        timeSpentDoingAction += Time.fixedDeltaTime;
        navmeshAgent.enabled = false;
        actionSelection = true;

        // Animate Defense

        if (timeSpentDoingAction >= selectedAbility.actionSpeed)
        {
            // Stop Defense animation

            EndAction();
        }
    }

    void Item()
    {
        timeSpentDoingAction += Time.fixedDeltaTime;
        navmeshAgent.enabled = false;
        actionSelection = true;

        if (timeSpentDoingAction >= selectedAbility.actionSpeed)
        {
            EndAction();
        }
    }

    void Haste()
    {
        timeSpentDoingAction += Time.fixedDeltaTime;
        navmeshAgent.enabled = false;
        actionSelection = true;

        if (timeSpentDoingAction >= selectedAbility.actionSpeed)
        {
            EndAction();
        }
    }

    void Slow()
    {
        timeSpentDoingAction += Time.fixedDeltaTime;
        navmeshAgent.enabled = false;
        actionSelection = true;

        if (timeSpentDoingAction >= selectedAbility.actionSpeed)
        {
            EndAction();
        }
    }

    void Blink()
    {
        timeSpentDoingAction += Time.fixedDeltaTime;
        navmeshAgent.enabled = false;
        actionSelection = true;

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
        timeSpentDoingAction += Time.fixedDeltaTime;
        navmeshAgent.enabled = false;
        actionSelection = true;

        if (timeSpentDoingAction >= selectedAbility.actionSpeed)
        {
            EndAction();
        }
    }

    void NetherSwap()
    {
        navmeshAgent.enabled = false;
        actionSelection = true;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Do we have a First target?
        if (netherSwapAbility.target1 == null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(ray, out hit, 200))
                {
                    // Check validity of target
                    if (IsValidNetherSwapTarget(hit.collider.gameObject))
                    {
                        // Set First target
                        netherSwapAbility.target1 = hit.collider.gameObject.transform;
                        Debug.Log("NetherSwap Target 1: SET");
                    }
                }
            }
        }
        // Do we have a Second target?
        else if (netherSwapAbility.target2 == null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(ray, out hit, 200))
                {
                    // Check validity of target
                    if (IsValidNetherSwapTarget(hit.collider.gameObject))
                    {
                        // Set Second target
                        netherSwapAbility.target2 = hit.collider.gameObject.transform;
                        Debug.Log("NetherSwap Target 2: SET");
                    }
                }
            }
        }

        // Both targets selected
        if (netherSwapAbility.target1 != null)
        {
            if (netherSwapAbility.target2 != null)
            {
                timeSpentDoingAction += Time.fixedDeltaTime;

                if (timeSpentDoingAction >= netherSwapAbility.actionSpeed)
                {
                    Vector3 tempT = netherSwapAbility.target1.position;

                    netherSwapAbility.target1.position = netherSwapAbility.target2.position;

                    netherSwapAbility.target2.position = tempT;

                    Debug.Log("Targets NetherSwapped!");

                    EndAction();
                }
            }
        }
    }

    void DoAction()
    {
        switch (selectedAbility.type)
        {
            case Ability.Type.WeaponAttack:
                Attack();
                break;

            case Ability.Type.Defend:
                Defend();
                break;

            case Ability.Type.Item:
                Item();
                break;

            case Ability.Type.Blink:
                Blink();
                break;

            case Ability.Type.Haste:
                Haste();
                break;

            case Ability.Type.Slow:
                Slow();
                break;

            case Ability.Type.InitiativeSwap:
                SwapInitiatives();
                break;

            case Ability.Type.Swap:
                NetherSwap();
                break;

            default:
                break;
        }
    }

    void EndAction()
    {
        isTakingAction = false;
        timeSpentDoingAction = 0.0f;
        navmeshAgent.enabled = true;
        actionSelection = false;

        // Clear NetherSwap targeting
        netherSwapAbility.target1 = null;
        netherSwapAbility.target2 = null;

        initiativeSpeed = oldInitiativeSpeed;
    }


    public void RestartLevel()
    {
        SceneManager.LoadScene(0);
    }

    void PopulateAbilitiesList()
    {
        Transform directChild = transform;
        if (directChild.GetChild(0).name == "PlayerAbilities")
        {
            directChild = transform.GetChild(0);
        }

        int abilityId = 0;
        foreach (Transform child in directChild)
        {
            abilities.Add(child.GetComponent<Ability>());
            child.GetComponent<Ability>().id = abilityId;

            switch (child.GetComponent<Ability>().type)
            {
                case Ability.Type.WeaponAttack:
                    attackID = abilityId;
                    break;

                case Ability.Type.Defend:
                    defendID = abilityId;
                    break;

                case Ability.Type.Item:
                    itemID = abilityId;
                    break;

                case Ability.Type.Haste:
                    hasteID = abilityId;
                    break;

                case Ability.Type.Slow:
                    slowID = abilityId;
                    break;

                case Ability.Type.Blink:
                    blinkID = abilityId;
                    break;

                case Ability.Type.Swap:
                    netherSwapID = abilityId;
                    netherSwapAbility = (NetherSwap)child.GetComponent<Ability>();
                    break;

                case Ability.Type.InitiativeSwap:
                    initiativeSwapID = abilityId;
                    break;

                default:
                    break;
            }

            abilityId++;
        }
    }

    public void SelectAbility(int id)
    {
        foreach (Ability ability in abilities)
        {
            if (ability.id == id)
            {
                actionSelection = true;
                selectedAbility = ability;
                break;
            }
        }
    }

    bool IsValidNetherSwapTarget(GameObject teleportedObject)
    {
        if (teleportedObject.tag.Contains("Player") || teleportedObject.tag.Contains("Enemy"))
        {
            return true;
        }
        Debug.Log("LOL, you tried teleporting something you CAN'T! HAH");
        return false;
    }
}
