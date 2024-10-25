using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtController : MonoBehaviour
{
    #region Variables

    [SerializeField] private Transform selfTransform;
    [SerializeField] private Transform targetTransform;

    public Transform SelfTransform { get { return this.selfTransform; } set { this.selfTransform = value; } }
    public Transform TargetTransform { get { return this.targetTransform; } set { this.targetTransform = value; } }

    #endregion

    #region MonoBehaviour

    void Start()
    {
        
    }

    void Update()
    {
        float delta = Time.deltaTime;
        UpdateTransform(delta);
    }

    #endregion

    #region PublicMethods
    #endregion

    #region PrivateMethods

    private void UpdateTransform(float delta)
    {
        // Can't update if any of the transform references are null, so we bail.
        if (this.SelfTransform == null || this.TargetTransform == null)
            return;

        Vector3 forwardVector = (this.TargetTransform.position - this.SelfTransform.position).normalized;

        // NOTE : This rotation can be lerped if we want a smoother LookAt impl, but for now I want it to be snapping only, for world-space UI elements and stuff.
        this.SelfTransform.rotation = Quaternion.LookRotation(forwardVector, Vector3.up);
    }

    #endregion
}
