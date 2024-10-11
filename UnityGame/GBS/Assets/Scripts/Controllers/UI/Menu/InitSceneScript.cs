using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitSceneScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.LoadScene("MenuScene_MainMenu", LoadSceneMode.Single);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
