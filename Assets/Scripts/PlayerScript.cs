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

    float baseMoveSpeed;

    //public float timeLeftUntilAction = 6.0f;
    float timeSpentDoingAction = 0.0f;
     public bool isTakingAction = false;
    bool actionSelection = false;
    public bool isExecutingAbility = false;

    float oldInitiativeSpeed = 3.0f;
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
    Ability hasteAbility;
    // -------------------------------------------------------

    bool isDead;
    bool damaged;
    //For animation
    public bool running = false;

    private NavMeshAgent navmeshAgent;
    private Transform self;
    // Start is called before the first frame update
    void Start()
    {
        navmeshAgent = GetComponent<NavMeshAgent>();
        self = GetComponentInParent<Transform>();

        baseMoveSpeed = navmeshAgent.speed;
    }

    void Awake()
    {
        PopulateAbilitiesList();
        //anim = GetComponent <Animator> ();
        //playerAudio = GetComponent <AudioSource> ();
        currentHealth = maxHealth;

        initiativeSpeed = oldInitiativeSpeed;

        //isTakingAction = true;
        //actionSelection = true;
        //SelectAbility(attackID);
    }

    private void Update()
    {
        if (isDead)
        {
            Death();
        }
        else
        {
            if (isTakingAction || isExecutingAbility)
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

    void CheckDamage()
    {
        //Damage related aspect
        //Any visual damage cues here
    }

    void Movement()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //RaycastHit hit;

        if (Input.GetMouseButton(0))
        {
            if (Physics.Raycast(ray, out RaycastHit hit, 100))
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
            Debug.Log("PlayerScript: isTakingAction has to equal true here. We should be rotating the player");
            if (Input.GetMouseButtonDown(0))
            {
                isTakingAction = false;
                isExecutingAbility = true;

                navmeshAgent.enabled = false;
                Debug.Log("PlayerScript: Attack Action rotation chosen");
            }

            //navmeshAgent.enabled = false;

            // Rotate player towards point
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //RaycastHit hit;

            if (Physics.Raycast(ray, out RaycastHit hit, 200))
            {
                Vector3 dir = (hit.point - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(dir);
                lookRotation.x = transform.rotation.x;
                lookRotation.z = transform.rotation.z;
                
                transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, ((Time.deltaTime / 0.02f) * (1.0f + (1.0f - Time.timeScale))));
            }
            
        }
        else
        {
            //navmeshAgent.enabled = true;
            timeSpentDoingAction += Time.fixedDeltaTime;



            // Set player to attack animate

            // Draw a range indicator based on weapon attack type
        }

        if (timeSpentDoingAction >= selectedAbility.actionSpeed)
        {
            Debug.Log("PlayerScript: Attack action completing");
            // Check if an enemy is standing in front of player
            // Deal damage to them if they got hit

            // Stop player attack animation
            EndAction();
        }
    }

    void Defend()
    {
        timeSpentDoingAction += Time.fixedDeltaTime;
        
        Debug.Log("PlayerScript: Defend timeSpentDoingAction = " + timeSpentDoingAction);
        //navmeshAgent.enabled = false;
        if (isTakingAction && !isExecutingAbility)
        {
            navmeshAgent.speed = navmeshAgent.speed * 0.2f;
        }
        
        actionSelection = true;
        //isTakingAction = true;
        isExecutingAbility = true;

        Movement();

        // Animate Defense
        
        if (timeSpentDoingAction >= selectedAbility.actionSpeed)
        {
            navmeshAgent.speed = baseMoveSpeed;
            // Stop Defense animation
            Debug.Log("PlayerScript: Defend action finished");
            EndAction();
        }
    }

    void Item()
    {
        timeSpentDoingAction += Time.deltaTime;
        navmeshAgent.enabled = false;
        actionSelection = true;

        //isTakingAction = false;
        //isExecutingAbility = true;

        if (timeSpentDoingAction >= selectedAbility.actionSpeed)
        {
            EndAction();
        }
    }

    void Haste()
    {
        timeSpentDoingAction += Time.deltaTime;
        
        actionSelection = true;

        isTakingAction = false;
        isExecutingAbility = true;
        navmeshAgent.enabled = false;

        // Cast time anim

        if (timeSpentDoingAction >= selectedAbility.actionSpeed)
        {
            selectedAbility.isBuffActive = true;

            initiativeSpeed = oldInitiativeSpeed * selectedAbility.magnitude;

            EndAction();
        }
    }

    void Slow()
    {
        timeSpentDoingAction += Time.deltaTime;
        navmeshAgent.enabled = false;
        actionSelection = true;

        if (timeSpentDoingAction >= selectedAbility.actionSpeed)
        {
            EndAction();
        }
    }

    void Blink()
    {
        timeSpentDoingAction += Time.deltaTime;
        navmeshAgent.enabled = false;
        actionSelection = true;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //RaycastHit hit;

        if (Input.GetMouseButtonDown(1))
        {
            if (Physics.Raycast(ray, out RaycastHit hit, 100))
            {
                self.localPosition = hit.point;
            }
        }
    }

    void SwapInitiatives()
    {
        timeSpentDoingAction += Time.deltaTime;
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
                        Debug.Log("PlayerScript: NetherSwap Target 1: SET");
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
                        Debug.Log("PlayerScript: NetherSwap Target 2: SET");
                    }
                }
            }
        }

        // Both targets selected
        if (netherSwapAbility.target1 != null)
        {
            if (netherSwapAbility.target2 != null)
            {
                timeSpentDoingAction += Time.deltaTime;

                if (timeSpentDoingAction >= netherSwapAbility.actionSpeed)
                {
                    Vector3 tempT = netherSwapAbility.target1.position;

                    netherSwapAbility.target1.position = netherSwapAbility.target2.position;

                    netherSwapAbility.target2.position = tempT;

                    Debug.Log("PlayerScript: Targets NetherSwapped!");

                    EndAction();
                }
            }
        }
    }

    void DoAction()
    {
        if (selectedAbility != null)
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
        else
        {
            SelectAbilityViaKeyboardHotkey();
        }
    }

    void SelectAbilityViaKeyboardHotkey()
    {
        if (Input.GetKeyDown("1"))
        {
            SelectAbility(attackID);
            Debug.Log("PlayerScript: Attack selected.");
        }
        else if (Input.GetKeyDown("2"))
        {
            SelectAbility(defendID);
            Debug.Log("PlayerScript: Defense selected.");
        }
        else if (Input.GetKeyDown("3"))
        {
            SelectAbility(hasteID);
            Debug.Log("PlayerScript: Haste selected.");
        }
        else if (Input.GetKeyDown("4"))
        {
            SelectAbility(slowID);
            Debug.Log("PlayerScript: Slow selected.");
        }
        else if (Input.GetKeyDown("5"))
        {
            SelectAbility(blinkID);
            Debug.Log("PlayerScript: Blink selected.");
        }
        else if (Input.GetKeyDown("6"))
        {
            SelectAbility(netherSwapID);
            Debug.Log("PlayerScript: Have you ever played the game Switch?");
        }
        else if (Input.GetKeyDown("7"))
        {
            SelectAbility(initiativeSwapID);
            Debug.Log("PlayerScript: InitiativeSwap selected.");
        }
    }

    void EndAction()
    {
        isTakingAction = false;
        isExecutingAbility = false;
        actionSelection = false;

        timeSpentDoingAction = 0.0f;
        navmeshAgent.enabled = true;
        

        // Clear NetherSwap targeting
        netherSwapAbility.target1 = null;
        netherSwapAbility.target2 = null;

        //initiativeSpeed = oldInitiativeSpeed;

        // This foreach loop contains: ABILITY COOLDOWNS AND BUFFS/DEBUFFS
        foreach (Ability ability in abilities)
        {
            // If the ability has been used/is on cooldown - increment the cooldown check value
            if (ability.turnsBeenOnCooldown < ability.cooldown)
            {
                ability.turnsBeenOnCooldown++;
            }
            // Is the abilities buff active? Increment the turnsBuffed
            if (ability.isBuffActive)
            {
                if (ability.turnsBuffed < ability.buffDuration+1)
                {
                    ability.turnsBuffed++;
                }
                // Buff has been active for desired duration.
                // What ability was it, so what do we do next
                else if (ability.id == hasteID)
                {
                    ability.isBuffActive = false;
                    ability.turnsBuffed = 0;

                    // We are no longer buffed. Haste debuffs our initiativeSpeed after our buff finishes
                    initiativeSpeed = oldInitiativeSpeed * ((hasteAbility.magnitude * 1.75f) / hasteAbility.magnitude);
                    ability.isDebuffActive = true;
                }
            }
            // Is the abilities debuff active? Increment the turnsDebuffed
            if (ability.isDebuffActive)
            {
                if (ability.turnsDebuffed < ability.debuffDuration)
                {
                    ability.turnsDebuffed++;
                }
                // debuff has been active for desired duration.
                // What ability was it, so what do we do next
                else if (ability.id == hasteID)
                {
                    ability.isDebuffActive = false;
                    ability.turnsDebuffed = 0;

                    initiativeSpeed = oldInitiativeSpeed;
                }
            }
        }

        selectedAbility.turnsBeenOnCooldown = 0;
        selectedAbility = null;
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
                    hasteAbility = child.GetComponent<Ability>();
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
                if (ability.turnsBeenOnCooldown >= ability.cooldown)
                {
                    actionSelection = true;
                    selectedAbility = ability;
                    break;
                }
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
