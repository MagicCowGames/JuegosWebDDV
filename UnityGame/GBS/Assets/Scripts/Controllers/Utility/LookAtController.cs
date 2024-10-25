using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtController : MonoBehaviour
{
    #region Enums

    public enum LookAtMode
    {
        FullRotation = 0,
        NormalAligned,
        Absolute
    }

    #endregion

    #region Variables

    [SerializeField] private Transform selfTransform;
    [SerializeField] private Transform targetTransform;
    [SerializeField] private LookAtMode lookAtMode = LookAtMode.NormalAligned;

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

        switch (this.lookAtMode)
        {
            default:
            case LookAtMode.FullRotation:
                UpdateTransformFullRotation(delta);
                break;
            case LookAtMode.NormalAligned:
                UpdateTransformNormalAligned(delta);
                break;
            case LookAtMode.Absolute:
                UpdateTransformAbsolute(delta);
                break;
        }
    }

    // NOTE : These rotations can be lerped if we want a smoother LookAt impl, but for now I want it to be snapping only, for world-space UI elements and stuff.
    
    // NOTE : The vectors look like they are inverted, because we calculate the one that goes from the target to the origin rather than from the origin to the target,
    // but this is actually correct, and it is done this way because the UI elements' forward vector faces away from the camera rather than toward the camera, so
    // calculating vectors that go from the origin to the target would point the forward vector toward the camera and cause the UI elements to be flipped / rotated 180º.

    private void UpdateTransformFullRotation(float delta)
    {
        Vector3 forward = (this.selfTransform.position - this.targetTransform.position).normalized;
        this.selfTransform.rotation = Quaternion.LookRotation(forward);
    }

    private void UpdateTransformNormalAligned(float delta)
    {
        Vector3 forward = this.targetTransform.forward;
        this.selfTransform.rotation = Quaternion.LookRotation(forward);
    }

    private void UpdateTransformAbsolute(float delta)
    {
        this.selfTransform.rotation = Quaternion.LookRotation(this.targetTransform.position);
    }

    #endregion
}
