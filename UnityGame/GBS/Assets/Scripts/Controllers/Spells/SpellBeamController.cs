using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBeamController : MonoBehaviour
{
    #region Variables

    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float maxDistance;

    private float currentMaxDistance;
    private float distanceGrowthRate = 80.0f;
    
    public Vector3 OriginPoint { get; private set; }
    public Vector3 TargetPoint { get; private set; }

    #endregion

    #region MonoBehaviour

    void Start()
    {
        this.currentMaxDistance = 0.0f;
    }

    void Update()
    {
        this.currentMaxDistance = Mathf.Clamp(this.currentMaxDistance + this.distanceGrowthRate * Time.deltaTime, 0.0f, this.maxDistance);

        this.OriginPoint = this.transform.position;

        RaycastHit hit;
        bool hasHit = Physics.Raycast(this.transform.position, this.transform.forward, out hit, this.currentMaxDistance);

        if (hasHit)
        {
            this.TargetPoint = hit.point;
        }
        else
        {
            this.TargetPoint = this.OriginPoint + this.transform.forward * this.currentMaxDistance;
        }

        Vector3[] points = { this.OriginPoint, this.TargetPoint };
        this.lineRenderer.SetPositions(points);

    }

    #endregion

    #region PublicMethods

    public void SetSpellColor(Color color)
    {
        this.lineRenderer.startColor = color;
        this.lineRenderer.endColor = color * 0.5f;
    }

    #endregion

    #region PrivateMethods
    #endregion
}
