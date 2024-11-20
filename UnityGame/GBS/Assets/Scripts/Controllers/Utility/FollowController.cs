using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NOTE : This could be used to replace the camera manager's move code in the future, but whatever, it works, so no need to fuck shit up.
public class FollowController : MonoBehaviour
{
    #region Variables

    [SerializeField] private Transform selfTransform;
    [SerializeField] private Transform targetTransform;
    [SerializeField] private float speed = 10.0f;
    [SerializeField] bool lerp = true;

    #endregion

    #region Properties

    public Transform SelfTrasnform { get { return this.selfTransform; } set { this.selfTransform = value; } }
    public Transform TargetTransform { get { return this.targetTransform; } set { this.targetTransform = value; } }
    public float Speed { get { return this.speed; } set { this.speed = value; } }
    public bool Lerp { get { return this.lerp; } set { this.lerp = value; } }

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

    // NOTE : The main use of having setters when we already have properties is being able to wire these up in a scene to use with triggers.
    // This allows us to make custom events with moveable objects without having to actually build yet another new component that does the movement logic
    // all over again when we already have existing components that can do that behaviour just fine.

    public void SetSelfTransform(Transform transform)
    {
        this.selfTransform = transform;
    }

    public void SetTargetTransform(Transform transform)
    {
        this.targetTransform = transform;
    }

    public Transform GetSelfTransform()
    {
        return this.selfTransform;
    }

    public Transform GetTargetTransform()
    {
        return this.targetTransform;
    }

    #endregion

    #region PrivateMethods

    private void UpdateTransform(float delta)
    {
        if (this.selfTransform == null || this.targetTransform == null)
            return;
        
        if(this.lerp)
            this.selfTransform.position = Vector3.Lerp(this.selfTransform.position, this.targetTransform.position, Mathf.Max(1.0f, delta * this.speed));
        else
            this.selfTransform.position = this.targetTransform.position;
    }

    #endregion
}
