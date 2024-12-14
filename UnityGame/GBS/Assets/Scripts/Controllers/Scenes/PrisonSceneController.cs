using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrisonSceneController : MonoBehaviour
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

    public void LoadNextLevel()
    {
        SceneLoadingManager.Instance.LoadSceneWithTransition("gs_s1_m2_prison_boss");
    }

    #endregion

    #region PrivateMethods
    #endregion
}
