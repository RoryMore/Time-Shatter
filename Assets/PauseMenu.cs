using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
	public GameObject gameplayUi;
	public GameObject pauseMenu;

	//public AudioSource MainMenuMusic;
	public Animator animator;

	private int levelToLoad;

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.P))
		{
			gameplayUi.SetActive(false);
			pauseMenu.SetActive(true);
			Time.timeScale = 0.0f;	
		}
    }

	public void ClickResume()
	{
		gameplayUi.SetActive(true);
		pauseMenu.SetActive(false);
		Time.timeScale = 1.0f;
	}

	public void MainMenu()
	{
		//FadeToLevel(0);
		Time.timeScale = 1.0f;
		SceneManager.LoadScene("MainMenu");
		//SceneManager.LoadScene("MainScene");
	}

	public void FadeToLevel(int levelIndex)
	{
		levelToLoad = levelIndex;
		animator.SetTrigger("FadeOut");
		//MainMenuMusic.pitch = Mathf.Lerp(MainMenuMusic.pitch, 0.8f, Time.deltaTime / 0.05f);
		//MainMenuMusic.volume = Mathf.Lerp(MainMenuMusic.volume, 0.0f, Time.deltaTime / 0.0005f);
	}

	public void OnFadeComplete()
	{
		SceneManager.LoadScene("MainMenu");
		///SceneState = Scene.BATTLESCENE;
	}
}
