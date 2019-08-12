using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowAbility : Ability
{
    [HideInInspector] public EnemyScript targettedEnemy = null;

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
