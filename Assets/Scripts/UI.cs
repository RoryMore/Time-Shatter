using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public Slider Healthbar;
    public Slider Inititivebar;
	

    PlayerScript player;
    turnManageScript turnManager;

	public GameObject playerInititiveSlider;
	//public GameObject playerActionBar;

    // Start is called before the first frame update

    private void Awake()
    {
        player = FindObjectOfType<PlayerScript>();
        turnManager = FindObjectOfType<turnManageScript>();
    }

    void Start()
    {

        Healthbar.value = CalculateHealth();


    }

    // Update is called once per frame
    void Update()
    {
        Healthbar.value = CalculateHealth();

		if (playerInititiveSlider.activeSelf)
		{	
			Inititivebar.value = CalculateInititive();
		}

	


		

		if (turnManager.Ui == true)
		{
			playerInititiveSlider.SetActive(true);
			//playerActionBar.SetActive(false);
		}
		if (turnManager.Ui == false)
		{	
			playerInititiveSlider.SetActive(false);
			//playerActionBar.SetActive(true);
		}

	}

	float CalculateInititive()
	{
		return turnManager.turnCounter / player.initiativeSpeed;

	}

	

    float CalculateHealth()
    {
        return player.currentHealth / player.maxHealth;
    }
}
