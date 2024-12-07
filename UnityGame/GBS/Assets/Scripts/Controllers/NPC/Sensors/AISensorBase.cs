using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AISensorBase : MonoBehaviour
{
    #region Variables

    [SerializeField] public bool senseOnEnter = true;
    [SerializeField] public bool senseOnStay = false;
    [SerializeField] public bool senseOnExit = false;

    public Action<GameObject> OnSense;

    #endregion

    #region ProtectedMethods

    protected virtual void Sense(GameObject obj)
    {
        this.OnSense?.Invoke(obj);
    }

    #endregion

    #region Collisions

    void OnTriggerEnter(Collider other)
    {
        if(this.senseOnEnter)
            this.Sense(other.gameObject);
    }

    void OnTriggerStay(Collider other)
    {
        if (this.senseOnStay)
            this.Sense(other.gameObject);
    }

    void OnTriggerExit(Collider other)
    {
        if (this.senseOnExit)
            this.Sense(other.gameObject);
    }

    #endregion
}
