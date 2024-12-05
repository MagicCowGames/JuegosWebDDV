using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NOTE : In the future, we should take a PlayerMovementController or something like a velocity controller or whatever the fuck, that way we could just access the max
// velocity values directly... right now we're going through the PlayerController reference, but we don't need everything about the PlayerController so yeah...
// we're also polluting the PlayerController's code by adding so much extra stuff with new getters and shit.

// TODO : Make player's code use the generic Entity classes in the future... or inherit from it.

public class PlayerAnimationController : MonoBehaviour
{
    #region Variables

    [Header("Components")]
    [SerializeField] private CharacterController controller;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private SpellCasterController spellCasterController;
    [SerializeField] private HealthController healthController;

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
        this.healthController.OnDeath += () => { this.animator?.Play("Base Layer.Death"); };
        // NOTE : Temporarily disabled because otherwise the animation will get fucked if the player dies while they are being healed.
        // this.healthController.OnRevive += () => { this.animator?.Play("Base Layer.Revive"); }; // TODO : Create revive animation and in the animator state machine make a transition to motion once it's done playing the revive anim.
    }

    private void UpdateAnimation(float delta)
    {
        UpdateAnimation_Walk(delta);
        UpdateAnimation_Cast(delta);
    }

    private void UpdateAnimation_Walk(float delta)
    {
        var vec = this.playerController.GetCurrentVelocity(); /* this should be this.controller.velocity, but Unity can't hanlde this shit... */
        var forward = this.playerController.GetMeshTransform().forward;
        var speed = this.playerController.GetWalkSpeed();

        // Update Forward Movement value.
        float newForwardMovementValue = (Vector3.Dot(vec, forward) / speed) * 2.0f; // we map the value to [-1,1], then to [-2,2]. And since the player can't walk backward in this game, then it's kinda as if we mapped it to [0,2]...
        this.forwardMovementValue = AnimationLerpFloat(this.forwardMovementValue, newForwardMovementValue, delta, this.forwardMovementUpdateSpeed);

        // This patch can suck my dick, it's trash and it only exists because Unity's animator introduces NaN when timescale is changed, so pause fucks the universe over for some fucking unknown reason.
        if (float.IsNaN(this.forwardMovementValue))
            this.forwardMovementValue = animator.GetFloat(this.forwardMovementName);

        animator.SetFloat(this.forwardMovementName, this.forwardMovementValue);

        // DebugManager.Instance?.Log($"forward = {this.forwardMovementValue}, velocity = {vec}");
    }

    private void UpdateAnimation_Cast(float delta)
    {
        float newCastingValue = this.spellCasterController.GetIsCasting() ? 1.0f : 0.0f;
        this.castingValue = AnimationLerpFloat(this.castingValue, newCastingValue, delta, this.castingUpdateSpeed);
        animator.SetFloat(this.castingName, this.castingValue);
    }

    // TODO : Implement custom structs to handle each type of parameter, such as a AnimParamFloat or something like that, with its own fields that contain the same 3 values we have as separate variables as of now...
    private float AnimationLerpFloat(float oldValue, float newValue, float delta, float updateSpeed)
    {
        return Mathf.Lerp(oldValue, newValue, delta * updateSpeed);
    }

    #endregion
}
