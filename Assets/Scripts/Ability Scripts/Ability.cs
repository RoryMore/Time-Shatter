using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    public enum Type
    {
        WeaponAttack,
        Defend,
        Item,
        Haste,
        Slow,
        Blink,
        Swap,
        InitiativeSwap
    };
    public float actionSpeed;
    public float range;
    public float magnitude;

    public Type type;

    public int id;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
