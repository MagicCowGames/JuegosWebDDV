using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
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
    #endregion

    #region PrivateMethods

    private void UpdateCamera(float delta)
    {
        this.activeCamera.transform.position = Vector3.Lerp(this.activeCamera.transform.position, this.activeTarget.position, Mathf.Min(delta * this.speed, 1.0f));
    }

    #endregion
}
