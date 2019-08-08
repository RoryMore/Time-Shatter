using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerActionUI : MonoBehaviour
{
    public Slider ActionBar;

    PlayerScript player;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player.selectedAbility == null)
        {
            ActionBar.value = 0;
           // ActionBar.
        }
    }
}
