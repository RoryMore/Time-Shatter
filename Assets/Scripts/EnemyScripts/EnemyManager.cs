using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject meleeEnemy;
    public GameObject rangedEnemy;
    public GameObject healerEnemy;

    

    public List<GameObject> initiativeList;
    public List<GameObject> healList;

    public List<GameObject> spawnPoints;


    
    // Start is called before the first frame update
    void Start()
    {

        foreach (GameObject location in spawnPoints)
        {
            if (location.name.Contains("MeleeSpawnPoint"))
            {
                Instantiate(meleeEnemy, location.transform);
                
            }
            else if (location.name.Contains("RangedSpawnPoint"))
            {
                Instantiate(rangedEnemy, location.transform);
            }
            else if (location.name.Contains("HealerSpawnPoint"))
            {
                Instantiate(healerEnemy, location.transform);
            }
        }

        initiativeList = new List<GameObject>();
        initiativeList.AddRange(GameObject.FindGameObjectsWithTag("EnemyTeleSlowInitswappable"));
        healList = new List<GameObject>();
    
        
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
