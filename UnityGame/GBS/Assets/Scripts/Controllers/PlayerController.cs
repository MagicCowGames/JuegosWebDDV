using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Variables

    [Header("Components")]
    [SerializeField] private CharacterController characterController;

    [Header("Transform")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform meshTransform;

    [Header("Sockets")]
    [SerializeField] private Transform cameraSocket;
    [SerializeField] private Transform shootSocket;

    [Header("Player Settings")]
    [SerializeField] private float walkSpeed; // walk speed
    [SerializeField] private float rotationSpeed; // speed at which the rotation takes place when using beams. Otherwise, the rotation vector is set instantaneously.

    [Header("Weapon Settings")]
    [SerializeField] private SpellCasterController spellCasterController;

    private Vector3 gravityVector;

    #endregion

    #region Variables2

    private float movementForward;
    private Quaternion targetRotation;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        this.movementForward = 0.0f;
        this.gravityVector = new Vector3(0.0f, -9.8f, 0.0f);

        SetPlayerReferences(); // We set it at the end to make sure that everything has been constructed first.
        RegisterEvents();
    }

    void Update()
    {
        if (GameUtility.GetPaused())
            return;

        float delta = Time.deltaTime;
        UpdatePosition(delta);
        UpdateRotation(delta);

        // DebugManager.Instance?.DrawLine(Color.red, this.playerTransform.position, this.meshTransform.position + this.meshTransform.forward * 100);
        DebugManager.Instance?.DrawSphere(this.playerTransform.position, 2, Color.blue);
    }

    void OnDestroy()
    {
        UnregisterEvents();
    }

    #endregion

    #region PublicMethods - Getters and Setters

    public Transform GetPlayerTransform()
    {
        return this.playerTransform;
    }

    public Transform GetMeshTransform()
    {
        return this.meshTransform;
    }

    #endregion

    #region Private Methods

    private void SetPlayerReferences()
    {
        PlayerDataManager.Instance?.SetPlayerReference(this);
        CameraManager.Instance?.SetActiveTarget(this.cameraSocket);
    }

    #endregion


    #region PrivateMethods

    private void UpdatePosition(float delta)
    {
        UpdatePositionWalk(delta);
        UpdatePositionGravity(delta);
    }

    private void UpdatePositionWalk(float delta)
    {
        // Can't walk while we're casting!
        if (this.spellCasterController.GetIsCasting())
            return;
        Vector3 movementVector = delta * this.movementForward * this.meshTransform.forward * this.walkSpeed;
        this.characterController.Move(movementVector);
    }

    private void UpdatePositionGravity(float delta)
    {
        Vector3 movementVector = delta * this.gravityVector;
        this.characterController.Move(movementVector); // move by the gravity vector separatedly from the other movement calculation to prevent the other one from cancelling out the gravity vector.
    }

    private void UpdateRotation(float delta)
    {
        // Rotate slower while we're casting to prevent beams from being insanely OP sniper lasers and to give them a little bit of extra eye-candy and a more weighted feel.
        if (this.spellCasterController.GetIsCasting())
            this.meshTransform.rotation = Quaternion.Lerp(this.meshTransform.rotation, targetRotation, delta * this.rotationSpeed);
        else
            this.meshTransform.rotation = targetRotation;
    }

    #endregion

    #region Private Methods

    #region Register Events

    // Subscribe to events
    private void RegisterEvents()
    {
        if (InputManager.Instance == null)
            return;

        InputManager.Instance.OnSetForwardAxis += SetForwardAxis;
        InputManager.Instance.OnSetScreenPoint += SetLookToPoint;
        InputManager.Instance.OnAddElement += AddElement;
        InputManager.Instance.OnRightClickDown += RightClickDown;
        InputManager.Instance.OnRightClickUp += RightClickUp;
        InputManager.Instance.OnSetForm += SetForm;
    }

    // Unsubscribe from events
    private void UnregisterEvents()
    {
        if (InputManager.Instance == null)
            return;

        InputManager.Instance.OnSetForwardAxis -= SetForwardAxis;
        InputManager.Instance.OnSetScreenPoint -= SetLookToPoint;
        InputManager.Instance.OnAddElement -= AddElement;
        InputManager.Instance.OnRightClickDown -= RightClickDown;
        InputManager.Instance.OnRightClickUp -= RightClickUp;
        InputManager.Instance.OnSetForm -= SetForm;
    }

    #endregion

    #region Input Events and Actions

    private void SetForwardAxis(float value)
    {
        this.movementForward = value;
    }

    private void SetLookToPoint(Vector3 inputScreenLocation)
    {
        if (CameraManager.Instance == null || CameraManager.Instance.GetActiveCamera() == null)
            return;

        var screenPosPlayer = CameraManager.Instance.GetActiveCamera().WorldToViewportPoint(playerTransform.position);
        var screenPosMouse = CameraManager.Instance.GetActiveCamera().ScreenToViewportPoint(inputScreenLocation);

        Vector3 vec = screenPosMouse - screenPosPlayer;
        Vector3 direction = vec.normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90.0f;

        this.targetRotation = Quaternion.Euler(new Vector3(0, -angle, 0));
    }

    private void AddElement(Element element)
    {
        this.spellCasterController.AddElement(element);
    }

    private void RightClickDown()
    {
        this.spellCasterController.Cast();
    }

    private void RightClickUp()
    {
        this.spellCasterController.StopCast();
    }

    private void SetForm(Form form)
    {
        this.spellCasterController.SetForm(form);
    }

    #endregion

    #endregion
}
