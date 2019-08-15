using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IsDeadGameReset : MonoBehaviour
{

	PlayerScript player;
	turnManageScript turnManager;
	EnemyManager enemy;
	float deathCount = 0;
	float winCount = 0;
	public bool start;
	public GameObject win;
	bool youwin;
	public GameObject gameui;

	public Animator animator;

	// Start is called before the first frame update
	private void Awake()
	{
		player = FindObjectOfType<PlayerScript>();
		
		turnManager = FindObjectOfType<turnManageScript>();
		enemy = FindObjectOfType<EnemyManager>();

	}

	void Start()
    {
	
    }

    // Update is called once per frame
    void Update()
    {
		if (player.isDead == true)
		{
		deathCount += Time.unscaledDeltaTime;
		}

		if (deathCount >= 5)
		{
			start = true;
		}

		if (winCount >= 3)
		{
			
			FadeToLevel(0);
			//SceneManager.LoadScene("MainMenu");
		

		
		}

		if (enemy.initiativeList.Count <= 0)
		{
			Time.timeScale = 0.1f;
			win.SetActive(true);
			//Debug.Log("Win");
			youwin = true;
			winCount += Time.unscaledDeltaTime;
			gameui.SetActive(false);

		}
	}

	public void FadeToLevel(int levelIndex)
	{
		//levelToLoad = levelIndex;
		animator.SetTrigger("FadeOut");
	}


	public void OnFadeComplete()
	{
		Time.timeScale = 1f;
		SceneManager.LoadScene("MainMenu");
	
		//SceneState = Scene.BATTLESCENE;
	}

}
