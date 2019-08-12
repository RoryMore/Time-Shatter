using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	public AudioSource MainMenuMusic;
	public Animator animator;

	private int levelToLoad;

	public enum Scene
	{
		MAINMENU,
		BATTLESCENE
	}

	public Scene SceneState;

	private void Update()
	{
		switch (SceneState)
		{
			case Scene.MAINMENU:
				{
					MainMenuMusic.volume = Mathf.Lerp(MainMenuMusic.volume, 0.7f, Time.deltaTime / 0.3f);
					break;
				}
			case Scene.BATTLESCENE:
				{
					break;
				}
		}
	}

	public void PlayGame()
    {
		FadeToLevel(1);
		Debug.Log("PlayClicked");
        //SceneManager.LoadScene("MainScene");
    }

	public void FadeToLevel(int levelIndex)
	{
		levelToLoad = levelIndex;
		animator.SetTrigger("FadeOut");
		MainMenuMusic.pitch = Mathf.Lerp(MainMenuMusic.pitch, 0.8f, Time.deltaTime / 0.05f);
		MainMenuMusic.volume = Mathf.Lerp(MainMenuMusic.volume, 0.0f, Time.deltaTime / 0.0005f);
	}

	public void OnFadeComplete()
	{
		SceneManager.LoadScene("MainScene");
		SceneState = Scene.BATTLESCENE;
	}

}
