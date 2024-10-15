using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBeamController : MonoBehaviour
{
    #region Variables

    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float maxDistance;
    
    public Vector3 OriginPoint { get; private set; }
    public Vector3 TargetPoint { get; private set; }

    #endregion

    #region MonoBehaviour

    void Start()
    {
        
    }

    void Update()
    {
        this.OriginPoint = this.transform.position;

        RaycastHit hit;
        bool hasHit = Physics.Raycast(this.transform.position, this.transform.forward, out hit, this.maxDistance);

        if (hasHit)
        {
            this.TargetPoint = hit.point;
        }
        else
        {
            this.TargetPoint = this.OriginPoint + this.transform.forward * this.maxDistance;
        }

        Vector3[] points = { this.OriginPoint, this.TargetPoint };
        this.lineRenderer.SetPositions(points);

    }

    #endregion

    #region PublicMethods
    #endregion

    #region PrivateMethods
    #endregion
}
