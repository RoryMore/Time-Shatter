using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public float maxHealth = 100.0f;
    public float currentHealth = 1.0f;

    float baseMoveSpeed;
    float currentMoveSpeed;
    // Current moveSpeed is the navmeshAgent.speed variable

    public float baseInitiativeSpeed = 3.0f;
    // How long it takes for the player to reach their action
    [HideInInspector] public float initiativeSpeed;

    // STRONGLY THINKING ABOUT CREATING A "StatBlock" class that PlayerScript and possibly enemies can Inherit from
    // FOR NOW, STATS AREN'T BEING UTILISED, SO UNNECESSARY FOR PROTOTYPE
    // Modifier to determine how strong physical attacks/abilities are
    //float strength;

    // Name can be changed to whatever. 
    //Purpose would be a to increase/decrease how powerful non-melee/spell/ability attacks would be
    //float spellPower; 

    //public float timeLeftUntilAction = 6.0f;
    public float timeSpentDoingAction = 0.0f;
    public bool isTakingAction = false;
    bool actionSelection = false;
    public bool isExecutingAbility = false;
	public bool finishedLoading = false;

    public bool playerWaited = false;
       
    public float initiativeEntrySpeed = 3.0f;

    Animator anim;
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
    public int rangedBeamID;

    PlayerAttack attackAbility;
    Ability defendAbility;
    NetherSwap netherSwapAbility;
    Ability hasteAbility;
    SlowAbility slowAbility;
    BlinkAbility blinkAbility;
    InitiativeSwapAbility initiativeSwapAbility;
    Ability waitAbility;
    PlayerAttack rangedBeamAbility;
    // -------------------------------------------------------

    public bool isDead;
    bool damaged;
    //For animation
    public bool running = false;

    private NavMeshAgent navmeshAgent;

    turnManageScript turnManager;

    EnemyScript[] enemies;

    OurCameraController cameraController;

    // This is to show max distances you can select targets
    ConeRangeIndicator abilityRangeCircle;

    public AnimationClip meleeAnimClip;
    public AnimationClip rangedAnimClip;
    public AnimationClip castAnimClip;

    public ParticleSystem meleeParticles;

    // The enemies should have their own instance of these types of particles
    public GameObject hoverTargetObject;
    ParticleSystem hoverTargetParticle;
    public GameObject selectTarget1Object;
    ParticleSystem selectTarget1Particle;
    public GameObject selectTarget2Object;
    ParticleSystem selectTarget2Particle;
    

    // Start is called before the first frame update
    void Start()
    {
        navmeshAgent = GetComponent<NavMeshAgent>();

        baseMoveSpeed = navmeshAgent.speed;
        currentMoveSpeed = navmeshAgent.speed;

        turnManager = FindObjectOfType<turnManageScript>();

        cameraController = FindObjectOfType<OurCameraController>();

        anim = GetComponent<Animator>();

        abilityRangeCircle = GetComponent<ConeRangeIndicator>();
        abilityRangeCircle.Init(180.0f);

        currentHealth = maxHealth;

        initiativeSpeed = baseInitiativeSpeed;

        isDead = false; // This is setting the bool isDead to false

        if (hoverTargetObject != null)
        {
            hoverTargetParticle = hoverTargetObject.GetComponent<ParticleSystem>();
        }
        if (selectTarget1Object != null)
        {
            selectTarget1Particle = selectTarget1Object.GetComponent<ParticleSystem>();
        }
        if (selectTarget2Object != null)
        {
            selectTarget2Particle = selectTarget2Object.GetComponent<ParticleSystem>();
        }
    }

    void Awake()
    {
        PopulateAbilitiesList();
        PopulateEnemiesList();
        Debug.Log("Size of Enemies Array: " + enemies.Length);
        
        //playerAudio = GetComponent <AudioSource> ();
        

        //isTakingAction = true;
        //actionSelection = true;
        //SelectAbility(attackID);
        finishedLoading = true;
    }

    private void Update()
    {
        // Make sure we populate our enemy array
        if (enemies.Length == 0)
        {
            PopulateEnemiesList();
            Debug.Log("Size of Enemies Array: " + enemies.Length);
        }

        CheckIsDead();

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
                CheckIsRunning();
                anim.SetBool("isRunning", running);
                anim.SetFloat("doingAction", timeSpentDoingAction);
                
                //TestTakingDamage();
                CheckDamage();
            }
        }
        anim.SetBool("isDead", isDead);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    void CheckIsDead()
    {
        if (isDead)
        {
            return;
        }

        if (currentHealth <= 0.0f)
        {
            isDead = true;
        }
        else
        {
            isDead = false;
        }
    }

    void CheckDamage()
    {
        //Damage related aspect
        //Any visual damage cues here
        anim.SetBool("gotHit", damaged);
        if (damaged)
        {
            damaged = false;
        }
    }

    void CheckIsRunning()
    {
        //Animation stuff
        //if (navmeshAgent.enabled == true)
        //{
            if (navmeshAgent.remainingDistance <= navmeshAgent.stoppingDistance)
            {
                running = false;
            }
            else
            {
                running = true;
            }
        //}
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
        //animator.SetBool("running", running);
        
    }

    public void TakeDamage(float amount)
    {
        if (isDead)
        {
            return;
        }

        damaged = true;

        float damageToTake = amount;

        if (defendAbility.isBuffActive)
        {
            damageToTake = amount - defendAbility.magnitude;
            if (damageToTake < 0.0f)
            {
                damageToTake = 0.0f;
            }
        }

        currentHealth = Mathf.Clamp(currentHealth - damageToTake, 0.0f, maxHealth);
        //currentHealth -= damageToTake;

        //healthSlider.value = currentHealth;

        //playerAudio.Play ();

        if (currentHealth <= 0.0f)
        {
            isDead = true;
        }
    }

    void TestTakingDamage()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(8.0f);
            Debug.Log("OUCH! HP: " + currentHealth);
        }
    }


    void Death()
    {
        //playerAudio.clip = deathClip;
        //playerAudio.Play ();

        // Make the player stop over a short amount of time. Can just make the player stop in their tracks immediately, but smoothing things tend to be nicer
        float finalDestX = Mathf.Lerp(navmeshAgent.destination.x, transform.position.x, Time.unscaledDeltaTime / 0.5f);
        float finalDestY = Mathf.Lerp(navmeshAgent.destination.y, transform.position.y, Time.unscaledDeltaTime / 0.5f);
        float finalDestZ = Mathf.Lerp(navmeshAgent.destination.z, transform.position.z, Time.unscaledDeltaTime / 0.5f);

        navmeshAgent.destination = new Vector3(finalDestX, finalDestY, finalDestZ);
        navmeshAgent.speed = Mathf.Lerp(navmeshAgent.speed, 0.0f, Time.unscaledDeltaTime / 0.5f);

    }

    void Attack()
    {
        if (isTakingAction)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isTakingAction = false;
                isExecutingAbility = true;

                navmeshAgent.speed = 0.0f;
                navmeshAgent.enabled = false;
                Debug.Log("PlayerScript: Attack Action rotation chosen");
                running = false;
            }

            // Rotate player towards point
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //RaycastHit hit;
            
            if (Physics.Raycast(ray, out RaycastHit hit, 200))
            {
                Vector3 dir = (hit.point - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(dir);
                lookRotation.x = transform.rotation.x;
                lookRotation.z = transform.rotation.z;
                
                transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, (Time.fixedDeltaTime / 0.25f));
            }

            // While choosing where to attack, show where we will be attacking
            if (selectedAbility == attackAbility)
            {
                attackAbility.DrawRangeIndicator();
            }
            else if (selectedAbility == rangedBeamAbility)
            {
                rangedBeamAbility.DrawRangeIndicator();
            }
            
            
        }
        else
        {
            //navmeshAgent.enabled = true;
            timeSpentDoingAction += Time.fixedDeltaTime;

            // Set player to attack animate

            // Draw a range indicator based on weapon attack type
            if (selectedAbility == attackAbility)
            {
                float animSpeed = meleeAnimClip.length;
                anim.SetFloat("attackPlaybackMultiplier", (animSpeed / selectedAbility.actionSpeed));
                anim.SetBool("meleeAttack", true);
                attackAbility.DrawCastTimeRangeIndicator(timeSpentDoingAction);
            }
            else if (selectedAbility == rangedBeamAbility)
            {
                float animSpeed = rangedAnimClip.length;
                anim.SetFloat("attackPlaybackMultiplier", (animSpeed / selectedAbility.actionSpeed));
                anim.SetBool("rangedAttack", true);

                rangedBeamAbility.DrawCastTimeRangeIndicator(timeSpentDoingAction);
            }
        }

        if (timeSpentDoingAction >= selectedAbility.actionSpeed)
        {
            //Debug.Log("PlayerScript: Attack action completing");
            // Check if an enemy is standing in front of player
            // Deal damage to them if they got hit
            for (int i = 0; i < enemies.Length; i++)
            {
                if (attackAbility.ShouldEnemyInPositionBeDamaged(enemies[i].transform.position))
                {
                    Debug.Log("DAMAGED ENEMIES HERE.");
                    enemies[i].TakeDamage(Mathf.RoundToInt(attackAbility.magnitude), enemies[i].transform.position);

                    if (selectedAbility == attackAbility)
                    {
                        meleeParticles.Play();
                    }
                    else if (selectedAbility == rangedBeamAbility)
                    {
                        
                    }
                }
            }

            // Stop player attack animation
            anim.SetBool("meleeAttack", false);
            anim.SetBool("rangedAttack", false);
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
            currentMoveSpeed = currentMoveSpeed * 0.4f;
            navmeshAgent.speed = currentMoveSpeed;
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
        navmeshAgent.speed = 0.0f;
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
        navmeshAgent.speed = 0.0f;
        navmeshAgent.enabled = false;

        // Cast time anim
        float animSpeed = castAnimClip.length;
        anim.SetFloat("attackPlaybackMultiplier", (animSpeed / selectedAbility.actionSpeed));
        anim.SetBool("castingAbility", true);

        running = false;

        if (timeSpentDoingAction >= selectedAbility.actionSpeed)
        {
            anim.SetBool("castingAbility", false);

            selectedAbility.isBuffActive = true;

            initiativeSpeed = baseInitiativeSpeed * selectedAbility.magnitude;

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
            // Check if there are valid targets within range
            int targetsInRange = 0;
            for (int i = 0; i < enemies.Length; i++)
            {
                float distanceFromPlayer = Vector3.Distance(enemies[i].transform.position, transform.position);
                if (distanceFromPlayer <= selectedAbility.range)
                {
                    if (IsValidSlowTarget(enemies[i].gameObject))
                    {
                        targetsInRange++;
                    }
                }
            }
            //Debug.Log("Slow targets in range: " + targetsInRange);
            if (targetsInRange <= 0)
            {
                selectedAbility = null;
            }

            if (selectedAbility != null)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, 500))
                {
                    if (IsValidSlowTarget(hit.collider.gameObject))
                    {
                        hoverTargetObject.SetActive(true);

                        hoverTargetObject.transform.position = hit.collider.gameObject.transform.position;

                        var main = hoverTargetParticle.main;
                        // Figure out how to make particles play outside of Time.timeScale

                        if (Input.GetMouseButtonDown(0))
                        {
                    
                    
                        
                            // Player used to be targettable. However, because of how the enemy and players are built, both are not inheriting from a common class
                            // making it unviable to slow down both types of objects initiativeSpeeds.
                            // Implementing this would require enemies and players to inherit from a common class containing the base initiativeSpeed variables

                            // This isn't impossible to do, however it is just a nuisance to implement the effect
                            // of the ability by doing multiple checks here and in the debuff part of the abilities section,
                            // and possible tweaking SlowAbility a little bit

                            // Check if target is in range
                            float distanceFromPlayer = Vector3.Distance(hit.collider.gameObject.transform.position, transform.position);
                            if (distanceFromPlayer <= selectedAbility.range)
                            {
                                slowAbility.targettedEnemy = hit.collider.gameObject.GetComponent<EnemyScript>();
                                isTakingAction = false;
                                isExecutingAbility = true;

                                navmeshAgent.speed = 0.0f;
                                navmeshAgent.enabled = false;
                            }
                        }

                    }
                    else
                    {
                        hoverTargetObject.SetActive(false);
                    }
                }

                abilityRangeCircle.DrawIndicator(180.0f, selectedAbility.range, selectedAbility.range + 0.1f);
            }
        }
        // We have a target
        else
        {
            if (hoverTargetParticle.isPlaying)
            {
                hoverTargetParticle.Stop();
            }

            timeSpentDoingAction += Time.fixedDeltaTime;
            running = false;

            float animSpeed = castAnimClip.length;
            anim.SetFloat("attackPlaybackMultiplier", (animSpeed / selectedAbility.actionSpeed));
            anim.SetBool("castingAbility", true);
        }

        if (timeSpentDoingAction >= selectedAbility.actionSpeed)
        {
            anim.SetBool("castingAbility", false);

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
            abilityRangeCircle.DrawIndicator(180.0f, selectedAbility.range, selectedAbility.range + 0.1f);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(ray, out RaycastHit hit, 400))
                {
                    // Is target in range
                    float distanceFromPlayer = Vector3.Distance(hit.point, transform.position);
                    if (distanceFromPlayer <= selectedAbility.range)
                    {
                        blinkAbility.blinkLocation = hit.point;
                        blinkAbility.blinkLocation.y += 0.5f;

                        isTakingAction = false;
                        isExecutingAbility = true;

                        navmeshAgent.speed = 0.0f;
                        navmeshAgent.enabled = false;
                    }
                }
            }
        }
        else if (isExecutingAbility)
        {
            timeSpentDoingAction += Time.fixedDeltaTime;

            // Animate cast-time
            running = false;

            float animSpeed = castAnimClip.length;
            anim.SetFloat("attackPlaybackMultiplier", (animSpeed / selectedAbility.actionSpeed));
            anim.SetBool("castingAbility", true);
        }
        

        if (timeSpentDoingAction >= selectedAbility.actionSpeed)
        {
            // Stop animate cast-time
            anim.SetBool("castingAbility", false);
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
            // Check if there are valid targets within range
            int targetsInRange = 0;
            for (int i = 0; i < enemies.Length; i++)
            {
                float distanceFromPlayer = Vector3.Distance(enemies[i].transform.position, transform.position);
                if (distanceFromPlayer <= selectedAbility.range)
                {
                    if (IsValidInitiativeSwapTarget(enemies[i].gameObject))
                    {
                        targetsInRange++;
                    }
                }
            }
            if (targetsInRange <= 2)
            {
                selectedAbility = null;
            }

            if (selectedAbility != null)
            {
                abilityRangeCircle.DrawIndicator(180.0f, selectedAbility.range, selectedAbility.range + 0.1f);

                if (Input.GetMouseButtonDown(0))
                {
                    if (Physics.Raycast(ray, out RaycastHit hit, 400))
                    {
                        // Check validity of target
                        if (IsValidInitiativeSwapTarget(hit.collider.gameObject))
                        {
                            // Is target in range
                            float distanceFromPlayer = Vector3.Distance(hit.collider.gameObject.transform.position, transform.position);
                            if (distanceFromPlayer <= selectedAbility.range)
                            {
                                initiativeSwapAbility.target1 = hit.collider.gameObject.GetComponent<EnemyScript>();
                                Debug.Log("PlayerScript: InitiativeSwap Target 1: SET");
                            }
                        }
                    }
                }
            }
        }
        else if (initiativeSwapAbility.target2 == null)
        {
            // Check if there are valid targets within range
            int targetsInRange = 0;
            for (int i = 0; i < enemies.Length; i++)
            {
                float distanceFromPlayer = Vector3.Distance(enemies[i].transform.position, transform.position);
                if (distanceFromPlayer <= selectedAbility.range)
                {
                    if (IsValidInitiativeSwapTarget(enemies[i].gameObject))
                    {
                        if (enemies[i] != initiativeSwapAbility.target1)
                        {
                            targetsInRange++;
                        }
                    }
                }
            }
            if (targetsInRange <= 0)
            {
                selectedAbility = null;
            }

            if (selectedAbility != null)
            {
                abilityRangeCircle.DrawIndicator(180.0f, selectedAbility.range, selectedAbility.range + 0.1f);

                if (Input.GetMouseButtonDown(0))
                {
                    if (Physics.Raycast(ray, out RaycastHit hit, 400))
                    {
                        // Check validity of target
                        if (IsValidInitiativeSwapTarget(hit.collider.gameObject))
                        {
                            // Is target in range
                            float distanceFromPlayer = Vector3.Distance(hit.collider.gameObject.transform.position, transform.position);
                            if (distanceFromPlayer <= selectedAbility.range)
                            {
                                initiativeSwapAbility.target2 = hit.collider.gameObject.GetComponent<EnemyScript>();
                                Debug.Log("PlayerScript: InitiativeSwap Target 2: SET");

                                isTakingAction = false;
                                isExecutingAbility = true;
                                navmeshAgent.speed = 0.0f;
                                navmeshAgent.enabled = false;
                            }
                        }
                    }
                }
            }
        }
        else
        {
            timeSpentDoingAction += Time.fixedDeltaTime;
            running = false;
            float animSpeed = castAnimClip.length;
            anim.SetFloat("attackPlaybackMultiplier", (animSpeed / selectedAbility.actionSpeed));
            anim.SetBool("castingAbility", true);
        }

        if (timeSpentDoingAction >= selectedAbility.actionSpeed)
        {
            anim.SetBool("castingAbility", false);

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
            // Check if there are valid targets within range
            int targetsInRange = 0;
            for (int i = 0; i < enemies.Length; i++)
            {
                float distanceFromPlayer = Vector3.Distance(enemies[i].transform.position, transform.position);
                if (distanceFromPlayer <= selectedAbility.range)
                {
                    if (IsValidNetherSwapTarget(enemies[i].gameObject))
                    {
                        targetsInRange++;
                    }
                }
            }
            if (targetsInRange <= 2)
            {
                selectedAbility = null;
            }

            if (selectedAbility != null)
            {
                abilityRangeCircle.DrawIndicator(180.0f, selectedAbility.range, selectedAbility.range + 0.1f);
                if (Input.GetMouseButtonDown(0))
                {
                    if (Physics.Raycast(ray, out RaycastHit hit, 400))
                    {
                        // Check validity of target
                        if (IsValidNetherSwapTarget(hit.collider.gameObject))
                        {
                            // Is target in range
                            float distanceFromPlayer = Vector3.Distance(hit.collider.gameObject.transform.position, transform.position);
                            if (distanceFromPlayer <= selectedAbility.range)
                            {
                                // Set First target
                                netherSwapAbility.target1 = hit.collider.gameObject.transform;
                                Debug.Log("PlayerScript: NetherSwap Target 1: SET");
                            }
                        }
                    }
                }
            }
        }
        // Do we have a Second target?
        else if (netherSwapAbility.target2 == null)
        {
            // Check if there are valid targets within range
            int targetsInRange = 0;
            for (int i = 0; i < enemies.Length; i++)
            {
                float distanceFromPlayer = Vector3.Distance(enemies[i].transform.position, transform.position);
                if (distanceFromPlayer <= selectedAbility.range)
                {
                    if (IsValidNetherSwapTarget(enemies[i].gameObject))
                    {
                        if (enemies[i].transform != netherSwapAbility.target1)
                        {
                            targetsInRange++;
                        }
                    }
                }
            }
            if (targetsInRange <= 0)
            {
                selectedAbility = null;
            }

            if (selectedAbility != null)
            {
                abilityRangeCircle.DrawIndicator(180.0f, selectedAbility.range, selectedAbility.range + 0.1f);
                if (Input.GetMouseButtonDown(0))
                {
                    if (Physics.Raycast(ray, out RaycastHit hit, 400))
                    {
                        // Check validity of target
                        if (IsValidNetherSwapTarget(hit.collider.gameObject))
                        {
                            // Is target in range
                            float distanceFromPlayer = Vector3.Distance(hit.collider.gameObject.transform.position, transform.position);
                            if (distanceFromPlayer <= selectedAbility.range)
                            {
                                // Set Second target
                                netherSwapAbility.target2 = hit.collider.gameObject.transform;
                                Debug.Log("PlayerScript: NetherSwap Target 2: SET");

                                isTakingAction = false;
                                isExecutingAbility = true;
                                navmeshAgent.speed = 0.0f;
                                navmeshAgent.enabled = false;
                            }
                        }
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
                running = false;
                float animSpeed = castAnimClip.length;
                anim.SetFloat("attackPlaybackMultiplier", (animSpeed / selectedAbility.actionSpeed));
                anim.SetBool("castingAbility", true);

                if (timeSpentDoingAction >= netherSwapAbility.actionSpeed)
                {
                    // Stop cast time animate
                    anim.SetBool("castingAbility", false);

                    Vector3 tempT = netherSwapAbility.target1.position;

                    netherSwapAbility.target1.position = netherSwapAbility.target2.position;
                    netherSwapAbility.target2.position = tempT;

                    // Deal damage to Target1
                    if (netherSwapAbility.target1.tag.Contains("Player"))
                    {
                        TakeDamage(netherSwapAbility.magnitude);
                    }
                    else if (netherSwapAbility.target1.tag.Contains("Enemy"))
                    {
                        netherSwapAbility.target1.GetComponent<EnemyScript>().TakeDamage(Mathf.RoundToInt(netherSwapAbility.magnitude), netherSwapAbility.target1.position);
                    }

                    // Deal damage to Target2
                    if (netherSwapAbility.target2.tag.Contains("Player"))
                    {
                        TakeDamage(netherSwapAbility.magnitude);
                    }
                    else if (netherSwapAbility.target2.tag.Contains("Enemy"))
                    {
                        netherSwapAbility.target2.GetComponent<EnemyScript>().TakeDamage(Mathf.RoundToInt(netherSwapAbility.magnitude), netherSwapAbility.target2.position);
                    }

                    Debug.Log("PlayerScript: Targets NetherSwapped!");

                    EndAction();
                }
            }
        }
    }

    void Wait()
    {
        playerWaited = true;

        timeSpentDoingAction += Time.fixedDeltaTime;

        actionSelection = true;

        isTakingAction = false;
        isExecutingAbility = true;
        navmeshAgent.speed = 0.0f;
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
        else if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("PlayerScript: Attempted to select RangedBeam");
            SelectAbility(rangedBeamID);
        }
    }

    void EndAction()
    {
        isTakingAction = false;
        isExecutingAbility = false;
        actionSelection = false;

        timeSpentDoingAction = 0.0f;
        navmeshAgent.enabled = true;
        navmeshAgent.speed = currentMoveSpeed;

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
                        initiativeSpeed = baseInitiativeSpeed * ((hasteAbility.magnitude * 1.75f) / hasteAbility.magnitude);
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
                        initiativeSpeed = baseInitiativeSpeed;
                    }
                    else if (ability.id == slowID)
                    {

                        slowAbility.targettedEnemy = null;
                    }
                    else if (ability.id == waitID)
                    {
                        // Is our initiativeSpeed still greater than the base value?
                        if (initiativeSpeed > baseInitiativeSpeed)
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
                            initiativeSpeed = baseInitiativeSpeed;
                        }
                    }
                    else if (ability.id == defendID)
                    {
                        currentMoveSpeed = baseMoveSpeed;
                        navmeshAgent.speed = currentMoveSpeed;
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
                    PlayerAttack tempAttack = (PlayerAttack)child.GetComponent<Ability>();
                    switch (tempAttack.attackType)
                    {
                        case PlayerAttack.AttackType.Cone:
                            attackID = abilityId;
                            attackAbility = tempAttack;
                            break;

                        case PlayerAttack.AttackType.Forward:
                            rangedBeamID = abilityId;
                            rangedBeamAbility = tempAttack;
                            break;
                        default:
                            break;
                    }
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

    public Ability GetAbility(int id)
    {
        Ability temp = abilities[0];
        foreach (Ability ability in abilities)
        {
            if (ability.id == id)
            {
                temp = ability;
            }
        }
        return temp;
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

    void PopulateEnemiesList()
    {
        enemies = new EnemyScript[FindObjectsOfType<EnemyScript>().Length];
        EnemyScript[] allEnemies = new EnemyScript[enemies.Length];

        allEnemies = FindObjectsOfType<EnemyScript>();

        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i] = allEnemies[i];
        }
    }
}
