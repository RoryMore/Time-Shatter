using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turnManageScript : MonoBehaviour
{
    //This may or may not be required. It could simply be for managing the actual 
    //UI bar element instead of facilitating turns, which seems more efficient to do inside of each character

    private float updateFixedUpdateCountPerSecond;
    public float turnCounter = 0;
    public float slowMotionCount;
    private float normalSpeedCount = 1.0f;
	public bool Ui  = true;
	public bool actionUi = false;
   // public float playerTurnCounter;
    public float battleStart = 0;
    bool start = false;

    public enum BattleState
    {
        START,
        BATTLE,
        ACTION,
        END
    }

    public BattleState state;

    PlayerScript player;

    //public SoundManager.MusicState muscType;
    SoundManager soundManager;


    void Awake()
    {
        StartCoroutine(Loop());

        player = FindObjectOfType<PlayerScript>();
        //player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        soundManager = GameObject.FindGameObjectWithTag("Music").GetComponent<SoundManager>();
    }

    private void Start()
    {
        state = BattleState.START;

    }

    void FixedUpdate()
    {
       // fixedUpdateCount += 1;

        

        switch (state)
        {
            case BattleState.START:
                {
                    //Time.timeScale = 0.1f;


                    if (battleStart >= 4.0f)
                    {
                        start = true;
                    }

                    if (start == true)
                    {
                        state = BattleState.BATTLE;
                        Ui = true;
                        actionUi = false;
                    }

					
                    break;
                }
            case BattleState.BATTLE:
                {
                    if (turnCounter >= player.initiativeSpeed)
                    {
                        player.isTakingAction = true;
                        turnCounter = 0;
                        Ui = false;
                        actionUi = true;
                    }
                    
                    if (player.isTakingAction == true)
                    {
                        Time.timeScale = Mathf.Lerp(Time.timeScale, slowMotionCount, Time.deltaTime / 0.01f);
						//Ui = false;
						//actionUi = true;
                        soundManager.state = SoundManager.MusicState.SLOWMOTION;
                        //if (player.isExecutingAbility == true)
                        //{
                            //state = BattleState.AC1TION;
                            //actionUi = true;
                        //}
                    }
                    else if (player.isTakingAction == false)
                    {
                        Time.timeScale = Mathf.Lerp(Time.timeScale, normalSpeedCount, Time.deltaTime / 0.1f);
                        soundManager.state = SoundManager.MusicState.BATTLE;
                        //Debug.Log("TurnManager: timeScale = " + Time.timeScale);
                        if (player.isExecutingAbility)
                        {
                            state = BattleState.ACTION;
                        }
                        if (player.playerWaited)
                        {
                            player.playerWaited = false;
                            Ui = true;
                            actionUi = false;
                        }
                    }

                    break;
                }
            case BattleState.ACTION:
                {
                    Time.timeScale = Mathf.Lerp(Time.timeScale, normalSpeedCount, Time.deltaTime / 0.1f);
                    soundManager.state = SoundManager.MusicState.BATTLE;

                    if (player.isExecutingAbility == false)
                    {
						Ui = true;
						actionUi = false;
                        turnCounter = 0;
                        state = BattleState.BATTLE;

                    }

                    break;
                }
            case BattleState.END:
                {

                    break;
                }
        }

    }

    private void Update()
    {
        if (player.isTakingAction == false)
        {
            if (player.isExecutingAbility == false)
            {
                if (state == BattleState.BATTLE)
                {
                    turnCounter += Time.deltaTime;

                }
            }
        }
    }

    IEnumerator Loop()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
         
            battleStart += 1;

        }

    }

}
