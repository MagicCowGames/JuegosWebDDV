using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NOTE : In the future, we should take a PlayerMovementController or something like a velocity controller or whatever the fuck, that way we could just access the max
// velocity values directly... right now we're going through the PlayerController reference, but we don't need everything about the PlayerController so yeah...
// we're also polluting the PlayerController's code by adding so much extra stuff with new getters and shit.
public class PlayerAnimationController : MonoBehaviour
{
    #region Variables

    [Header("Components")]
    [SerializeField] private CharacterController controller;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private SpellCasterController spellCasterController;

    [Header("Animation Controller")]
    [SerializeField] private Animator animator;

    // NOTE : This shouldn't be exposed tbh... but for now it's ok. I mean, maybe it could make sense for it to be editable in the future, but not really as of now.
    [Header("Animation Values")]
    [SerializeField] private string forwardMovementName = "ForwardMovement";
    [SerializeField] private float forwardMovementValue = 0.0f;
    [SerializeField] private float forwardMovementUpdateSpeed = 2.0f;

    [SerializeField] private string castingName = "Casting";
    [SerializeField] private float castingValue = 0.0f;
    [SerializeField] private float castingUpdateSpeed = 2.0f;

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
        UpdateAnimation_Walk(delta);
        UpdateAnimation_Cast(delta);
    }

    private void UpdateAnimation_Walk(float delta)
    {
        var vec = this.playerController.GetCurrentVelocity();

        // Update Forward Movement value.
        float newForwardMovementValue = (Vector3.Dot(/*this.controller.velocity*/vec, this.playerController.GetMeshTransform().forward) / this.playerController.GetWalkSpeed()) * 2.0f; // we map the value to [-1,1], then to [-2,2]. And since the player can't walk backward in this game, then it's kinda as if we mapped it to [0,2]...
        this.forwardMovementValue = AnimationLerpFloat(this.forwardMovementValue, newForwardMovementValue, delta, this.forwardMovementUpdateSpeed);

        animator.SetFloat(this.forwardMovementName, this.forwardMovementValue);

        // DebugManager.Instance?.Log($"forward = {this.forwardMovementValue}, velocity = {vec}");
    }

    private void UpdateAnimation_Cast(float delta)
    {
        float newCastingValue = this.spellCasterController.GetIsCasting() ? 1.0f : 0.0f;
        this.castingValue = AnimationLerpFloat(this.castingValue, newCastingValue, delta, this.castingUpdateSpeed);
    }

    // TODO : Implement custom structs to handle each type of parameter, such as a AnimParamFloat or something like that, with its own fields that contain the same 3 values we have as separate variables as of now...
    private float AnimationLerpFloat(float oldValue, float newValue, float delta, float updateSpeed)
    {
        return Mathf.Lerp(oldValue, newValue, delta * updateSpeed);
    }

    #endregion
}
