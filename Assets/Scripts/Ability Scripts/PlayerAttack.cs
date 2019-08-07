using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : Ability
{
    [Tooltip("The width a straight attack will have, damaging enemies in an area based on width and range")]
    public float attackWidth;     // effective width of the ability in which it can damage enemies 
    //making a 'hit rectangle' with range and attackWidth

    public enum AttackType
    {
        Forward,
        Cone,
        Radius
    };

    public AttackType attackType;

    [Tooltip("The angle that a conal attack will hit, damaging enemies in an area based on angle and range")]
    public float angle;

    ConeRangeIndicator coneRangeIndicator = null;

    // Start is called before the first frame update
    void Start()
    {
        coneRangeIndicator = transform.GetComponent<ConeRangeIndicator>();

        if (coneRangeIndicator == null)
        {
            Debug.LogAssertion("coneRangeIndicator failed to be set");
        }
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

    public void DrawRangeIndicator()
    {
        if (attackType == AttackType.Cone)
        {
            coneRangeIndicator.DrawIndicator(angle, 0.0f, range);
        }
    }
}
