using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turnManageScript : MonoBehaviour
{
    //This may or may not be required. It could simply be for managing the actual 
    //UI bar element instead of facilitating turns, which seems more efficient to do inside of each character

    private float fixedUpdateCount = 0;
    private float updateFixedUpdateCountPerSecond;
    private float turnCounter = 0;
    public float slowMotionCount;
    private float normalSpeedCount = 1.0f;
    public float playerTurnCounter;
    //bool playerAction = false;

    PlayerScript player;

	//public SoundManager.MusicState muscType;
	SoundManager soundManager;


    void Awake()
    {
        StartCoroutine(Loop());
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
		soundManager = GameObject.FindGameObjectWithTag("Music").GetComponent<SoundManager>();
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
			soundManager.state = SoundManager.MusicState.SLOWMOTION;
        }
        else if (player.isTakingAction == false)
        {
            Time.timeScale = Mathf.Lerp(Time.timeScale, normalSpeedCount, Time.deltaTime / 0.1f);
			soundManager.state = SoundManager.MusicState.BATTLE;
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
