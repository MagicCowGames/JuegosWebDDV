using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Variables

    [Header("Component References")]
    [SerializeField] private CharacterController characterController;
    // [SerializeField] private Rigidbody rigidBody;

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

    private Vector3 walkVector;
    private Vector3 gravityVector;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        this.walkVector = Vector3.zero;
        this.gravityVector = new Vector3(0.0f, -9.8f, 0.0f);
    }

    void Update()
    {
        UpdateInput();
    }

    void FixedUpdate()
    {
        float delta = Time.fixedDeltaTime;
        UpdatePosition(delta);
        UpdateRotation(delta);
    }

    #endregion

    #region PublicMethods
    #endregion

    #region PrivateMethods

    private void UpdateInput()
    {
        this.walkVector = Vector3.zero;

        if (Input.GetKeyDown(KeyCode.Escape))
            Debug.Log("show menu lol");

        if (Input.GetKeyDown(KeyCode.Return))
            this.Shoot();
    }

    private void Shoot()
    {
        ObjectSpawner.Spawn(bulletPrefab, shootSocket);
    }

    private void UpdatePosition(float delta)
    {
        Vector3 movementVector1 = delta * this.walkVector * this.walkSpeed;
        Vector3 movementVector2 = delta * this.gravityVector;
        this.characterController.Move(movementVector1);
        this.characterController.Move(movementVector2); // move by the gravity vector separatedly from the other movement calculation to prevent the other one from cancelling out the gravity vector.
    }

    private void UpdateRotation(float delta)
    {
        // this.rigidBody.transform.rotation = Quaternion.LookRotation(-vec, Vector3.up);
    }

    #endregion
}
