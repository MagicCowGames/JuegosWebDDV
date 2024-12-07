using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO : Clean a lot of shit up in this class...
public class PlayerController : MonoBehaviour
{
    #region Variables

    [Header("Components")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private HealthController healthController;

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

    [Header("Sounds")]
    [SerializeField] private AudioClip deathSound; // TODO : Again, add this to some sound manager or some shit...

    private Vector3 gravityVector;

    private Vector3 currentVelocity = Vector3.zero;

    #endregion

    #region Variables2

    private float movementForward;
    private Quaternion targetRotation;

    #endregion

    #region Variables3

    public bool CanActivateTriggers { get; set; } = true; // TODO : Make this a generic EntityController property rather than player specific...

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

    public float GetWalkSpeed()
    {
        return this.walkSpeed;
    }

    public Vector3 GetCurrentVelocity()
    {
        return this.currentVelocity;
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
        Vector3 oldPosition = this.transform.position;

        UpdatePositionWalk(delta);
        UpdatePositionGravity(delta);

        Vector3 currentPosition = this.transform.position;

        this.currentVelocity = (currentPosition - oldPosition) / delta; // This is a shitty patch that really needs to be moved to a different controller...
        // NOTE : The reason this crappy patch exists is because the PlayerAnimationController cannot access characterController.velocity AT ALL!
        // This happens because the velocity is calculated after all of the Move() and SimpleMove() calls take place... which means that at frame start, it is reset to
        // <0,0,0>, so that means we need to either manually calculate it or save it so that it can be used by other classes...
        // Oh btw, saving the value of characterController.velocity AFTER doing all of the Move() calls DOES NOT WORK EITHER! because the value is LOST if it is not
        // fetched immediately AFTER any of the Move() calls. Oh btw (x2), this behaviour is not consistent! if it takes place within the same function call, then it's
        // all fine and dandy... otherwise? you're fucked! why? WHO KNOWS! The solution to decouple this shit is to fuck Unity and, as always, do the work by hand
        // ourselves... As always, the documentation says X, but the reality is Y, plus a few sprinkles of shit.
        // TODO : Move all of the movement logic to a PlayerMovementController... ffs...
        // DebugManager.Instance?.Log($"playervelocity = {this.currentVelocity}");
    }

    private void UpdatePositionWalk(float delta)
    {
        // Can't walk if we're dead!
        if (this.healthController.IsDead())
            return;
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
        // Can't look around if we're dead!
        if (this.healthController.IsDead())
            return;
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
        GameUtility.SetCanPause(true); // This is a fucking hack tho

        // Fucking hack to make UIs visible and invisible, should probably be done somewhere else
        // TODO : Clean this crap up please.
        UIManager.Instance?.GetDeathUIController().UI_SetVisible(false);
        UIManager.Instance?.GetFinishUIController().UI_SetVisible(false);
        UIManager.Instance?.GetPlayerUIController().UI_SetVisible(true);

        // Other Events
        this.healthController.OnDeath += HandleDeath;

        // User Input Events
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
        GameUtility.SetCanPause(false); // Same, fucking hack, need to find a cleaner way to do this shit.

        // Other Events
        this.healthController.OnDeath -= HandleDeath;

        // User Input Events
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
        this.spellCasterController.StartCasting();
    }

    private void RightClickUp()
    {
        this.spellCasterController.StopCasting();
    }

    private void SetForm(Form form)
    {
        this.spellCasterController.SetForm(form);
    }

    #endregion

    #region Other Events

    // NOTE : Deprecated for now. Could be used to add stagger animations when taking damage or something like that.
    private void HealthUpdated(float oldValue, float newValue)
    {
        if (newValue <= 0.0f)
        {
            GameUtility.SetCanPause(false);
            UIManager.Instance.GetDeathUIController().UI_SetVisible(true);
        }
    }

    private void HandleDeath()
    {
        this.spellCasterController.RemoveElements();
        this.spellCasterController.StopCasting();

        StartCoroutine(HandleDeathCoroutine());
    }

    private IEnumerator HandleDeathCoroutine()
    {
        GameUtility.SetCanPause(false); // We set the can pause to false before waiting for 2 seconds because that wait time is in real time, it does not account for time scale, which is what we use for pausing.
        AudioSource.PlayClipAtPoint(this.deathSound, CameraManager.Instance.GetActiveCamera().transform.position + CameraManager.Instance.GetActiveCamera().transform.forward * 3.0f);
        yield return new WaitForSeconds(2.0f);
        UIManager.Instance.GetDeathUIController().UI_SetVisible(true);
        SoundManager.Instance.PlayMusic("death", false);
    }

    #endregion

    #endregion
}
