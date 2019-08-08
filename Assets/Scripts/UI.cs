using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public Slider Healthbar;

    PlayerScript player;
    // Start is called before the first frame update

    private void Awake()
    {
        player = FindObjectOfType<PlayerScript>();
    }

    void Start()
    {
        Healthbar.value = CalculateHealth();
        Debug.Log("YAY");
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    float CalculateHealth()
    {
        return player.currentHealth / player.maxHealth;
    }
}
