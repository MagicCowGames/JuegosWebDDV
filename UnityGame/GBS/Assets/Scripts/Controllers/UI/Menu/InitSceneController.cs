using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitSceneController : UIController
{
    #region Variables

    #endregion

    #region MonoBehaviour

    // Start is called before the first frame update
    void Start()
    {
        this.LoadMainMenu();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion

    #region PublicMethods
    #endregion

    #region PrivateMethods

    private void LoadMainMenu()
    {
        SceneManager.LoadScene("MenuScene_MainMenu", LoadSceneMode.Single);
    }

    #endregion
}
