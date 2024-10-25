using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUIController : UIController, IComponentValidator
{
    #region Variables

    [Header("Health Bar UIController")]
    [SerializeField] private HealthController healthController;
    [SerializeField] private LookAtController lookAtController;
    [SerializeField] private Image healthBarImage;

    #endregion

    #region MonoBehaviour

    void Start()
    {

    }

    void Update()
    {
        if (!AllComponentsAreValid())
            return;

        float delta = Time.deltaTime;
        UpdateHealthBar(delta);
        UpdateLookAtTarget();
    }

    #endregion

    #region PublicMethods
    #endregion

    #region PrivateMethods

    private void UpdateHealthBar(float delta)
    {
        float healthPercentage = this.healthController.GetPercentage();
        this.healthBarImage.fillAmount = Mathf.Clamp01(Mathf.Lerp(this.healthBarImage.fillAmount, healthPercentage, delta * 10.0f));
    }

    private void UpdateLookAtTarget()
    {
        this.lookAtController.TargetTransform = CameraManager.Instance.GetActiveCamera().transform;
    }

    #endregion

    #region IComponentValidator

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
