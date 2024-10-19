// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// 
// public class SpellBeamController : SpellBaseController
// {
//     #region Variables
// 
//     [Header("Prefab Reference")]
//     [SerializeField] private GameObject beamPrefab;
// 
//     [Header("Beam Settings")]
//     [SerializeField] private LineRenderer lineRenderer;
//     [SerializeField] private float maxDistance;
//     [SerializeField] private ParticleSystem particleStart;
//     [SerializeField] private ParticleSystem particleEnd;
//     [SerializeField] private CapsuleCollider capsuleCollider; // This capsule collider could be used both for beam collisions AND damage area. But maybe it makes more sense to only add a damage area at the target location, since that's technically the only possible contact point for a beam...
// 
//     private float currentMaxDistance;
//     private float distanceGrowthRate = 80.0f;
// 
//     public Vector3 OriginPoint { get; private set; }
//     public Vector3 TargetPoint { get; private set; }
// 
//     public float Length { get; private set; } // Contains the length of the beam, which is the distance between the origin point and the target point.
// 
// 
//     // NOTE : The encapsulation is fucked with these ones, but it's the only way I could come up with after a couple of beers to achieve setting
//     // the child beam on the collided-with beam when 2 beams collide with eachother so as to prevent spawning another fucking child.
//     public SpellBeamController OtherBeam { get; set; }
//     public SpellBeamController ChildBeam { get; set; }
// 
//     #endregion
// 
//     #region MonoBehaviour
// 
//     void Start()
//     {
//         this.currentMaxDistance = 0.0f;
// 
//         this.OtherBeam = null;
//         this.ChildBeam = null;
//     }
// 
//     void Update()
//     {
//         // Calculate the current max distance.
//         // Prevents the beam from growing to full distance instantly, so that it is both more visually appealing and does not become an insta-killing sniper laser.
//         this.currentMaxDistance = Mathf.Clamp(this.currentMaxDistance + this.distanceGrowthRate * Time.deltaTime, 0.0f, this.maxDistance);
// 
//         this.OriginPoint = this.transform.position;
// 
//         // Stupid fucking hack of a workaround. Prevents the beam from colliding with itself when performing the raycast.
//         // We disable the beam, raycast, then enable it again so that when other beams perform their raycasts, this beam will still
//         // have its collision enabled, all within the same frame.
//         // There is a proper Unity built-in system for this, but it seems like it only works with Physics2D...
//         this.capsuleCollider.enabled = false;
//         if (this.ChildBeam != null)
//             this.ChildBeam.GetComponent<CapsuleCollider>().enabled = false;
// 
//         RaycastHit hit;
//         bool hasHit = Physics.Raycast(this.transform.position, this.transform.forward, out hit, this.currentMaxDistance);
// 
//         this.capsuleCollider.enabled = true;
//         if (this.ChildBeam != null)
//             this.ChildBeam.GetComponent<CapsuleCollider>().enabled = true;
// 
//         if (hasHit)
//         {
//             this.TargetPoint = hit.point;
//             this.currentMaxDistance = hit.distance; // Reset the current max distance to the distance between the origin point and the hit point so that the beam wont grow instantly when moving between surfaces at different distances.
// 
//             this.OtherBeam = hit.collider.gameObject.GetComponent<SpellBeamController>();
// 
//             if (this.ChildBeam == null && this.OtherBeam != null)
//             {
//                 this.ChildBeam = ObjectSpawner.Spawn(this.beamPrefab, this.TargetPoint).GetComponent<SpellBeamController>();
//                 this.OtherBeam.ChildBeam = this.ChildBeam;
//             }
//             else
//             {
//                 if (this.ChildBeam != null)
//                     this.ChildBeam.transform.position = this.TargetPoint;
//             }
//         }
//         else
//         {
//             this.TargetPoint = this.OriginPoint + this.transform.forward * this.currentMaxDistance;
// 
//             if (this.OtherBeam != null)
//             {
//                 this.OtherBeam.ChildBeam = null;
//                 this.OtherBeam = null;
//                 Destroy(this.ChildBeam.gameObject);
//                 this.ChildBeam = null;
//             }
//         }
// 
//         this.Length = Vector3.Distance(this.TargetPoint, this.OriginPoint);
// 
// 
//         // Update the positions for the beam's line renderer
//         Vector3[] points = { this.OriginPoint, this.TargetPoint };
//         this.lineRenderer.SetPositions(points);
// 
//         // Update the positions for the beam's particles
//         this.particleStart.transform.position = this.OriginPoint;
//         this.particleEnd.transform.position = this.TargetPoint;
// 
//         // Update the capsule collider to fit the size of the beam
//         this.capsuleCollider.height = this.Length;
//         this.capsuleCollider.center = new Vector3(0.0f, 0.0f, this.Length / 2.0f);
//     }
// 
//     #endregion
// 
//     #region PublicMethods
//     #endregion
// 
//     #region PrivateMethods
//     #endregion
// 
//     #region ISpell
// 
//     public override void UpdateSpellColor()
//     {
//         this.lineRenderer.startColor = GetSpellColor();
//         this.lineRenderer.endColor = GetSpellColor();
// 
//         // TODO : Find something that is not deprecated...
//         this.particleStart.startColor = GetSpellColor();
//         this.particleEnd.startColor = GetSpellColor();
//     }
// 
//     #endregion
// }
