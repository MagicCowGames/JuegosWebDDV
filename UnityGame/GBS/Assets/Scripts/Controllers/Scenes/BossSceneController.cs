using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSceneController : MonoBehaviour
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

    public void MoveCamera(Transform transform)
    {
        CameraManager.Instance.SetActiveTarget(transform);
    }

    public void MoveCameraToPlayer()
    {
        PlayerDataManager.Instance.GetPlayer().MoveCameraToPlayer();
    }

    #endregion

    #region PrivateMethods
    #endregion
}
