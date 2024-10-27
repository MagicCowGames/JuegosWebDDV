using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsMenuController : MonoBehaviour
{
    #region Variables

    #endregion

    #region MonoBehaviour

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    #endregion

    #region PublicMethods

    public void ReturnToMenu()
    {
        SceneLoadingManager.Instance?.LoadSceneMainMenu();
    }

    public void EmployeeURL_DRA()
    {
        Application.OpenURL("https://github.com/DanielRodriguezAriza");
    }

    public void CompanyURL_MCG()
    {
        Application.OpenURL("https://magiccowgames.github.io/");
    }

    #endregion

    #region PrivateMethods
    #endregion
}
