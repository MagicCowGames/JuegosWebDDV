using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO : Make it so that child beams are slightly more powerful and combine the parent beam elements...
// Also make it so that when beams collide at extremely steep anbles (ie: facing eachother) they cause an explosion in the middle or something.
public class SpellBeamController : SpellBaseController
{
    #region Variables

    [Header("Prefab Reference")]
    [SerializeField] private GameObject beamPrefab;

    [Header("Beam Settings")]
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float maxDistance;
    [SerializeField] private ParticleSystem particleStart;
    [SerializeField] private ParticleSystem particleEnd;
    [SerializeField] private CapsuleCollider capsuleCollider; // This capsule collider could be used both for beam collisions AND damage area. But maybe it makes more sense to only add a damage area at the target location, since that's technically the only possible contact point for a beam...
    [SerializeField] private bool canMix = false;

    private float currentMaxDistance;
    private float distanceGrowthRate = 80.0f;
    
    public Vector3 OriginPoint { get; private set; }
    public Vector3 TargetPoint { get; private set; }

    public float Length { get; private set; } // Contains the length of the beam, which is the distance between the origin point and the target point.


    // NOTE : The encapsulation is fucked with these ones, but it's the only way I could come up with after a couple of beers to achieve setting
    // the child beam on the collided-with beam when 2 beams collide with eachother so as to prevent spawning another fucking child.
    public SpellBeamController OtherBeam { get; set; }
    public SpellBeamController ChildBeam { get; set; }

    public SpellBeamController ParentBeamA { get; set; }
    public SpellBeamController ParentBeamB { get; set; }

    private bool isChildOwner;

    public bool IsChild { get; private set; }

    #endregion

    #region MonoBehaviour

    void Awake()
    {
        this.currentMaxDistance = 0.0f;
        this.OtherBeam = null;
        this.ChildBeam = null;
        this.ParentBeamA = null;
        this.ParentBeamB = null;
        this.IsChild = false;
        this.isChildOwner = false;
    }

    void Start()
    {
        this.GetComponent<HealthModifierController>().Owner = this.owner;
    }

    void Update()
    {
        UpdateBeam();
    }

    #endregion

    #region PublicMethods
    #endregion

    #region PrivateMethods

    private void UpdateBeamMaxDistance()
    {
        // Calculate the current max distance for the beam.
        // Prevents the beam from growing to full distance instantly, so that it is both more visually appealing and does not become an insta-killing sniper laser.
        float delta = Time.deltaTime;
        this.currentMaxDistance = Mathf.Clamp(this.currentMaxDistance + this.distanceGrowthRate * delta, 0.0f, this.maxDistance);
    }

    private void SetBeamCollision(bool collisionEnabled)
    {
        // Set the beam's collision
        this.capsuleCollider.enabled = collisionEnabled;

        // If we have a child beam, we also set the collision so that it won't affect our raycasts.
        if (this.ChildBeam != null)
            this.ChildBeam.capsuleCollider.enabled = collisionEnabled;

        // If we are a child beam, we also set the collision of our parents so that it won't affect our raycasts.
        if (this.ParentBeamA != null)
        {
            this.ParentBeamA.capsuleCollider.enabled = collisionEnabled;
            // DebugManager.Log("parent A off");
        }
        if (this.ParentBeamB != null)
        {
            this.ParentBeamB.capsuleCollider.enabled = collisionEnabled;
            // DebugManager.Log("parent B off");
        }
    }

    // TODO : Implement spell object pool in magic manager in the future.
    private void UpdateBeam()
    {
        // Update the beam's max distance
        UpdateBeamMaxDistance();
        
        // Update Beam's origin point
        this.OriginPoint = this.transform.position;

        // Handle the beam raycast logic
        #region Comment

        // Stupid fucking hack of a workaround. Prevents the beam from colliding with itself when performing the raycast.
        // We disable the beam, raycast, then enable it again so that when other beams perform their raycasts, this beam will still
        // have its collision enabled, all within the same frame.
        // There is a proper Unity built-in system for this, but it seems like it only works with Physics2D...

        #endregion
        SetBeamCollision(false); // Stupid hack. Disable the beam collision before ray casting to avoid self collisions.
        UpdateBeamRaycastLogic();
        SetBeamCollision(true); // Enable the collision again.

        // Update the beam's length
        this.Length = Vector3.Distance(this.TargetPoint, this.OriginPoint);

        // Update the positions for the beam's line renderer
        Vector3[] points = { this.OriginPoint, this.TargetPoint };
        this.lineRenderer.SetPositions(points);

        // Update the positions for the beam's particles
        this.particleStart.transform.position = this.OriginPoint;
        this.particleEnd.transform.position = this.TargetPoint;

        // Update the capsule collider to fit the size of the beam
        this.capsuleCollider.height = this.Length;
        this.capsuleCollider.center = new Vector3(0.0f, 0.0f, this.Length / 2.0f);

        // Check if this beam is a child and must destroy themselves
        if (
            this.IsChild &&
            (
                (this.ParentBeamA == null || this.ParentBeamB == null)
            )
        )
        {
            Destroy(this.gameObject);
        }
    }

    private void UpdateBeamRaycastLogic()
    {
        // Make a raycast to check if the beam is colliding with anything.
        RaycastHit hit;
        bool hasHit = Physics.Raycast(this.transform.position, this.transform.forward, out hit, this.currentMaxDistance);

        // If the beam has hit, handle hit logic
        if (hasHit)
        {
            // Reset the current max distance to the distance between the origin point and the hit point so that the beam wont grow instantly
            // when moving between surfaces at different distances
            this.currentMaxDistance = hit.distance;

            // Check if the hit surface is another beam and set this.OtherBeam to the returned value.
            this.OtherBeam = hit.collider.gameObject.GetComponent<SpellBeamController>();
        }
        else
        {
            // If there is no hit, reset the this.OtherBeam reference.
            this.OtherBeam = null;
        }

        // Check if we are in contact with another beam
        // NOTE : This is a fucking hack to deal with Unity's shitty collision system... it should not exist, but here we are...
        bool isContactingBeam = this.OtherBeam != null && (hasHit || (Vector3.Distance(this.TargetPoint, this.OtherBeam.TargetPoint) < 1.0f));

        // Update the max distance by setting it to the min value between the current max distance and the length of the other beam.
        if (isContactingBeam)
        {
            this.currentMaxDistance = Mathf.Min(this.currentMaxDistance, Vector3.Distance(this.OriginPoint, this.OtherBeam.TargetPoint)) + 0.5f;
        }

        // Set the target point to the max distance point.
        this.TargetPoint = this.OriginPoint + this.transform.forward * this.currentMaxDistance;


        // Handle collisions with other beams and child beam spawning
        // NOTE : Only allows generating child beams if both beam are allowed to mix
        if (isContactingBeam && this.canMix && this.OtherBeam.canMix)
        {
            // Update the other beam's OtherBeam reference to this beam.
            // If this beam was colliding with another beam and now collides with this one, it means that this beam is colliding at an earlier point along
            // the other beam's length, so it will be shortened and we don't need to do anything else to update the logic.
            this.OtherBeam.OtherBeam = this;

            // Handle the child beam logic
            // If both beams don't have a child beam, we spawn it and claim ownership (whoever comes first becomes the "child owner")
            if (this.ChildBeam == null && this.OtherBeam.ChildBeam == null)
            {
                this.isChildOwner = true;
                this.OtherBeam.isChildOwner = false;
                this.ChildBeam = ObjectSpawner.Spawn(this.beamPrefab, this.TargetPoint).GetComponent<SpellBeamController>();


                // Set the child beam's element counts to make a new spell.
                // TODO : Maybe change this to make use of the element mixing system so that beams with opposites cancel their opposites
                // rather than creating super OP beams? Or maybe that's part of the fun of beams! breaking the rules and stuff!
                // To do this, it would make more sense to add a ElementQueue to the SpellBaseController, which would make things easier for a small price to pay regarding memory usage / potential fragmentation... which would be basically gone when pooling is implemented. Kinda.
                // Also, if we use an EQ, we can always make a new one with a copy of the parents, or make one that has as total slots the sum of the slots of ParentA and ParentB, which will guarantee that each successive child beam can potentially become ever so stronger.
                int[] childElementCounts = new int[this.elementsCounts.Length];
                int count = 0;
                for (int i = 0; i < childElementCounts.Length; ++i)
                {
                    childElementCounts[i] = this.elementsCounts[i] + this.OtherBeam.elementsCounts[i];
                    count += childElementCounts[i];
                }
                this.ChildBeam.SetSpellData(count, childElementCounts);
                
                this.ChildBeam.IsChild = true;
                this.ChildBeam.ParentBeamA = this;
                this.ChildBeam.ParentBeamB = this.OtherBeam;
                this.OtherBeam.ChildBeam = this.ChildBeam;
            }
            // If either of the beams has a child, then we handle the child beam logic, but only if we are the child owner.
            else
            {
                if (this.isChildOwner)
                {
                    // Remind daddy to come back home with the milk...
                    this.OtherBeam.ChildBeam = this.ChildBeam;

                    // Set the child beam position to the middle point between the target points of both beams.
                    this.ChildBeam.transform.position = (this.TargetPoint + this.OtherBeam.TargetPoint) / 2.0f;

                    // Set the child beam rotation to the average between the rotations of the parent beams.
                    this.ChildBeam.transform.rotation = Quaternion.Lerp(this.transform.rotation, this.OtherBeam.transform.rotation, 0.5f);

                    // Remind the child beam who's daddy!
                    this.ChildBeam.ParentBeamA = this;
                    this.ChildBeam.ParentBeamB = this.OtherBeam;
                }
            }
        }
        else
        {
            this.OtherBeam = null;
            if (!this.isChildOwner)
                this.ChildBeam = null;
        }

        if (this.isChildOwner && this.ChildBeam != null && this.OtherBeam == null)
        {
            this.ChildBeam.ParentBeamA = null;
            this.ChildBeam.ParentBeamB = null;
            this.ChildBeam = null;
            this.isChildOwner = false;
        }
    }

    #endregion

    #region ISpell

    public override void UpdateSpellColor()
    {
        // NOTE : Still works even after adding custom shaders since we're using vertex color.
        this.lineRenderer.startColor = GetSpellColor();
        this.lineRenderer.endColor = GetSpellColor();

        // TODO : Find something that is not deprecated...
        this.particleStart.startColor = GetSpellColor();
        this.particleEnd.startColor = GetSpellColor();

        // NOTE : The following is an example of how to change color when using custom shaders that have a color property that is not tied to vertex color.
        // This works because we pray that there's always a material in slot 0. Which should be the case with the beam prefab tbh.
        // And if it isn't that means that someone fucked up big time by removing the material from the line renderer.
        // this.lineRenderer.materials[0].color = GetSpellColor();
    }

    #endregion
}
