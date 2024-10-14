using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugLineRendererController : MonoBehaviour
{
    #region Variables

    public LineRenderer lineRenderer;
    public bool usedLastFrame;

    #endregion

    #region PublicMethods

    void Start()
    {
        this.lineRenderer = GetComponent<LineRenderer>();
        this.usedLastFrame = false;
    }

    void Update()
    {

    }

    void LateUpdate()
    {

    }

    #endregion

    #region PublicMethods
    #endregion

    #region PrivateMethods
    #endregion
}
