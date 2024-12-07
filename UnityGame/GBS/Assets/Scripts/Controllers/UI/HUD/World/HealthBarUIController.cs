using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUIController : UIController, IComponentValidator
{
    #region Variables

    [Header("Health Bar UIController")]
    [SerializeField] private HealthController healthController;
    [SerializeField] private LookAtController lookAtController;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Image healthBarImage;

    private float elapsedTime;
    private float timeToHide;

    private float previousHealthValue;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        this.elapsedTime = 0.0f;
        this.timeToHide = 5.0f;
        this.previousHealthValue = this.healthController.Health;
        this.canvasGroup.alpha = 0.0f; // Start with an invisible health bar to avoid cluttering the screen.
    }

    void Update()
    {
        if (!AllComponentsAreValid())
            return;

        float delta = Time.deltaTime;
        UpdateHealthValue(delta);
        UpdateHealthVisibility(delta);
        UpdateLookAtTarget();
    }

    #endregion

    #region PublicMethods
    #endregion

    #region PrivateMethods

    private void UpdateHealthValue(float delta)
    {
        float healthPercentage = this.healthController.GetPercentage();
        this.healthBarImage.fillAmount = Mathf.Clamp01(Mathf.Lerp(this.healthBarImage.fillAmount, healthPercentage, delta * 10.0f));
    }

    private void UpdateHealthVisibility(float delta)
    {
        this.elapsedTime += delta;

        // If the health value has changed, then make the bar visible again, update the previous health value stored within this component and reset the visibility timer.
        // NOTE : This could be changed to use an event instead of checking every single frame...
        if (Mathf.Abs(this.previousHealthValue - this.healthController.Health) > float.Epsilon)
        {
            this.canvasGroup.alpha = 1.0f;
            this.previousHealthValue = this.healthController.Health;
            this.elapsedTime = 0.0f;
        }

        // If the visibility time has passed since the last time the health value was changed, then make the health slowly invisible.
        if (this.elapsedTime >= this.timeToHide)
        {
            this.canvasGroup.alpha -= delta * 1.0f; // The alpha value from the CanvasGroup is clamped so we don't need to clamp it ourselves.
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
            this.healthController != null &&
            this.lookAtController != null &&
            this.healthBarImage != null
            ;
    }

    #endregion
}
