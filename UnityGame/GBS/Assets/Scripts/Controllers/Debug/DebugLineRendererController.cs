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
        // This component is now gotten from the inspector panel so we don't need to get it from here anymore.
        // Getting it from here used to cause some issues with race conditions where sometimes, the component was assigned to the lineRenderer variable
        // AFTER it had been used in the code that calls GetLineRenderer() on the DebugManager, meaning that we would get a dumb null pointer exception...
        // this.lineRenderer = GetComponent<LineRenderer>();
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
