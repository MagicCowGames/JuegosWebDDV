using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AlertUIController : UIController, IComponentValidator
{
    #region Variables

    [Header("Alert UIController")]
    [SerializeField] private NPCController npcController;
    [SerializeField] private LookAtController lookAtController;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Image alertImageDetecting;
    [SerializeField] private Image alertImageDetected;

    float elapsedTime;
    float timeToHide;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        this.elapsedTime = 0.0f;
        this.timeToHide = 2.5f;
        this.canvasGroup.alpha = 0.0f; // Start with an invisible health bar to avoid cluttering the screen.
        this.npcController.detectionProgress.OnValueChanged += OnDetectionChanged;
    }

    void Update()
    {
        if (!AllComponentsAreValid())
            return;

        float delta = Time.deltaTime;
        UpdateAlertUI(delta);
    }

    #endregion

    #region PublicMethods
    #endregion

    #region PrivateMethods

    private void UpdateAlertUI(float delta)
    {
        UpdateVisibility(delta);
        UpdateLookAtTarget();
    }

    private void UpdateVisibility(float delta)
    {
        if (this.npcController.detectionProgress.Value >= 1.0f)
        {
            this.elapsedTime += delta;
            if (this.elapsedTime >= this.timeToHide)
            {
                this.canvasGroup.alpha -= delta * 1.0f;
            }
        }
    }

    private void OnDetectionChanged(float oldValue, float newValue)
    {
        // Update the icon to match the current state
        bool showQuestion = newValue > 0.0f && newValue < 1.0f;
        bool showExclamation = newValue >= 1.0f;
        this.alertImageDetecting.gameObject.SetActive(showQuestion);
        this.alertImageDetected.gameObject.SetActive(showExclamation);

        // If the old value was lower than max detection, then we make the alert icon visible.
        if (oldValue < 1.0f)
        {
            this.canvasGroup.alpha = 1.0f;
        }
    }

    private void UpdateLookAtTarget()
    {
        this.lookAtController.TargetTransform = CameraManager.Instance.GetActiveCamera().transform;
    }

    #endregion

    #region IComponentValidator

    // TODO : In the future, we can add the camera and stuff like that to the init scene so that it always exists, and always use that one and manipulate it
    // with the camera manager with teleports and ease-in/out movements. That way, none of the null checks would have to be performed every single frame, since
    // we can guarantee that the data exists statically at all times from the moment the game data is initialized.
    public bool AllComponentsAreValid()
    {
        return
            CameraManager.Instance != null &&
            CameraManager.Instance.GetActiveCamera() != null &&
            this.npcController != null &&
            this.lookAtController != null &&
            this.alertImageDetecting != null &&
            this.alertImageDetected != null
            ;
    }

    #endregion
}
