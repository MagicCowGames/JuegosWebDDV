using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    #region Variables

    [SerializeField] private Camera cameraReference;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        RegisterCamera();
    }

    void Update()
    {
        
    }

    #endregion

    #region PublicMethods
    #endregion

    #region PrivateMethods

    private void RegisterCamera()
    {
        CameraManager.Instance?.SetActiveCamera(this.cameraReference);
    }

    #endregion
}
