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
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private SpellCasterController spellCasterController;

    private Vector3 gravityVector;

    #endregion

    #region Variables2

    private float movementForward;
    private Quaternion targetRotation;

    private bool isCasting;

    #endregion

    #region Variables3

    private ElementQueue elementQueue;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        SetPlayerReferences();

        this.movementForward = 0.0f;
        this.gravityVector = new Vector3(0.0f, -9.8f, 0.0f);

        this.elementQueue = new ElementQueue(5);

        RegisterEvents();
    }

    void Update()
    {
        if (GameUtility.GetPaused())
            return;

        float delta = Time.deltaTime;
        UpdatePosition(delta);
        UpdateRotation(delta);
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
        CameraManager.Instance?.SetActiveTarget(this.cameraSocket);
    }

    #endregion


    #region PrivateMethods

    private void Shoot()
    {
        ObjectSpawner.Spawn(bulletPrefab, shootSocket);
    }

    private void UpdatePosition(float delta)
    {
        Vector3 movementVector1 = delta * this.movementForward * this.meshTransform.forward * this.walkSpeed;
        Vector3 movementVector2 = delta * this.gravityVector;
        this.characterController.Move(movementVector1);
        this.characterController.Move(movementVector2); // move by the gravity vector separatedly from the other movement calculation to prevent the other one from cancelling out the gravity vector.
    }

    private void UpdateRotation(float delta)
    {
        if (this.isCasting)
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
    }

    // Unsubscribe from events
    private void UnregisterEvents()
    {
        if (InputManager.Instance == null)
            return;

        InputManager.Instance.OnSetForwardAxis -= SetForwardAxis;
        InputManager.Instance.OnSetScreenPoint -= SetLookToPoint;
        InputManager.Instance.OnAddElement -= AddElement;
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
        this.elementQueue.Add(element);
        UIManager.Instance?.GetPlayerUIController().UpdateElementDisplay(this.elementQueue);
    }

    #endregion

    #endregion
}
