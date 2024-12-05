using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMovementController : MonoBehaviour
{
    #region Variables

    [SerializeField] private CharacterController characterController;
    [SerializeField] private Transform characterTransform;
    [SerializeField] private float walkSpeed = 10;

    #endregion

    #region Variables - Private

    private Vector3 gravityVector;
    private Vector3 movementVector;
    private Vector3 currentVelocity;
    private bool isGrounded; // TODO : Implement is grounded logic. This will be useful in the future for falling animations and stuff. For now, not useful at all...

    private bool canWalk;
    private bool canFall;

    #endregion

    #region Properties

    public Vector3 Gravity { get { return gravityVector; } }
    public Vector3 Velocity { get { return currentVelocity; } }

    public bool Grounded { get { return isGrounded; } }

    public bool CanWalk { get { return canWalk; } set { this.canWalk = value; } }
    public bool CanFall { get { return canFall; } set { this.canFall = value; } }

    public float WalkSpeed { get { return this.walkSpeed; } set { this.walkSpeed = value; } }
    public Vector3 MovementVector { get { return this.movementVector; } set { this.movementVector = value; } }

    #endregion

    #region MonoBehaviour

    void Start()
    {
        Init();
    }

    void Update()
    {
        if (GameUtility.GetPaused())
            return;

        float delta = Time.deltaTime;
        UpdatePosition(delta);
    }

    #endregion

    #region PublicMethods
    #endregion

    #region PrivateMethods

    private void Init()
    {
        this.gravityVector = new Vector3(0.0f, -9.8f, 0.0f);
        this.movementVector = Vector3.zero;
        this.currentVelocity = Vector3.zero;

        this.isGrounded = false;

        this.canWalk = true;
        this.canFall = true;
    }

    private void UpdatePosition(float delta)
    {
        Vector3 oldPosition = this.transform.position;

        UpdatePositionWalkInDirection(delta, this.characterTransform.forward, this.movementVector.x, this.walkSpeed);
        UpdatePositionGravity(delta);

        Vector3 currentPosition = this.transform.position;

        this.currentVelocity = (currentPosition - oldPosition) / delta;
    }

    private void UpdatePositionWalkInDirection(float delta, Vector3 direction, float movementAxisValue, float velocity)
    {
        if (!this.canWalk) // NOTE : This should be disabled while we're casting or when we die, etc... done from the entity's main controller logic.
            return;

        Vector3 movementVector = delta * movementAxisValue * direction * velocity;
        this.characterController.Move(movementVector);
    }

    private void UpdatePositionGravity(float delta)
    {
        Vector3 movementVector = delta * this.gravityVector;
        this.characterController.Move(movementVector); // move by the gravity vector separatedly from the other movement calculation to prevent the other one from cancelling out the gravity vector.
    }

    #endregion
}
