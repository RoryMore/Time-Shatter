﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public Image Healthbar;
	//public Image Healthbar2;
	public Slider Inititivebar;
   // public Button testbutton;
    PlayerScript player;
    turnManageScript turnManager;

	public GameObject playerInititiveSlider;
	public GameObject BattleUI;
	public GameObject Lose;

	bool doOnce = false;
	//public GameObject playerActionBar;

    // Start is called before the first frame update

    private void Awake()
    {
        player = FindObjectOfType<PlayerScript>();
        turnManager = FindObjectOfType<turnManageScript>();
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
		if (turnManager.state == turnManageScript.BattleState.BATTLE && !doOnce)
		{
			BattleUI.SetActive(true);
			doOnce = true;
		}

		Healthbar.fillAmount = CalculateHealth();
		//	Healthbar2.value = CalculateHealth();

			if (playerInititiveSlider.activeSelf)
			{
				Inititivebar.value = CalculateInititive();
			}

			// testbutton.interactable = true;

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
		//}
		//else {
		//BattleUI.SetActive(false);
		//}

		if (player.isDead == true)
		{
			BattleUI.SetActive(false);
			Lose.SetActive(true);
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
