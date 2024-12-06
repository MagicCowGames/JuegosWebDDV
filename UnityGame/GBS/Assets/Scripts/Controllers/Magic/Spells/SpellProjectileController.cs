using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellProjectileController : SpellBaseController
{
    #region Variables

    [Header("Projectile Settings")]
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private float force;
    [SerializeField] private ParticleSystem elementParticles;
    [SerializeField] private MeshFilter projectileMeshRock;
    [SerializeField] private MeshFilter projectileMeshIceSpikes;
    [SerializeField] private MeshFilter projectileMeshIceShard;

    public float Force { get { return this.force; } set { this.force = value; } }

    #endregion

    #region MonoBehaviour

    void Start()
    {
        this.rigidBody.AddForce(this.transform.forward * force, ForceMode.Impulse);
    }

    void Update()
    {
        
    }

    #endregion

    #region PublicMethods
    #endregion

    #region PrivateMethods
    #endregion

    #region ISpell

    public override void UpdateSpellColor()
    {
        // TODO : Find non-deprecated alternative to do this.
        this.elementParticles.startColor = this.spellColor;

        bool hasRock = this.elementsCounts[(int)Element.Earth] > 0;
        bool hasIce = this.elementsCounts[(int)Element.Ice] > 0;

        this.projectileMeshRock.gameObject.SetActive(false);
        this.projectileMeshIceSpikes.gameObject.SetActive(false);
        this.projectileMeshIceShard.gameObject.SetActive(false);

        // Only display rock or ice according to whether the Earth and Ice elements are present or not.
        if (hasRock && hasIce)
        {
            this.projectileMeshRock.gameObject.SetActive(true);
            this.projectileMeshIceSpikes.gameObject.SetActive(true);
        }
        else
        {
            this.projectileMeshRock.gameObject.SetActive(hasRock);
            this.projectileMeshIceShard.gameObject.SetActive(hasIce);
        }
    }

    #endregion

    #region Collisions

    private void OnCollisionEnter(Collision collision)
    {
        // TODO : Change this logic when we implement spell pooling.
        Destroy(this.gameObject);
    }

    #endregion
}
