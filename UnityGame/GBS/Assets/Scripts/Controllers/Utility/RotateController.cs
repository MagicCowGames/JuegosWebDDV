using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateController : MonoBehaviour
{
    #region Variables

    [SerializeField] private Transform objectTransform;
    [SerializeField] private Vector3 rotationAxis;
    [SerializeField] private float rotationSpeed; // Degrees per second.

    #endregion

    #region MonoBehaviour

    void Start()
    {
        
    }

    void Update()
    {
        float delta = Time.deltaTime;
        this.UpdateRotation(delta);
    }

    #endregion

    #region PublicMethods
    #endregion

    #region PrivateMethods

    public void UpdateRotation(float delta)
    {
        this.objectTransform?.Rotate(this.rotationAxis, delta * rotationSpeed);
    }

    #endregion
}
