using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerAbilitiyUI : MonoBehaviour
{
    //public Image defendButton;

	Ability defendAbility;
	Ability hasteAbility;
	Ability slowAbility;
	Ability blinkAbility;
	Ability netherSwapAbility;
	Ability initiativeSwapAbility;


	PlayerScript player;

	//Defend
	public Image backgroundWhiteDefend;
	public Image mainIconDefend;

	//Haste
	public Image backgroundHaste;
	public Image iconHaste;

	//Slow
	public Image backgroundSlow;
	public Image iconSlow;

	//Blink
	public Image backgroundBlink;
	public Image iconBlink;

	//NetherSwap
	public Image backgroundNSwap;
	public Image iconNSwap;

	//InitiativeSwap
	public Image backgroundISwap;
	public Image iconISwap;


	bool gotAbility = false;

	public float testCDPercent;

	void Awake()
	{
		player = FindObjectOfType<PlayerScript>();
		
	}
	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (!gotAbility)
		{
			if (player.finishedLoading == true)
			{
				defendAbility = player.GetAbility(player.defendID);
				hasteAbility = player.GetAbility(player.hasteID);
				slowAbility = player.GetAbility(player.slowID);
				blinkAbility = player.GetAbility(player.blinkID);
				netherSwapAbility = player.GetAbility(player.netherSwapID);
				initiativeSwapAbility = player.GetAbility(player.initiativeSwapID);
				gotAbility = true;
			}
		}
		if (gotAbility)
		{
			//Defend
			testCDPercent = CalculateDefendCooldown(defendAbility.turnsBeenOnCooldown, defendAbility.cooldown);
			backgroundWhiteDefend.fillAmount = testCDPercent;
			mainIconDefend.fillAmount = testCDPercent;

			//Haste
			backgroundHaste.fillAmount = CalculateDefendCooldown(hasteAbility.turnsBeenOnCooldown, hasteAbility.cooldown);
			iconHaste.fillAmount = CalculateDefendCooldown(hasteAbility.turnsBeenOnCooldown, hasteAbility.cooldown);

			//Slow
			backgroundSlow.fillAmount = CalculateDefendCooldown(slowAbility.turnsBeenOnCooldown, slowAbility.cooldown);
			iconSlow.fillAmount = CalculateDefendCooldown(slowAbility.turnsBeenOnCooldown, slowAbility.cooldown);

			//Blink
			backgroundBlink.fillAmount = CalculateDefendCooldown(blinkAbility.turnsBeenOnCooldown, blinkAbility.cooldown);
			iconBlink.fillAmount = CalculateDefendCooldown(blinkAbility.turnsBeenOnCooldown, blinkAbility.cooldown);

			//NeatherSwap
			backgroundNSwap.fillAmount = CalculateDefendCooldown(netherSwapAbility.turnsBeenOnCooldown, netherSwapAbility.cooldown);
			iconNSwap.fillAmount = CalculateDefendCooldown(netherSwapAbility.turnsBeenOnCooldown, netherSwapAbility.cooldown);

			//InitiativeSwap
			backgroundISwap.fillAmount = CalculateDefendCooldown(initiativeSwapAbility.turnsBeenOnCooldown, initiativeSwapAbility.cooldown);
			iconISwap.fillAmount = CalculateDefendCooldown(initiativeSwapAbility.turnsBeenOnCooldown, initiativeSwapAbility.cooldown);
		}
	}

	//when white image == 1 is when on max cooldown

	float CalculateDefendCooldown(float turnsCD, float CD)
	{
		return (1.0f - (float)turnsCD / (float)CD);
		//return (1.0f - (float)defendAbility.turnsBeenOnCooldown / (float)defendAbility.cooldown);
	}
		   
}
