using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtController : MonoBehaviour
{
    

    #region Variables

    [SerializeField] private Transform objectTransform;
    [SerializeField] private Transform targetTransform;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        
    }

    void Update()
    {
        float delta = Time.deltaTime;
        UpdateLookAt(delta);
    }

    #endregion

    #region PublicMethods

    public Transform GetLookAtTarget()
    {
        return this.targetTransform;
    }

    public void SetLookAtTarget(Transform target)
    {
        this.targetTransform = target;
    }

    #endregion

    #region PrivateMethods

    private void UpdateLookAt(float delta)
    {
        // Can't update if any of the transform references are null, so we bail.
        if (this.objectTransform == null || this.targetTransform == null)
            return;

        Vector3 forwardVector = (this.targetTransform.position - this.objectTransform.position).normalized;

        // NOTE : This rotation can be lerped if we want a smoother LookAt impl, but for now I want it to be snapping only, for world-space UI elements and stuff.
        this.objectTransform.rotation = Quaternion.LookRotation(forwardVector, Vector3.up);
    }

    #endregion
}
