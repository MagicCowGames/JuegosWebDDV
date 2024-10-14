using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSpellController : MonoBehaviour
{
    #region Variables

    public GameObject owner;

    [Header("Spell Components")]
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private GameObject spellMesh;
    [SerializeField] private ParticleSystem sprayParticle;
    [SerializeField] private ParticleSystem beamParticle;

    [Header("Spell Config")]
    [SerializeField] private SpellType spellType;

    [Header("Spell Stats")]
    [SerializeField] private float damage = 10.0f;
    [SerializeField] private float speed = 100.0f;

    [Header("Spell Lifetime")]
    [SerializeField] private float duration1 = 10.0f; // Amount of time until the spell's effects are disabled and it's visual appearance is hidden.
    [SerializeField] private float duration2 = 3.0f; // Amount of time after disabled until the spell is actually removed.

    private float lifeTime;// Amount of time until the spell's entity is actually killed / returned to the pool.
    private float durationTime;

    private float elapsed;
    private int[] elementCounts;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        this.durationTime = this.duration1;
        this.lifeTime = this.duration1 + this.duration2;

        this.elapsed = 0.0f;
        this.elementCounts = new int[(int)Element.COUNT];

        if (this.spellType == SpellType.Projectile)
        {
            this.rigidBody.AddForce(rigidBody.transform.up * speed, ForceMode.VelocityChange);
        }
        else
        {
            this.rigidBody.useGravity = false;
            this.rigidBody.detectCollisions = false;
        }
    }

    void Update()
    {
        this.elapsed += Time.deltaTime;

        if (this.elapsed >= this.durationTime)
            DisableSpell();
        
        if (this.elapsed >= this.lifeTime)
            KillSpell();
    }

    #endregion

    #region PublicMethods
    #endregion

    #region PrivateMethods

    private void SpawnSpell()
    {
        this.sprayParticle.Play();
    }

    private void KillSpell()
    {
        this.gameObject.SetActive(false);
        Destroy(this.gameObject, 0.0f);
    }

    private void DisableSpell()
    {
        this.sprayParticle.Stop();
    }

    private void UpdateParticle()
    {
        
    }

    #endregion
}
