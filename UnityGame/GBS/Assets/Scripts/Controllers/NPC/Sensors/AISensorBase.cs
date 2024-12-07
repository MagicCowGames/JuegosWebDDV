using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AISensorBase : MonoBehaviour
{
    #region Variables

    [SerializeField] protected Transform originTransform;
    [SerializeField] protected float detectionAmount = 2.0f;
    [SerializeField] public bool senseOnEnter = true;
    [SerializeField] public bool senseOnStay = false;
    [SerializeField] public bool senseOnExit = false;

    public Action<GameObject, float> OnSense;

    #endregion

    #region ProtectedMethods

    protected virtual void Sense(GameObject obj, float delta, float distance)
    {
        this.OnSense?.Invoke(obj, delta * 1.0f / distance); // NOTE : This is an example invokation that the caller could do on their specific implementation.
    }

    #endregion

    #region Collisions

    void OnTriggerEnter(Collider other)
    {
        if (this.senseOnEnter)
            this.Sense(other.gameObject, 1.0f, Vector3.Distance(this.originTransform.position, other.gameObject.transform.position));
    }

    void OnTriggerStay(Collider other)
    {
        if (this.senseOnStay)
            this.Sense(other.gameObject, Time.deltaTime, Vector3.Distance(this.originTransform.position, other.gameObject.transform.position));
    }

    void OnTriggerExit(Collider other)
    {
        if (this.senseOnExit)
            this.Sense(other.gameObject, 1.0f, Vector3.Distance(this.originTransform.position, other.gameObject.transform.position));
    }

    #endregion
}
