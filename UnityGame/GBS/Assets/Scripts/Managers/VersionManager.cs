using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VersionManager : SingletonPersistent<VersionManager>
{
    #region Variables

    [Header("Version Information")]
    [SerializeField] private GameVersion versionData;

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

    public GameVersion GetVersion()
    {
        return this.versionData;
    }

    // This one's pretty dumb ngl lol, but we're keeping it cause it's funny.
    public void SetVersion(GameVersion version)
    {
        this.versionData = version;
    }

    #endregion

    #region PrivateMethods
    #endregion
}
