using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This controller class makes use of the underlying DebugLineRendererController and the DebugManager's debug drawing system to render different types of shapes.
// NOTE : This renderer renders shapes based on collider type, so the whole shape business is temporarily disabled.
public class DebugShapeRendererController : MonoBehaviour
{
    #region Enums

    // TODO : Move this shit somewhere else, maybe.
    public enum Shape
    {
        None = 0,
        Line,
        Segment,
        Sphere
    }

    #endregion

    #region Variables

    // [SerializeField] private Shape shape; // The render shape used by this debug renderer.

    [SerializeField] private SphereCollider sphereCollider;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        
    }

    void Update()
    {
        if (sphereCollider != null)
        {
            float scale = Mathf.Max(this.sphereCollider.transform.localScale.x, this.sphereCollider.transform.localScale.y, this.sphereCollider.transform.localScale.z);
            DebugManager.Instance.DrawSphere(this.sphereCollider.transform.position, this.sphereCollider.radius * scale, Color.red);
        }
    }

    #endregion

    #region PublicMethods

    /*
    public void RenderShape(Shape shape)
    {
        switch (shape)
        {
            default:
            case Shape.None:
                // Don't render anything if the shape is none.
                break;
            case Shape.Line:

                break;
            case Shape.Segment:
                // Not implemented yet
                break;
            case Shape.Sphere:
                break;
        }
    }
    */

    #endregion

    #region PrivateMethods
    #endregion
}
