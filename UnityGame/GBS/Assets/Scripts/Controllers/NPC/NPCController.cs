using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// TODO : Move some of this logic to a base NPC class or something like that so that we can more easily implement NPCs. Either that or make an NPC component.
// Or maybe just have a basic set of "Entity" related things on an Entity component (like the health bar and handling all of the life stuff and canDie stuff, wet / burning status, etc...)
// And also an "NPCController" or "AIController" component to handle all of the combat and path finding related stuff.
// For now, just bundle it all together in the same place.
public class NPCController : MonoBehaviour
{
    #region Variables

    [Header("TestDummy Components")]
    [SerializeField] private HealthController healthController;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private NavMeshAgent agent;

    [Header("TestDummy Config")]
    [SerializeField] private bool hasAi = false;
    [SerializeField] private bool canDie = false;
    [SerializeField] private float speed = 3.0f;
    
    public bool CanDie { get { return this.canDie; } set { this.canDie = value; } }
    public Vector3 NavTarget { get; set; }
    public GameObject Target { get; set; }

    private Vector3 gravityVector = new Vector3(0.0f, -9.8f, 0.0f);

    #endregion

    #region Variables - AI State

    private float forwardAxis = 0.0f;

    private AIState state;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        this.healthController.OnDeath += HandleDeath;
        this.healthController.OnValueChanged += HandleDamaged;

        this.healthController.OnDeath += () => { PlayerDataManager.Instance.GetPlayerScore().Score += 150; };

        this.agent.updatePosition = true;
        this.agent.updateRotation = true;

        this.Target = null;
        this.NavTarget = Vector3.zero;
    }

    void Update()
    {
        float delta = Time.deltaTime;

        UpdateMovement(delta);
        UpdateFSM(delta);
        UpdatePathing();
        
        // For now, just walk toward the selected target GameObject.
        if (this.Target != null)
        {
            this.forwardAxis = 1.0f;
            this.NavTarget = this.Target.transform.position;
            // this.agent.destination = NavTarget;
        }

        // Stop the actor from moving if they reach a distance that is less than or equal to the stopping distance of the NavMeshAgent component.
        if (Vector3.Distance(this.transform.position, this.NavTarget) <= this.agent.stoppingDistance)
        {
            this.forwardAxis = 0.0f;
        }
    }

    #endregion

    #region PublicMethods
    #endregion

    #region PrivateMethods

    private void HandleDeath()
    {
        if (this.canDie)
            Destroy(this.gameObject);
    }

    private void HandleDamaged(float oldHealth, float newHealth)
    {
        this.Target = PlayerDataManager.Instance.GetPlayer().gameObject;
    }

    #endregion

    #region PrivateMethods - Physical Movement

    private void Move(float delta, Vector3 direction = default, float speed = 1.0f)
    {
        Vector3 movementVector = delta * direction * speed;
        this.characterController.Move(movementVector);
    }

    private void UpdateMovementWalk(float delta)
    {
        Move(delta, this.transform.forward, this.speed * this.forwardAxis);
    }

    private void UpdateMovementGravity(float delta)
    {
        Move(delta, this.gravityVector);
    }

    private void UpdateMovement(float delta)
    {
        // AI independent movement
        UpdateMovementGravity(delta);

        // AI dependent movement
        if (!this.hasAi)
            return;
        UpdateMovementWalk(delta);
    }

    #endregion

    #region PrivateMethods - Pathing and AI Logical position handling

    // All of this convoluted crap is filled with patches and terrible solutions to artificially created problems that exist due to Unity's terrible
    // nav mesh implementation that everyone seems to praise, except those who are not on a payroll ofc.
    // In any case, after much effort, I have been able to get something decent (and expensive) out of it...
    // At this point, I think I should just write my own solution for a nav mesh, but I don't have the time to do so, so I'll rely on this shit.

    // Do note that 90% of what's written here is comments, all because the hacky solutions need to be explained so that they make sense...

    // NOTE : This function is now deprecated since I have found a better way to get an Unreal-like behaviour by mixing the character controller component
    // and the NavMeshAgent component. This is not properly documented, but it works because agent.updatePosition makes it so that the nav mesh agent's logical
    // position sticks to the agent's real physical position if a character controller component is involved. This is not actually written anywhere within the
    // documentation, but they say that this is not UB and that it is expected, so I suppose I'll keep using it for now, since it does feel like a more
    // simple solution.

    // This behaviour does not appear to be documented officially but it is consistent across versions, so it is going to be the solution used for now.

    private void UpdatePathing_OLD()
    {
        // If the AI is disabled, just early return because no pathing logic should be performed at all.
        if (!this.hasAi)
            return;

        // Stupid fixes for Unity's NavMeshAgent being kinda weird...
        #region Comment - this.agent.Warp()
        /*
            This is such a fucking stupid situation that I had to make a region exclussively for this comment that is basically a wall of text...
            Stupid fix that exists because Unity's NavMeshAgent components seem to be allowed to move freely on their own away from their body if
            updatePosition is set to false...
            updatePosition in the documentation promises that the AI controller would not update its position allowing the user to use their own method
            for position updating, such as physics or a character controller, allwoing the NavMeshAgent component to be used as a logical unit for pathing...
            sadly this is not the case, and what it actually does is allow the logical pathing to move through the nav mesh independent of the body.
            Funnily enough, the origin of the GameObject changes, causing any other form of movement to spaz out.
            The solution in the official documentation hidden deep within layers and layers of indirection is legit, I shit you not, to warp the AI controller
            to the position where we want the NPC's physical representation to be located. What the fuck is this shit. Wouldn't it make more sense to just
            have a method to NOT update the AI's logical position and set it manually or use nextPosition? yes, yes it would. But the Unity people seem to disagree.

            In short: it's pretty cool that the logical position and physical position can be separated, but why can't we define the logical position to follow
            a GO's transform out of the box? I know it would do the same as updating it ourselves every single frame but... then why the fuck does it update and move
            on its own rather than taking as an input the new position?
        */
        #endregion
        #region Comment - NavMehs.SamplePosition() and Distance() > 2.0f

        // Only allow warping the logical agent position to the physical one if it is close enough to attach to a nav mesh.
        // This prevents any warnings from coming up. Could be discarded, but still, it's cleaner to use it.

        // Also only warp the NPC if the logical position has moved further than 2.0f units from the real position.
        // This crappy patchy solution allows Unity's built in nav mesh agent's rotation system to work and not shit its pants...

        #endregion
        
        NavMeshHit hit;
        bool hasHit = NavMesh.SamplePosition(this.characterController.transform.position, out hit, 1.0f, NavMesh.AllAreas);
        if (hasHit && Vector3.Distance(this.agent.nextPosition, this.transform.position) > 2.0f)
        {
            this.agent.Warp(this.characterController.transform.position);
        }

        // Only update AI / nav agent logic if the agent is within the bounds of a nav mesh.
        // Prevents the agent from trying to path if they are out of a nav mesh, which prevents any errors from coming up.
        if (this.agent.isOnNavMesh)
        {
            // Update the destination to the target position
            this.agent.destination = this.NavTarget;
        }
    }

    private void UpdatePathing()
    {
        if (!this.hasAi)
            return;

        if (this.Target == null)
            return;

        this.agent.destination = this.NavTarget;
    }

    #endregion

    #region PrivateMethods - FSM

    private void UpdateFSM(float delta)
    {
        /*
        switch (this.state)
        {
            case AIState.Idle:
                this.forwardAxis = 0.0f;
                if (this.Target != null)
                    this.state = AIState.Chasing;
                break;
            case AIState.Wandering:
                this.forwardAxis = 1.0f;
                break;
            case AIState.Chasing:
                this.forwardAxis = 1.0f;
                break;
            case AIState.Fighting:
                break;
            default:
                this.state = AIState.Idle;
                break;
        }
        */
    }

    #endregion
}
