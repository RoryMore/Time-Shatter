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
    // Current moveSpeed is the navmeshAgent.speed variable

    float oldInitiativeSpeed = 3.0f;
    // If turns are to change to a different speed system
    // How long it takes for the player to reach their action
    public float initiativeSpeed = 2.0f;

    // Modifier to determine how strong physical attacks/abilities are
    //float strength;

    // Name can be changed to whatever. 
    //Purpose would be a to increase/decrease how powerful non-melee/physical attacks would be
    //float spellPower; 

    //public float timeLeftUntilAction = 6.0f;
    public float timeSpentDoingAction = 0.0f;
     public bool isTakingAction = false;
    bool actionSelection = false;
    public bool isExecutingAbility = false;

       
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
    public int waitID;

    PlayerAttack attackAbility;
    Ability defendAbility;
    NetherSwap netherSwapAbility;
    Ability hasteAbility;
    SlowAbility slowAbility;
    BlinkAbility blinkAbility;
    InitiativeSwapAbility initiativeSwapAbility;
    Ability waitAbility;
    // -------------------------------------------------------

    bool isDead;
    bool damaged;
    //For animation
    public bool running = false;

    private NavMeshAgent navmeshAgent;

    turnManageScript turnManager;
    // Start is called before the first frame update
    void Start()
    {
        navmeshAgent = GetComponent<NavMeshAgent>();

        baseMoveSpeed = navmeshAgent.speed;

        turnManager = FindObjectOfType<turnManageScript>();
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
            if ((turnManager.state == turnManageScript.BattleState.BATTLE) || (turnManager.state == turnManageScript.BattleState.ACTION))
            {
                if (isTakingAction || isExecutingAbility)
                {
                    DoAction();
                }
                else
                {
                    Movement();
                }

                CheckDamage();
            }
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
        Debug.Log("Player Should Take Damage NOW!!!!!");

        damaged = true;

        float damageToTake = amount;

        if (defendAbility.isBuffActive)
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
            //Debug.Log("PlayerScript: isTakingAction has to equal true here. We should be rotating the player");
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

            // While choosing where to attack, show where we will be attacking
            attackAbility.DrawRangeIndicator();
            
        }
        else
        {
            //navmeshAgent.enabled = true;
            timeSpentDoingAction += Time.fixedDeltaTime;

            // Set player to attack animate

            // Draw a range indicator based on weapon attack type
            attackAbility.DrawCastTimeRangeIndicator(timeSpentDoingAction);
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

        selectedAbility.isBuffActive = true;
        selectedAbility.isDebuffActive = true;

        //Debug.Log("PlayerScript: Defend timeSpentDoingAction = " + timeSpentDoingAction);
        //navmeshAgent.enabled = false;
        if (isTakingAction && !isExecutingAbility)
        {
            navmeshAgent.speed = navmeshAgent.speed * 0.4f;
        }
        
        actionSelection = true;
        isTakingAction = false;
        isExecutingAbility = true;

        Movement();

        // Animate Defense
        
        if (timeSpentDoingAction >= selectedAbility.actionSpeed)
        {
            navmeshAgent.speed = navmeshAgent.speed * 1.6f;
            selectedAbility.turnsBuffed = 0;
            
            selectedAbility.turnsDebuffed = 0;
            // Stop Defense animation
            //Debug.Log("PlayerScript: Defend action finished");
            EndAction();
        }
    }

    void Item()
    {
        timeSpentDoingAction += Time.fixedDeltaTime;
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
        timeSpentDoingAction += Time.fixedDeltaTime;
        
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

    // Currently the slow will stay on the target for 2 of Player's turns
    // I.e if player hastes while a target is slowed, the target may become unslowed unintentionally quickly
    void Slow()
    {
        //actionSelection = true;

        if (slowAbility.targettedEnemy == null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, 200))
                {
                    if (IsValidSlowTarget(hit.collider.gameObject))
                    {
                        slowAbility.targettedEnemy = hit.collider.gameObject.GetComponent<EnemyScript>();
                        isTakingAction = false;
                        isExecutingAbility = true;

                        navmeshAgent.enabled = false;
                    }
                }
            }
        }
        // We have a target
        else
        {
            timeSpentDoingAction += Time.fixedDeltaTime;
        }

        if (timeSpentDoingAction >= selectedAbility.actionSpeed)
        {
            slowAbility.targettedEnemy.enemyCooldown = slowAbility.targettedEnemy.enemyCooldown / selectedAbility.magnitude;
            selectedAbility.isDebuffActive = true;

            EndAction();
        }
    }

    void Blink()
    {
        actionSelection = true;

        if (isTakingAction)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(ray, out RaycastHit hit, 400))
                {
                    blinkAbility.blinkLocation = hit.point;
                    blinkAbility.blinkLocation.y += 0.5f;

                    isTakingAction = false;
                    isExecutingAbility = true;

                    navmeshAgent.enabled = false;
                }
            }
        }
        else if (isExecutingAbility)
        {
            timeSpentDoingAction += Time.fixedDeltaTime;

            // Animate cast-time
        }
        

        if (timeSpentDoingAction >= selectedAbility.actionSpeed)
        {
            // Stop animate cast-time
            // Animate spell cast
            transform.position = blinkAbility.blinkLocation;

            EndAction();
        }
    }

    void SwapInitiatives()
    {
        actionSelection = true;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (initiativeSwapAbility.target1 == null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(ray, out RaycastHit hit, 400))
                {
                    // Check validity of target
                    if (IsValidInitiativeSwapTarget(hit.collider.gameObject))
                    {
                        initiativeSwapAbility.target1 = hit.collider.gameObject.GetComponent<EnemyScript>();
                        Debug.Log("PlayerScript: InitiativeSwap Target 1: SET");
                    }
                }
            }
        }
        else if (initiativeSwapAbility.target2 == null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(ray, out RaycastHit hit, 400))
                {
                    // Check validity of target
                    if (IsValidInitiativeSwapTarget(hit.collider.gameObject))
                    {
                        initiativeSwapAbility.target2 = hit.collider.gameObject.GetComponent<EnemyScript>();
                        Debug.Log("PlayerScript: InitiativeSwap Target 2: SET");

                        isTakingAction = false;
                        isExecutingAbility = true;
                        navmeshAgent.enabled = false;
                    }
                }
            }
        }
        else
        {
            timeSpentDoingAction += Time.fixedDeltaTime;
        }

        if (timeSpentDoingAction >= selectedAbility.actionSpeed)
        {
            float tempF = initiativeSwapAbility.target1.enemyCooldown;
            initiativeSwapAbility.target1.enemyCooldown = initiativeSwapAbility.target2.enemyCooldown;
            initiativeSwapAbility.target2.enemyCooldown = tempF;

            initiativeSwapAbility.isDebuffActive = true;
            Debug.Log("PlayerScript: Enemy initiatives swapped!");

            EndAction();
        }
    }

    void NetherSwap()
    {
        //navmeshAgent.enabled = false;
        actionSelection = true;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //RaycastHit hit;

        // Do we have a First target?
        if (netherSwapAbility.target1 == null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(ray, out RaycastHit hit, 400))
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
                if (Physics.Raycast(ray, out RaycastHit hit, 400))
                {
                    // Check validity of target
                    if (IsValidNetherSwapTarget(hit.collider.gameObject))
                    {
                        // Set Second target
                        netherSwapAbility.target2 = hit.collider.gameObject.transform;
                        Debug.Log("PlayerScript: NetherSwap Target 2: SET");

                        isTakingAction = false;
                        isExecutingAbility = true;
                        navmeshAgent.enabled = false;
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

                // Animate cast time

                if (timeSpentDoingAction >= netherSwapAbility.actionSpeed)
                {
                    // Stop cast time animate
                    // Animate spell cast?

                    Vector3 tempT = netherSwapAbility.target1.position;

                    netherSwapAbility.target1.position = netherSwapAbility.target2.position;

                    netherSwapAbility.target2.position = tempT;

                    Debug.Log("PlayerScript: Targets NetherSwapped!");

                    EndAction();
                }
            }
        }
    }

    void Wait()
    {
        timeSpentDoingAction += Time.fixedDeltaTime;

        actionSelection = true;

        isTakingAction = false;
        isExecutingAbility = true;
        navmeshAgent.enabled = false;

        // Cast time anim

        if (timeSpentDoingAction >= selectedAbility.actionSpeed)
        {
            selectedAbility.isDebuffActive = true;

            initiativeSpeed += selectedAbility.magnitude;
            Debug.Log("PlayerScript: initiativeSpeed = " + initiativeSpeed);

            EndAction();
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

                case Ability.Type.Wait:
                    Wait();
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
            Debug.Log("PlayerScript: Attempted to select Attack");
            SelectAbility(attackID);
            
        }
        else if (Input.GetKeyDown("2"))
        {
            Debug.Log("PlayerScript: Attempted to select Defend");
            SelectAbility(defendID);
            
        }
        else if (Input.GetKeyDown("3"))
        {
            Debug.Log("PlayerScript: Attempted to select Haste");
            SelectAbility(hasteID);
            
        }
        else if (Input.GetKeyDown("4"))
        {
            Debug.Log("PlayerScript: Attempted to select Slow");
            SelectAbility(slowID);
            
        }
        else if (Input.GetKeyDown("5"))
        {
            Debug.Log("PlayerScript: Attempted to select Blink");
            SelectAbility(blinkID);
            
        }
        else if (Input.GetKeyDown("6"))
        {
            Debug.Log("PlayerScript: Attempted to select NetherSwap");
            SelectAbility(netherSwapID);
            
        }
        else if (Input.GetKeyDown("7"))
        {
            Debug.Log("PlayerScript: Attempted to select InitiativeSwap");
            SelectAbility(initiativeSwapID);
            
        }
        else if (Input.GetKeyDown("8"))
        {
            Debug.Log("PlayerScript: Attempted to select Wait");
            SelectAbility(waitID);
        }
    }

    void EndAction()
    {
        isTakingAction = false;
        isExecutingAbility = false;
        actionSelection = false;

        timeSpentDoingAction = 0.0f;
        navmeshAgent.enabled = true;
        
        // - CLEAR SPELL TARGETTING -----------
        // Clear NetherSwap targeting
        netherSwapAbility.target1 = null;
        netherSwapAbility.target2 = null;

        slowAbility.targettedEnemy = null;

        initiativeSwapAbility.target1 = null;
        initiativeSwapAbility.target2 = null;
        // ------------------------------------

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
                if (ability.turnsBuffed >= ability.buffDuration)
                {
                    if (ability.id == hasteID)
                    {
                        ability.isBuffActive = false;
                        ability.turnsBuffed = 0;

                        // We are no longer buffed. Haste debuffs our initiativeSpeed after our buff finishes
                        initiativeSpeed = oldInitiativeSpeed * ((hasteAbility.magnitude * 1.75f) / hasteAbility.magnitude);
                        ability.isDebuffActive = true;
                        Debug.Log("PlayerScript: hasteAbility->turnsDebuffed = " + hasteAbility.turnsDebuffed);
                    }
                    
                }
                // Buff has been active for desired duration.
                // What ability was it, so what do we do next
                else
                {
                    ability.turnsBuffed++;
                }
            }
            // Is the abilities debuff active? Increment the turnsDebuffed
            if (ability.isDebuffActive)
            {
                if (ability.turnsDebuffed >= ability.debuffDuration)
                { 
                    ability.isDebuffActive = false;
                    ability.turnsDebuffed = 0;

                    if (ability.id == hasteID)
                    {
                        initiativeSpeed = oldInitiativeSpeed;
                    }
                    else if (ability.id == slowID)
                    {
                        slowAbility.targettedEnemy.enemyCooldown = slowAbility.targettedEnemy.enemyCooldown * selectedAbility.magnitude;
                        slowAbility.targettedEnemy = null;
                    }
                    else if (ability.id == waitID)
                    {
                        // Is our initiativeSpeed still greater than the base value?
                        if (initiativeSpeed > oldInitiativeSpeed)
                        {
                            // Therefore we are still debuffed
                            ability.isDebuffActive = true;

                            // Debuff reduction 1
                            ability.turnsDebuffed = 1;
                            initiativeSpeed -= waitAbility.magnitude * 0.5f;

                            // Debuff reduction 2
                            //initiativeSpeed -= waitAbility.magnitude;

                            Debug.Log("PlayerScript->EndAction: initiativeSpeed = " + initiativeSpeed);
                        }
                        else
                        {
                            // Our initiativeSpeed has gone back down to a more normal value
                            // If the new initiativeSpeed accidentally goes below where we want it
                            // Set it to where we want it just in case
                            initiativeSpeed = oldInitiativeSpeed;
                        }
                    }
                    else if (ability.id == defendID)
                    {
                        navmeshAgent.speed = baseMoveSpeed;
                    }
                    // Currently no way to swap back the enemyCooldown values on the initiativeSwap debuff.
                    // Permanently changes the enemies enemyCooldown values with eachother
                    /*else if (ability.id == initiativeSwapID)
                    {

                    }*/
                }
                // debuff has been active for desired duration.
                // What ability was it, so what do we do next
                else
                {
                    ability.turnsDebuffed++;
                }
            }
        }
        Debug.Log("PlayerScript: initiativeSpeed = " + initiativeSpeed);
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
                    attackAbility = (PlayerAttack)child.GetComponent<Ability>();
                    break;

                case Ability.Type.Defend:
                    defendID = abilityId;
                    defendAbility = child.GetComponent<Ability>();
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
                    slowAbility = (SlowAbility)child.GetComponent<Ability>();
                    break;

                case Ability.Type.Blink:
                    blinkID = abilityId;
                    blinkAbility = (BlinkAbility)child.GetComponent<Ability>();
                    break;

                case Ability.Type.Swap:
                    netherSwapID = abilityId;
                    netherSwapAbility = (NetherSwap)child.GetComponent<Ability>();
                    break;

                case Ability.Type.InitiativeSwap:
                    initiativeSwapID = abilityId;
                    initiativeSwapAbility = (InitiativeSwapAbility)child.GetComponent<Ability>();
                    break;

                case Ability.Type.Wait:
                    waitID = abilityId;
                    waitAbility = child.GetComponent<Ability>();
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
                    Debug.Log("Ability selected!");
                    break;
                }
            }
        }
    }

    bool IsValidNetherSwapTarget(GameObject teleportedObject)
    {
        if (teleportedObject.tag.Contains("Player") || teleportedObject.tag.Contains("Tele"))
        {
            return true;
        }
        Debug.Log("LOL, you tried teleporting something you CAN'T! HAH");
        return false;
    }

    bool IsValidSlowTarget(GameObject slowedObject)
    {
        // Player used to be targettable. However, because of how the enemy and players are built, both are not inheriting from a common class
        // making it unviable to slow down both types of objects initiativeSpeeds.
        // Implementing this would require enemies and players to inherit from a common class containing the base initiativeSpeed variables
        if (slowedObject.tag.Contains("Slow"))
        {
            return true;
        }
        Debug.Log("Target cannot be slowed.");
        return false;
    }

    bool IsValidInitiativeSwapTarget(GameObject initSwappedObject)
    {
        if (initSwappedObject.tag.Contains("Initswappable"))
        {
            return true;
        }
        return false;
    }
}
