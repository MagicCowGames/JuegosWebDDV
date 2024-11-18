using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpellShieldController : SpellBaseController
{
    #region Enums

    public enum ShieldAnimStatus
    {
        MovingUp = 0,
        Idle,
        MovingDown,
        Finish
    }

    #endregion

    #region Variables

    [Header("Shield Settings")]

    // TODO : Replace this with an actual health component so that walls can be damaged and broken down faster with counter spells such as projectiles and whatnot.
    [SerializeField] private float lifeTime = 5.0f;

    [SerializeField] private Transform shieldTransform;
    [SerializeField] private Transform startTransform;
    [SerializeField] private Transform idleTransform;

    [SerializeField] private float animSpeed = 10.0f;

    private ShieldAnimStatus animStatus;

    #endregion

    #region Properties

    public float LifeTime { get { return this.lifeTime; } set { this.lifeTime = value; } }
    public ShieldAnimStatus AnimStatus { get { return this.animStatus; } set { this.animStatus = value; } }

    #endregion

    #region MonoBehaviour

    void Start()
    {
        this.animStatus = ShieldAnimStatus.MovingUp;
        this.shieldTransform.position = this.startTransform.position;
    }

    void Update()
    {
        float delta = Time.deltaTime;
        float speed = this.animSpeed;
        Transform target;

        switch (this.animStatus)
        {
            case ShieldAnimStatus.MovingUp:
                target = this.idleTransform;
                if (MoveToTarget(target, speed, delta))
                    this.animStatus = ShieldAnimStatus.Idle;
                break;
            case ShieldAnimStatus.Idle:
                this.lifeTime -= delta;
                if(this.lifeTime < 0.0f)
                    this.animStatus = ShieldAnimStatus.MovingDown;
                break;
            case ShieldAnimStatus.MovingDown:
                target = this.startTransform;
                if (MoveToTarget(target, speed, delta))
                    this.animStatus = ShieldAnimStatus.Finish;
                break;
            default:
            case ShieldAnimStatus.Finish:
                this.gameObject.SetActive(false);
                Destroy(this.gameObject);
                break;
        }
    }

    #endregion

    #region PublicMethods
    #endregion

    #region PrivateMethods

    private bool MoveToTarget(Transform target, float speed, float delta)
    {
        if (Vector3.Distance(this.shieldTransform.position, target.position) > 0.01f)
        {
            this.shieldTransform.position = Vector3.Lerp(this.shieldTransform.position, target.position, Mathf.Clamp01(delta * speed));
            return false;
        }
        return true;
    }

    #endregion

    #region ISpell

    public override void UpdateSpellColor()
    {

    }

    #endregion

    #region Collision

    // If the wall collides with another wall, the other wall is removed / killed.
    // The wall actually deletes itself by setting their own lifetime to 0 if their lifetime is smaller than that of
    // the other wall.
    
    // NOTE : This currently has a bug where the walls will have the same life time when they first detect eachother
    // if they were spawned shortly one after the other, during the spawning animation (at that point in time,
    // the life time has not started to go down yet on either of them).
    // This means that, since both walls will have the same life time and they only detect eachother on the initial
    // collision, they will never update and remove eachother, leading both walls to acting as if their collision had
    // never happened and just despawning after 5 secs like normal.
    
    // TODO : Fix this shit. It will be trivial when I finally implement the health component on shield spells.
    private void OnTriggerEnter(Collider other)
    {
        DebugManager.Instance.Log("SHIELDS!!!!! fsafsafas");
        var wall = other.GetComponent<SpellShieldController>();
        if (wall == null)
            return;

        if (this.lifeTime < wall.LifeTime)
        {
            this.lifeTime = 0.0f;
        }
    }

    #endregion
}
