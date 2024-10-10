using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Variables

    [Header("Component References")]
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private Transform shootSocket;

    [Header("Player Settings")]
    [SerializeField] private float forwardSpeed;
    [SerializeField] private float rotationSpeed;

    [Header("Weapon Settings")]
    [SerializeField] private GameObject bulletPrefab;

    private Vector2 movementVector;

    #endregion

    #region MonoBehaviour
    
    void Start()
    {
        
    }

    void Update()
    {
        UpdateInput();
        UpdateMovement(Time.deltaTime);
    }

    #endregion

    #region PublicMethods
    #endregion

    #region PrivateMethods

    private void UpdateInput()
    {
        this.movementVector = Vector2.zero;

        if (Input.GetKeyDown(KeyCode.Escape))
            Debug.Log("show menu lol");

        if (Input.GetKey(KeyCode.W)) // Forward
            this.movementVector.x += 1;

        if (Input.GetKey(KeyCode.S)) // Backward
            this.movementVector.x -= 1;

        if (Input.GetKey(KeyCode.A)) // Rotate right
            this.movementVector.y += 1;

        if (Input.GetKey(KeyCode.D)) // Rotate left
            this.movementVector.y -= 1;

        if (Input.GetKeyDown(KeyCode.Return))
            this.Shoot();
    }

    private void UpdateMovement(float delta)
    {
        // NOTE : forward and up vectors are "swapped" because they come from 3D space, but we're on a 2D top down game, so the global up vector is forward from our
        // perspective, while the global forward is our up.
        var forwardForce = delta * this.forwardSpeed * this.movementVector.x * this.rigidBody.transform.up;
        var rotationForce = delta * this.rotationSpeed * this.movementVector.y * this.rigidBody.transform.forward;

        this.rigidBody.AddForce(forwardForce, ForceMode.Acceleration);
        this.rigidBody.AddTorque(rotationForce, ForceMode.Acceleration);
    }

    private void Shoot()
    {
        ObjectSpawner.Spawn(bulletPrefab, shootSocket);
    }

    #endregion
}
