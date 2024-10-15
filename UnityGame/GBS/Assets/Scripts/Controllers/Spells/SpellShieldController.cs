using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellShieldController : MonoBehaviour
{
    #region Enums

    private enum ShieldAnimStatus
    {
        MovingUp = 0,
        Idle,
        MovingDown,
        Finish
    }

    #endregion

    #region Variables

    [SerializeField] private float lifeTime = 5.0f;

    [SerializeField] private Transform shieldTransform;
    [SerializeField] private Transform startTransform;
    [SerializeField] private Transform idleTransform;

    [SerializeField] private float animSpeed = 10.0f;

    private ShieldAnimStatus animStatus;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        this.animStatus = ShieldAnimStatus.MovingUp;
        this.shieldTransform.position = this.startTransform.position;
    }

    void Update()
    {
        float delta = Time.deltaTime;
        float speed = this.animSpeed;
        Transform target;

        switch (this.animStatus)
        {
            case ShieldAnimStatus.MovingUp:
                target = this.idleTransform;
                if (MoveToTarget(target, speed, delta))
                    this.animStatus = ShieldAnimStatus.Idle;
                break;
            case ShieldAnimStatus.Idle:
                this.lifeTime -= delta;
                if(this.lifeTime < 0.0f)
                    this.animStatus = ShieldAnimStatus.MovingDown;
                break;
            case ShieldAnimStatus.MovingDown:
                target = this.startTransform;
                if (MoveToTarget(target, speed, delta))
                    this.animStatus = ShieldAnimStatus.Finish;
                break;
            default:
            case ShieldAnimStatus.Finish:
                this.gameObject.SetActive(false);
                Destroy(this.gameObject);
                break;
        }
    }

    #endregion

    #region PublicMethods
    #endregion

    #region PrivateMethods

    private bool MoveToTarget(Transform target, float speed, float delta)
    {
        if (Vector3.Distance(this.shieldTransform.position, target.position) > 0.01f)
        {
            this.shieldTransform.position = Vector3.Lerp(this.shieldTransform.position, target.position, Mathf.Clamp01(delta * speed));
            return false;
        }
        return true;
    }

    #endregion
}
