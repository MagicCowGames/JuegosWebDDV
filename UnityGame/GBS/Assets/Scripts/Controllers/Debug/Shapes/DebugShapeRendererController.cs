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

    [SerializeField] private SphereCollider[] sphereColliders;
    [SerializeField] private BoxCollider[] boxColliders;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        
    }

    void Update()
    {
        DrawBoxes();
        DrawSpheres();
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

    private void DrawBoxes()
    {
        foreach (var boxCollider in boxColliders)
            DrawBox(boxCollider);
    }

    private void DrawSpheres()
    {
        foreach(var sphereCollider in sphereColliders)
            DrawSphere(sphereCollider);
    }

    private void DrawBox(BoxCollider boxCollider)
    {
        if (boxCollider == null)
            return;

        Vector3 origin = boxCollider.center + boxCollider.transform.position;
        Vector3 sideLengths = new Vector3(
            boxCollider.size.x * boxCollider.transform.localScale.x,
            boxCollider.size.y * boxCollider.transform.localScale.y,
            boxCollider.size.z * boxCollider.transform.localScale.z
            );
        Color color = Color.red;

        DebugManager.Instance.DrawBox(origin, sideLengths, color);
    }

    private void DrawSphere(SphereCollider sphereCollider)
    {
        if (sphereCollider == null)
            return;

        Vector3 origin = sphereCollider.center + sphereCollider.transform.position;
        float scale = Mathf.Max(
            sphereCollider.transform.localScale.x,
            sphereCollider.transform.localScale.y,
            sphereCollider.transform.localScale.z
            );
        float radius = sphereCollider.radius * scale;
        Color color = Color.red;

        DebugManager.Instance.DrawSphere(origin, radius, color);
    }

    #endregion
}
