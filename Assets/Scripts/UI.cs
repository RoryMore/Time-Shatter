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
    // Start is called before the first frame update

    private void Awake()
    {
        player = FindObjectOfType<PlayerScript>();
        turnManager = FindObjectOfType<turnManageScript>();
    }

    void Start()
    {
        Healthbar.value = CalculateHealth();
        Inititivebar.value = CalculateInititive();
    }

    // Update is called once per frame
    void Update()
    {
        Healthbar.value = CalculateHealth();
        Inititivebar.value = CalculateInititive();

        if (turnManager.turnCounter == player.initiativeSpeed)
        {

        }
    }

    float CalculateInititive()
    {
        return turnManager.turnCounter / player.initiativeSpeed;

        //player.timeSpentDoingAction 
    }

    float CalculateHealth()
    {
        return player.currentHealth / player.maxHealth;
    }
}
