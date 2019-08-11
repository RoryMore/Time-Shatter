using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPlayerUiVisability : MonoBehaviour
{

    public GameObject playerUi;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        playerUi.SetActive(false);
    }
}
