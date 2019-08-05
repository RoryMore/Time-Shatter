using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turnManageScript : MonoBehaviour
{
    //This may or may not be required. It could simply be for managing the actual 
    //UI bar element instead of facilitating turns, which seems more efficient to do inside of each character


    // Start is called before the first frame update

    //ResetTurn of player and enemey
    //TurnTick
    //thing goes when reach 5
    //get the things speed
    //action at 10
    //bool player action
    //bool enemey actions
    //when player turn pause
    //actions have a speed very slow, slow, Medium, Fast , Very Fast , get from the thing
    // turn timer 
    //Take action
    //each enemey player had a turncoutner or speed at which it turns into an action

    private float fixedUpdateCount = 0;
    private float updateFixedUpdateCountPerSecond;
    private float turnCounter = 0;
    public float slowMotionCount;
    private float normalSpeedCount = 1.0f;
    public float playerTurnCounter;
    //bool playerAction = false;

    PlayerScript player;




    void Awake()
    {
        StartCoroutine(Loop());
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
    }

    void FixedUpdate()
    {
        fixedUpdateCount += 1;

        if (turnCounter == player.initiativeSpeed)
        {
            player.isTakingAction = true;
        }

        if (player.isTakingAction == true)
        {
            Time.timeScale = Mathf.Lerp(Time.timeScale, slowMotionCount, Time.deltaTime / 0.01f);
            turnCounter = 0;
        }
        else if (player.isTakingAction == false)
        {
            Time.timeScale = Mathf.Lerp(Time.timeScale, normalSpeedCount, Time.deltaTime / 0.1f);
           
        }

  

    }

    IEnumerator Loop()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            updateFixedUpdateCountPerSecond = fixedUpdateCount;
            fixedUpdateCount = 0;
            if (player.isTakingAction == false)
            {
                turnCounter += 1;
            }


            Debug.Log(turnCounter);

        }
    }
}
