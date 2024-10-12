using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>, IManager
{
    #region Variables

    [SerializeField] private Camera activeCamera;
    [SerializeField] private Transform activeTarget;
    [SerializeField] private float speed;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        UpdateCamera(Time.deltaTime);
    }

    #endregion

    #region PublicMethods

    public void SetActiveTarget(Transform target)
    {
        this.activeTarget = target;
    }

    public void SetActiveCamera(Camera camera)
    {
        this.activeCamera = camera;
    }

    public void ResetManager()
    {
        SetActiveTarget(null);
        SetActiveCamera(null);
    }

    #endregion

    #region PrivateMethods

    private void UpdateCamera(float delta)
    {
        if (this.activeCamera == null || this.activeTarget == null)
            return;
        UpdateCameraPosition(delta);
        UpdateCameraRotation(delta);
    }

    private void UpdateCameraPosition(float delta)
    {
        this.activeCamera.transform.position = Vector3.Lerp(this.activeCamera.transform.position, this.activeTarget.position, Mathf.Min(delta * this.speed, 1.0f));
    }

    private void UpdateCameraRotation(float delta)
    {
        this.activeCamera.transform.rotation = Quaternion.Lerp(this.activeCamera.transform.rotation, this.activeTarget.rotation, Mathf.Min(delta * this.speed, 1.0f));
    }

    #endregion
}
