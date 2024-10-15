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

    public float GetPercentage()
    {
        // NOTE : We don't care about dividing by 0 because these are not integer values.
        // Division by 0 is a perfectly valid operation for floating point values.
        // Now, whether it gives a value that is useable or even consistent or not is a whole other story,
        // but what matters is that the universe does not explode if we don't make this check.
        // Besides, who the fuck would set the min and max to the same value? Am I right? (famous last words)
        return this.health / (this.healthMax - this.healthMin);
    }

    public bool IsAlive()
    {
        // har har har such a smart function!!! Yes, I know, this is indeed a real thing, leave me alone...
        return this.health > 0.0f;
    }

    public void ForceSetHealth(float value)
    {
        this.health = value;
    }

    public void Heal()
    {
        this.health = this.HealthMax;
    }

    #endregion

    #region PrivateMethods
    #endregion
}
