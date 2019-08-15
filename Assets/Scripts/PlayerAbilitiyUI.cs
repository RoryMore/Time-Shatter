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
    turnManageScript turnManager;


    //attack
    public Button attackButton;
    public Button waitButton;

	//Defend
	public Image backgroundWhiteDefend;
	public Image mainIconDefend;
    public Button defendButton;
    //bool defendCD;

	//Haste
	public Image backgroundHaste;
	public Image iconHaste;
    public Button hasteButton;
   // bool hasteCD;

    //Slow
    public Image backgroundSlow;
	public Image iconSlow;
    public Button slowButton;
    //bool slowCD;

    //Blink
    public Image backgroundBlink;
	public Image iconBlink;
    public Button blinkButton;
    //  bool blinkCD;

    //NetherSwap
    public Image backgroundNSwap;
	public Image iconNSwap;
    public Button netherButton;
   // bool netherCD;

    //InitiativeSwap
    public Image backgroundISwap;
	public Image iconISwap;
    public Button initiativeButton;
	// bool initiativeCD;

	public Image backgroundRange;
	public Image rangeIcon;
	public Button rangeButton;

	bool gotAbility = false;

	public float testCDPercent;

	void Awake()
	{
		player = FindObjectOfType<PlayerScript>();
        turnManager = FindObjectOfType<turnManageScript>();

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

			backgroundRange.fillAmount = CalculateDefendCooldown(initiativeSwapAbility.turnsBeenOnCooldown, initiativeSwapAbility.cooldown);
			rangeIcon.fillAmount = CalculateDefendCooldown(initiativeSwapAbility.turnsBeenOnCooldown, initiativeSwapAbility.cooldown);


			//Defend Button Check
			if ((player.isTakingAction))
            {
                if (CheckIfOnCooldown(defendAbility.turnsBeenOnCooldown, defendAbility.cooldown) == true)
                {
                    defendButton.gameObject.SetActive(false);
                }
                else
                {
                    defendButton.gameObject.SetActive(true);
                }

                //haste
                if (CheckIfOnCooldown(hasteAbility.turnsBeenOnCooldown, hasteAbility.cooldown) == true)
                {
                    hasteButton.gameObject.SetActive(false);
                }
                else
                {
                    hasteButton.gameObject.SetActive(true);
                }

                //slow
                if (CheckIfOnCooldown(slowAbility.turnsBeenOnCooldown, slowAbility.cooldown) == true)
                {
                    slowButton.gameObject.SetActive(false);
                }
                else
                {
                    slowButton.gameObject.SetActive(true);
                }

                //Blink
                if (CheckIfOnCooldown(blinkAbility.turnsBeenOnCooldown, blinkAbility.cooldown) == true)
                {
                    blinkButton.gameObject.SetActive(false);
                }
                else
                {
                    blinkButton.gameObject.SetActive(true);
                }

                //NetherSwap
                if (CheckIfOnCooldown(netherSwapAbility.turnsBeenOnCooldown, netherSwapAbility.cooldown) == true)
                {
                    netherButton.gameObject.SetActive(false);
                }
                else
                {
                    netherButton.gameObject.SetActive(true);
                }

                //InitiativeSwap
                if (CheckIfOnCooldown(initiativeSwapAbility.turnsBeenOnCooldown, initiativeSwapAbility.cooldown) == true)
                {
                    initiativeButton.gameObject.SetActive(false);
                }
                else
                {
                    initiativeButton.gameObject.SetActive(true);
                }

				if (CheckIfOnCooldown(initiativeSwapAbility.turnsBeenOnCooldown, initiativeSwapAbility.cooldown) == true)
				{
					rangeButton.gameObject.SetActive(false);
				}
				else
				{
					rangeButton.gameObject.SetActive(true);
				}

				attackButton.gameObject.SetActive(true);
                waitButton.gameObject.SetActive(true);
            }
            else if (!player.isTakingAction)
            {
                defendButton.gameObject.SetActive(false);
                hasteButton.gameObject.SetActive(false);
                slowButton.gameObject.SetActive(false);
                blinkButton.gameObject.SetActive(false);
                netherButton.gameObject.SetActive(false);
                initiativeButton.gameObject.SetActive(false);
                attackButton.gameObject.SetActive(false);
                waitButton.gameObject.SetActive(false);
				rangeButton.gameObject.SetActive(false);

			}

        }

	}

    public void defendClick()
    {
        if (player.selectedAbility == null)
        {
            player.SelectAbility(player.defendID);
        }

    }

    public void hasteClick()
    {
        if (player.selectedAbility == null)
        {
            player.SelectAbility(player.hasteID);
        }
    }

    public void slowClick()
    {
        if (player.selectedAbility == null)
        {
            player.SelectAbility(player.slowID);
        }
    }

    public void blinkClick()
    {
        if (player.selectedAbility == null)
        {
            player.SelectAbility(player.blinkID);
        }
    }

    public void netherClick()
    {
        if (player.selectedAbility == null)
        {
            player.SelectAbility(player.netherSwapID);
        }
    }

    public void IntSwapClick()
    {
        if (player.selectedAbility == null)
        {
            player.SelectAbility(player.initiativeSwapID);
        }
    }

    public void attackClick()
    {
        if (player.selectedAbility == null)
        {
            player.SelectAbility(player.attackID);
        }
    }

    public void waitClick()
    {
        if (player.selectedAbility == null)
        {
            player.SelectAbility(player.waitID);
        }
    }

	public void waitrange()
	{
		if (player.selectedAbility == null)
		{
			player.SelectAbility(player.rangedBeamID);
		}
	}

	//when white image == 1 is when on max cooldown

	float CalculateDefendCooldown(float turnsCD, float CD)
	{
		return (1.0f - (float)turnsCD / (float)CD);
		//return (1.0f - (float)defendAbility.turnsBeenOnCooldown / (float)defendAbility.cooldown);
	}

    bool CheckIfOnCooldown(float turnsCD, float CD)
    {
        if (turnsCD == CD)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
		   
}
