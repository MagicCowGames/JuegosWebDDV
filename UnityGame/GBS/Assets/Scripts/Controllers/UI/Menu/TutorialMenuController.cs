using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// NOTE : WARNING!!!! REMEMBER THE CRAPPY PATCH YOU MADE TO LOAD INTO THE FUCKING TUTORIAL SCENE AND FIX THAT IN THE FUTURE.

public class TutorialMenuController : MonoBehaviour
{
    #region Variables

    #endregion

    #region MonoBehaviour

    // CRAPPY PATCH GOES HERE
    void Awake()
    {
        // THIS CRAPPY PATCH LOADS STRAIGHT INTO THE TUTORIAL MAP. LOADS ON AWAKE TO PREVENT VISUALIZING THIS SCENE EVEN FOR 1 FRAME.
        // YES, SHITTY IDEA, BUT I DID THIS BECAUSE I'M OUT OF FUCKING TIME AND I CAN'T REIMPLEMENT THE IN-MENU TUTORIAL.

        SoundManager.Instance?.StopMusic();
        SceneLoadingManager.Instance?.LoadScene("TutorialMap");
    }

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

    #endregion

    #region PrivateMethods
    #endregion
}
