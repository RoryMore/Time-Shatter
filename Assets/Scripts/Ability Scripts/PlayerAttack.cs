using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : Ability
{
    //public float attackWidth;     // effective width of the ability in which it can damage enemies 
                                    //making a 'hit rectangle' with range and attackWidth

    public enum AttackType
    {
        Forward,
        Cone,
        Radius
    };

    public AttackType attackType;

    // Start is called before the first frame update
    void Start()
    {
        // If there were to be diff equipped weapons
        //GameObject pRef = GameObject.FindGameObjectWithTag("Player");
        //magnitude = pRef.equippedWep.damage;
        type = Type.WeaponAttack;
        attackType = AttackType.Cone;

        actionSpeed = 2.0f;
        range = 5.0f;
        magnitude = 20.0f;

        turnsBeenOnCooldown = cooldown;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
