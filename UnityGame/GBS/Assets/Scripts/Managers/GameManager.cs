using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    #region Variables

    public int Score { get; set; }
    public int Money { get; set; }

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

    // TODO : You gotta Polish that Finish screen! damn these jokes suck... just fix this shit lol, just make it look good...
    public void FinishGame()
    {
        PlayerDataManager.Instance.GetPlayerHealth().HealthMin = 1.0f; // Basically, make the player immortal so that they can't die during the victory screen.
        UIManager.Instance.GetFinishUIController().UI_SetVisible(true); // Display the finish UI
        UIManager.Instance.GetPlayerUIController().UI_SetVisible(false); // Hide the player's UI so taht we don't see the scores twice (they are also displayed on the finish UI).
    }

    #endregion

    #region PrivateMethods
    #endregion
}
