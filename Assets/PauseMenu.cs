using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
	public GameObject gameplayUi;
	public GameObject pauseMenu;
    public GameObject controlMenu;

	//public AudioSource MainMenuMusic;
	public Animator animator;
    SoundManager soundManager;

    bool isPaused = false;

	private int levelToLoad;

    // Start is called before the first frame update

    private void Awake()
    {
        soundManager = GameObject.FindGameObjectWithTag("Music").GetComponent<SoundManager>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPaused)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                gameplayUi.SetActive(false);
                pauseMenu.SetActive(true);
                Time.timeScale = 0.0f;
                isPaused = true;
            }
        }

        if (isPaused)
        {
           // soundManager.state = SoundManager.MusicState.PAUSED;
        }

    }

	public void ClickResume()
	{
		gameplayUi.SetActive(true);
		pauseMenu.SetActive(false);
		Time.timeScale = 1.0f;
        isPaused = false;
       // soundManager.state = SoundManager.MusicState.PAUSED;
    }

    public void backToPause()
    {
        pauseMenu.SetActive(true);
        controlMenu.SetActive(false);
    }

    public void clickControls()
    {
        pauseMenu.SetActive(false);
        controlMenu.SetActive(true);
    }

    public void exit()
    {
        Application.Quit();
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #endif
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
