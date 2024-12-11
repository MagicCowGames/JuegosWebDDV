using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayMenuController : MonoBehaviour
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

    public void Button_ReturnToMenu()
    {
        SceneLoadingManager.Instance?.LoadSceneMainMenu();
    }

    public void Button_PlayCampaign()
    {
        // Old map name was "GS_PrisonCell"
        LoadGame("gs_s1_m1_prison_arena");
    }

    public void Button_PlayProcedural()
    {
        LoadGame("MapGenPrefabsScene");
    }

    #endregion

    #region PrivateMethods

    private void LoadGame(string levelName)
    {
        SoundManager.Instance?.StopMusic();
        SceneLoadingManager.Instance?.LoadSceneWithTransition(levelName);
    }

    #endregion
}
