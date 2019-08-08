using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIPLayerAction : MonoBehaviour
{

	public Slider ActionBar;
	PlayerScript player;
	turnManageScript turnManager;

	public GameObject playerActionBar;

	private float precentage;
	


	// Start is called before the first frame update
	void Start()
    {
		player = FindObjectOfType<PlayerScript>();
		turnManager = FindObjectOfType<turnManageScript>();
	}

    // Update is called once per frame
    void Update()
    {

		if (turnManager.actionUi == true)
		{
			playerActionBar.SetActive(true);
		}
		if (turnManager.actionUi == false)
		{
			playerActionBar.SetActive(false);
		}
		//playerInititiveSlider.SetActive(true);
		
	
	
		
			//playerInititiveSlider.SetActive(false);
			
		
		ActionBar.value = CalculateActionBar();
	}

	//float CalculatePacent()
	//{
	//	precentage = player.selectedAbility.actionSpeed * 90.0f;

	//	precentage = precentage / 100;

	//	return precentage;
	//}

	float CalculateActionBar()
	{
		try
		{
			return player.timeSpentDoingAction / player.selectedAbility.actionSpeed;
		}
		catch (NullReferenceException)
		{
			return 0.0f;
		}
	}



}
