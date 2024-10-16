using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBeamController : SpellBaseController
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
        // Calculate the current max distance.
        // Prevents the beam from growing to full distance instantly, so that it is both more visually appealing and does not become an insta-killing sniper laser.
        this.currentMaxDistance = Mathf.Clamp(this.currentMaxDistance + this.distanceGrowthRate * Time.deltaTime, 0.0f, this.maxDistance);

        this.OriginPoint = this.transform.position;

        RaycastHit hit;
        bool hasHit = Physics.Raycast(this.transform.position, this.transform.forward, out hit, this.currentMaxDistance);

        if (hasHit)
        {
            this.TargetPoint = hit.point;
            this.currentMaxDistance = hit.distance; // Reset the current max distance to the distance between the origin point and the hit point so that the beam wont grow instantly when moving between surfaces at different distances.
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
    #endregion

    #region PrivateMethods
    #endregion

    #region ISpell

    public override void UpdateSpellColor()
    {
        this.lineRenderer.startColor = GetSpellColor();
        this.lineRenderer.endColor = GetSpellColor();
    }

    #endregion
}
