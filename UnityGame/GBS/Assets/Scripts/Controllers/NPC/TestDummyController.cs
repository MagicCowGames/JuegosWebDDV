using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// TODO : Move some of this logic to a base NPC class or something like that so that we can more easily implement NPCs. Either that or make an NPC component.
// Or maybe just have a basic set of "Entity" related things on an Entity component (like the health bar and handling all of the life stuff and canDie stuff, wet / burning status, etc...)
// And also an "NPCController" or "AIController" component to handle all of the combat and path finding related stuff.
// For now, just bundle it all together in the same place.
public class TestDummyController : MonoBehaviour
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

    private Vector3 gravityVector = new Vector3(0.0f, -9.8f, 0.0f);

    #endregion

    #region MonoBehaviour

    void Start()
    {
        this.healthController.OnDeath += HandleDeath;

        this.agent.updatePosition = false;
        this.agent.updateRotation = true;
    }

    void Update()
    {
        float delta = Time.deltaTime;

        UpdateMovement(delta);

        // Stupid fix for Unity's NavMeshAgent being kinda weird...
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
        // Only allow warping the logical agent position to the physical one if it is close enough to attach to a nav mesh.
        // This prevents any warnings from coming up. Could be discarded, but still, it's cleaner to use it.
        
        // Also only warp the NPC if the logical position has moved further than 2.0f units from the real position.
        // This crappy patchy solution allows Unity's built in nav mesh agent's rotation system to work and not shit its pants...
        
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
            // Update the destination to the player's position.
            this.agent.destination = PlayerDataManager.Instance.GetPlayer().transform.position;
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

    private void Move(float delta, Vector3 direction = default, float speed = 1.0f)
    {
        Vector3 movementVector = delta * direction * speed;
        this.characterController.Move(movementVector);
    }

    private void UpdateMovementWalk(float delta)
    {
        Move(delta, this.transform.forward, this.speed);
    }

    private void UpdateMovementGravity(float delta)
    {
        Move(delta, this.gravityVector);
    }

    private void UpdateMovement(float delta)
    {
        UpdateMovementWalk(delta);
        UpdateMovementGravity(delta);
    }

    #endregion
}
