using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuMusic : MonoBehaviour
{
    public AudioSource MainMenu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         MainMenu.volume = Mathf.Lerp(MainMenu.volume, 0.7f, Time.deltaTime / 0.3f);
    }
}
