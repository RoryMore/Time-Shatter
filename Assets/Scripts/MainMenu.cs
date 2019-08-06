using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	public AudioSource MainMenuMusic;
	public Animator animator;

	private int levelToLoad;

	private void Update()
	{
		MainMenuMusic.volume = Mathf.Lerp(MainMenuMusic.volume, 0.7f, Time.deltaTime / 0.3f);
	}

	public void PlayGame()
    {
		FadeToLevel(1);
        //SceneManager.LoadScene("MainScene");
    }

	public void FadeToLevel(int levelIndex)
	{
		levelToLoad = levelIndex;
		animator.SetTrigger("FadeOut");
		MainMenuMusic.pitch = Mathf.Lerp(MainMenuMusic.pitch, 0.8f, Time.deltaTime / 0.05f);
		MainMenuMusic.volume = Mathf.Lerp(MainMenuMusic.volume, 0.1f, Time.deltaTime / 0.3f);
	}

	public void OnFadeComplete()
	{
		SceneManager.LoadScene("MainScene");
	}

}
