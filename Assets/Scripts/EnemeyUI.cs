using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemeyUI : MonoBehaviour
{
    public Slider enemySlider;

    EnemyScript enemyScript;

    // Start is called before the first frame update
    void Start()
    {
       // enemyScript = GetComponent<EnemyScript>();
    }

    // Update is called once per frame
    void Update()
    {
       // enemySlider.value = CalculateInitative();
    }

   //float CalculateInitative()
   // {
       //return enemyScript.initiativeSpeed / enemyScript.enemyCooldown;
   // }
}
