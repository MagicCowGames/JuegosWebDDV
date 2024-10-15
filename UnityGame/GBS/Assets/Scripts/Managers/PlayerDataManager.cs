using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager : SingletonPersistent<PlayerDataManager>
{
    #region Variables

    // Funny story, you see, we don't need to add any sort of cleanup, whether this is a Singleton or a SingletonPersistent, because Unity
    // already sets the references to null when an object is destroyed, which is one of the things that happens when a scene is unloaded,
    // so a lot of the crappy cleanup code can just be yeeted out the window...
    // That is why we never set these variables to null manually, even tho I feel like we should for correctness sake, and it would be trivial to do so...

    private PlayerController playerController;
    private HealthController healthController;

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

    public void SetPlayerReference(PlayerController reference)
    {
        // Gets the player reference and a reference to all of its relevant components.
        this.playerController = reference;
        this.healthController = reference.GetComponent<HealthController>();
    }

    public PlayerController GetPlayer()
    {
        return this.playerController;
    }

    public HealthController GetPlayerHealth()
    {
        return this.healthController;
    }

    #endregion

    #region PrivateMethods
    #endregion
}
