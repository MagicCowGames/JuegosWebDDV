using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO : Implement a speed value for smoothly lerping...
// NOTE : This could be used to replace the camera manager's move code in the future, but whatever, it works, so no need to fuck shit up.
public class FollowController : MonoBehaviour
{
    #region Variables

    [SerializeField] private Transform selfTransform;
    [SerializeField] private Transform targetTransform;

    public Transform SelfTrasnform { get { return this.selfTransform; } set { this.selfTransform = value; } }
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
        if (this.selfTransform == null || this.targetTransform == null)
            return;

        this.selfTransform.position = this.targetTransform.position; // NOTE : Use pos lerping here in the future lol
    }

    #endregion
}
