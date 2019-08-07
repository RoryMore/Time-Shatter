using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitiativeSwapAbility : Ability
{
    [HideInInspector] public EnemyScript target1 = null;
    [HideInInspector] public EnemyScript target2 = null;

    // Start is called before the first frame update
    void Start()
    {
        turnsBeenOnCooldown = cooldown;

        turnsBuffed = 0;
        isBuffActive = false;

        turnsDebuffed = 0;
        isDebuffActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
