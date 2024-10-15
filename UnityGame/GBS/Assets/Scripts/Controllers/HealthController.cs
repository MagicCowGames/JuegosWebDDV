using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    #region Variables

    [SerializeField] private float health;
    [SerializeField] private float healthMax;
    [SerializeField] private float healthMin;

    public float Health { get { return this.health; } set { this.health = Mathf.Clamp(value, this.healthMin, this.healthMax); } }

    public float HealthMax { get { return this.healthMax; } set { this.healthMax = value; } }
    public float HealthMin { get { return this.healthMin; } set { this.healthMin = value; } }

    #endregion

    #region MonoBehaviour

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    #endregion

    #region PublicMethods
    #endregion

    #region PrivateMethods
    #endregion
}
