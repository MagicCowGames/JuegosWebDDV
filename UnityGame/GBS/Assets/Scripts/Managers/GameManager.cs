using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    #region Variables

    [SerializeField] private string musicName;

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject cameraPrefab;

    /*
    public int Score { get; set; }
    public int Money { get; set; }
    */

    #endregion

    #region MonoBehaviour

    void Start()
    {
        SoundManager.Instance?.PlayMusic(this.musicName);
    }

    void Update()
    {
        
    }

    #endregion

    #region PublicMethods

    // TODO : You gotta Polish that Finish screen! damn these jokes suck... just fix this shit lol, just make it look good...
    public void FinishGameDemo()
    {
        // Make the player immortal so that they cannot die during this screen.
        PlayerDataManager.Instance.GetPlayerHealth().HealthMin = 1.0f; // Basically, make the player immortal so that they can't die during the victory screen.
        
        // Change UI visibility.
        UIManager.Instance.GetFinishUIController().UI_SetVisible(true); // Display the finish UI
        UIManager.Instance.GetPlayerUIController().UI_SetVisible(false); // Hide the player's UI so taht we don't see the scores twice (they are also displayed on the finish UI).

        //Update score and money values on the server
        AccountManager.Instance.UpdateScore(PlayerDataManager.Instance.GetPlayerScore().Score);
        AccountManager.Instance.UpdateMoney(PlayerDataManager.Instance.GetPlayerMoney().Money);
    }

    public void FinishGame()
    {
        // Start next level after you enter the portal
        SceneLoadingManager.Instance?.LoadSceneWithTransition("MapGenPrefabsScene");

        //Update score and money values on the server
        AccountManager.Instance.UpdateScore(PlayerDataManager.Instance.GetPlayerScore().Score);
        AccountManager.Instance.UpdateMoney(PlayerDataManager.Instance.GetPlayerMoney().Money);
    }

    public void SpawnPlayer(Vector3 position)
    {
        ObjectSpawner.Spawn(playerPrefab, position);
        ObjectSpawner.Spawn(cameraPrefab, position);
    }

    #endregion

    #region PrivateMethods
    #endregion
}
