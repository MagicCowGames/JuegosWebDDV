using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NOTE : This could be used to replace the camera manager's move code in the future, but whatever, it works, so no need to fuck shit up.

// NOTE : Maybe it should be renamed to "MoveToController" or something like that? or "FollowTargetController"?
// "FollowController" is too generic and could potentially get confused with some NPC AI related component when it is not.

// NOTE : Overshooting lerping animations could be implemented if we lerped using as self position the same value despite changes in current position, only updating it
// if the target position is modified.
// This can be very easily implemented but would require thinking about whether we should rework this component's logic or make a completely different component for that.
// It would be easier to implement / make more sense to implement it as a MoveTo() function that lerps over time. Maybe. Not sure.

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
            this.selfTransform.position = Vector3.Lerp(this.selfTransform.position, this.targetTransform.position, Mathf.Min(1.0f, delta * this.speed));
        else
            this.selfTransform.position = this.targetTransform.position;
    }

    #endregion
}
