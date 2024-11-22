using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    #region Variables

    [Header("Components")]
    [SerializeField] private CharacterController controller;

    [Header("Animation Controller")]
    [SerializeField] private Animator animator;

    // NOTE : This shouldn't be exposed tbh... but for now it's ok. I mean, maybe it could make sense for it to be editable in the future, but not really as of now.
    [Header("Animation Values")]
    [SerializeField] private string forwardMovementName = "ForwardMovement";
    [SerializeField] private float forwardMovementValue = 0.0f;
    [SerializeField] private float forwardMovementUpdateSpeed = 2.0f;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        Init();
    }

    void Update()
    {
        float delta = Time.deltaTime;
        UpdateAnimation(delta);
    }

    #endregion

    #region PublicMethods
    #endregion

    #region PrivateMethods

    private void Init()
    {

    }

    private void UpdateAnimation(float delta)
    {
        // Update Forward Movement value.
        float newForwardMovementValue = Vector3.Dot(controller.velocity, controller.transform.forward);
        this.forwardMovementValue = Mathf.Lerp(this.forwardMovementValue, newForwardMovementValue, delta * this.forwardMovementUpdateSpeed);
        animator.SetFloat(this.forwardMovementName, this.forwardMovementValue);
    }

    #endregion
}
