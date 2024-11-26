using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSceneController : MonoBehaviour
{
    #region Variables
    #endregion

    #region MonoBehaviour

    void Start()
    {
        SoundManager.Instance?.PlayMusic("gameplay1");
        UIManager.Instance?.GetPopUpUIController().Open("loc_tutorial", "loc_tutorial_message_0");
    }

    void Update()
    {
        
    }

    #endregion

    #region PublicMethods

    public void EnterArea2()
    {
        UIManager.Instance?.GetPopUpUIController().Open("loc_tutorial", "loc_tutorial_message_1");
    }

    #endregion

    #region PrivateMethods
    #endregion
}
