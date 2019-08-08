using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    GameObject[] initiativeListInitial;
    

    public List<GameObject> initiativeList;
    public List<GameObject> healList;


    
    // Start is called before the first frame update
    void Start()
    {


        initiativeList = new List<GameObject>();
        initiativeList.AddRange(GameObject.FindGameObjectsWithTag("EnemyTeleSlowInitswappable"));
        
    
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnemyGotHurt(GameObject enemy)
    {
        healList.Add(enemy);
    }

    public void EnemyNoLongerNeedHealed(GameObject enemy)
    {
        healList.Remove(enemy);
    }
}
