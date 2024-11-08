using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AISensorBase : MonoBehaviour
{
    #region Variables

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
        this.Sense(other.gameObject);
    }

    #endregion
}
