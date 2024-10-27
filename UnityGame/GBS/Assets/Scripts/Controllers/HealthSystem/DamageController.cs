using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO : Implement damage resistance / weakness / immunity system for each elemental damage type and stuff...
// This needs either changes on health and damage controllers, or a new ReisistanceController or ProtectionController or whatever tf.
public class DamageController : MonoBehaviour
{
    #region Enums

    // NOTE : Maybe replace this with a bool flag that determines the damage to be applied over time or not? I mean, there aren't any other damage "types" so yeah...
    // either that or make a DamageApplicationType enum in the public scope on the Data folder.
    public enum DamageType
    {
        Instant = 0,
        OverTime
    }

    #endregion

    #region Variables

    [Header("Components")]
    [SerializeField] private Collider damageCollider; // wtf are u used for tho TODO : Cleanup
    [SerializeField] private bool usesCollision = true;

    [Header("Damage Configuration")]
    [SerializeField] private float damage = 10.0f;
    [SerializeField] private DamageType damageType = DamageType.Instant;

    #endregion

    #region MonoBehaviour

    void Start()
    {

    }

    void Update()
    {
        DebugManager.Instance?.DrawSphere(this.transform.position, this.transform.lossyScale.z, Color.red);
    }

    #endregion

    #region PublicMethods

    // If the target has no health controller, then no damage will be applied, but the game won't explode either, we just early return, so it's all good.
    // NOTE : This function is redundant with the whole ApplyDamage() method, but I'm trying to come up with a relatively good and standardised public interface
    // for damaging and healing when protection is involved...
    public void Damage(GameObject target)
    {
        var protection = target.GetComponent<ProtectionController>();
        var health = target.GetComponent<HealthController>();

        if (health == null)
            return;

        // TODO : Add protection handling.

        health.Health -= this.damage;
    }

    #endregion

    #region PrivateMethods

    private void ApplyDamage(GameObject obj, float delta = 1.0f)
    {
        var hp = obj.GetComponent<HealthController>();
        if (hp == null)
            return;
        hp.Health -= this.damage * delta;
    }

    #endregion

    #region CollisionMethods

    void OnTriggerEnter(Collider other)
    {
        if (!this.usesCollision)
            return;
        if (this.damageType == DamageType.Instant)
            ApplyDamage(other.gameObject);
    }

    void OnTriggerStay(Collider other)
    {
        if (!this.usesCollision)
            return;
        if (this.damageType == DamageType.OverTime)
            ApplyDamage(other.gameObject, Time.deltaTime);
    }

    #endregion
}
