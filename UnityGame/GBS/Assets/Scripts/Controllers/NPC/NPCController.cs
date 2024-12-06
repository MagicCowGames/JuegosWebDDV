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
    [SerializeField] public HealthController healthController;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private NavMeshAgent agent;

    [Header("TestDummy Config")]
    [SerializeField] private bool hasAi = false;
    [SerializeField] private bool canDie = false;
    [SerializeField] private float speed = 3.0f;

    [Header("Weapons Components")] // Weapons systems
    [SerializeField] public SpellCasterController spellCaster;

    public bool CanDie { get { return this.canDie; } set { this.canDie = value; } }

    private Vector3 navTarget;
    public Vector3 NavTarget { get { return this.navTarget; } set { this.navTarget = GetClosestNavPoint(value); } }
    public GameObject Target { get; set; }

    private Vector3 gravityVector = new Vector3(0.0f, -9.8f, 0.0f);

    public float DistanceToNavTarget { get { return Vector3.Distance(this.transform.position, this.NavTarget); } }
    public float DistanceToTarget { get { return Vector3.Distance(this.transform.position, this.Target.transform.position); } }

    #endregion

    #region Variables - AI Related

    private float forwardAxis = 0.0f;
    public float ForwardAxis { get { return this.forwardAxis; } set { this.forwardAxis = value; } }

    [Header("AI States")]
    [SerializeField] private AIState_Main stateMain = AIState_Main.None;
    [SerializeField] private AIState_Wandering stateWandering = AIState_Wandering.None;
    [SerializeField] private AIState_Combat stateCombat = AIState_Combat.None;

    private float idleTime = 0.0f;
    private float wanderTime = 0.0f;

    private float retreatTime = 0.0f;

    IUtilityAction[] actions;

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

        this.actions = new IUtilityAction[] {
            new ChaseAction(this),
            new FleeAction(this),
            new AttackAction(this, new Element[]{Element.Fire, Element.Fire, Element.Earth}, Form.Projectile, 1.5f, 3.5f, 10.0f, 20.0f),
            new AttackAction(this, new Element[]{Element.Fire, Element.Death, Element.Death}, Form.Beam, 3.0f, 10.5f, 21.0f, 40.0f),
            new AttackAction(this, new Element[]{Element.Fire, Element.Fire}, Form.Projectile, 0.1f, 1.5f, 0.0f, 3.0f)
        };
    }

    void Update()
    {
        float delta = Time.deltaTime;

        UpdateMovement(delta);
        UpdateAI(delta);

        // For now, just walk toward the selected target GameObject.
        if (this.Target != null)
        {
            // this.forwardAxis = 1.0f;
            // this.NavTarget = this.Target.transform.position;
            // this.agent.destination = NavTarget;
            // DebugManager.Instance?.Log($"Distance = {this.DistanceToTarget}");
            this.stateMain = AIState_Main.Combat; // This should be set through an event only ONCE, but whatever... for now we do it this way lol...
        }

        // Stop the actor from moving if they reach a distance that is less than or equal to the stopping distance of the NavMeshAgent component.
        if (Vector3.Distance(this.transform.position, this.NavTarget) <= this.agent.stoppingDistance)
        {
            this.forwardAxis = 0.0f;
        }
    }

    #endregion

    #region PublicMethods

    public void Attack(Element[] elements, Form form, float castDuration)
    {
        if (this.spellCaster.GetIsCasting())
            return;

        // TODO : Use some kind of list of known spells or something...? or adjust the elements according to what the target uses, idk.
        this.spellCaster.AddElements(elements);
        this.spellCaster.SetForm(form);
        this.spellCaster.StartCasting();
        StartCoroutine(StopAttackCoroutine(castDuration));
    }

    // Using coroutines for this makes me cry blood, but it is what it is! time constraints and deadlines for the win, baby! Long live crappy code!!!
    private IEnumerator StopAttackCoroutine(float castDuration)
    {
        yield return new WaitForSeconds(castDuration); // You think he was the Don? but you got the Fever!
        if(this.spellCaster.GetIsCasting())
            this.spellCaster.StopCasting();
    }

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

        // If the NPC is casting a spell, just like the player, it cannot move.
        if (this.spellCaster.GetIsCasting())
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

        /*
        if (this.Target == null)
            return;
        */

        this.agent.destination = this.NavTarget;
    }

    // Returns the closest point to an input coordinate that is a valid point located within the nav mesh.
    private Vector3 GetClosestNavPoint(Vector3 position)
    {
        Vector3 ans = transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(position, out hit, 1000, NavMesh.AllAreas))
            ans = hit.position;
        return ans;
    }

    #endregion

    #region PrivateMethods - FSM

    private void UpdateFSM_Main(float delta)
    {
        switch (this.stateMain)
        {
            case AIState_Main.None:
            default:
                this.stateMain = AIState_Main.Idle;
                break;
            case AIState_Main.Idle:
                this.forwardAxis = 0.0f;
                this.idleTime += delta;
                if (this.idleTime >= 5.0f)
                {
                    this.idleTime = 0.0f;
                    this.stateMain = AIState_Main.Wandering;
                }
                break;
            case AIState_Main.Wandering:
                this.forwardAxis = 1.0f;
                UpdateFSM_Wandering(delta);
                break;
            case AIState_Main.Combat:
                UpdateUtilitySystem(delta);
                break;
        }
    }

    private void UpdateFSM_Wandering(float delta)
    {
        switch (this.stateWandering)
        {
            case AIState_Wandering.None:
            default:
                this.stateWandering = AIState_Wandering.SelectingTarget;
                break;
            case AIState_Wandering.SelectingTarget:
                float rngX = Random.Range(-20, 20);
                float rngY = Random.Range(-20, 20);
                Vector3 vec = new Vector3(rngX, 0, rngY);
                this.NavTarget = this.transform.position + vec;
                this.stateWandering = AIState_Wandering.MovingToTarget;
                break;
            case AIState_Wandering.MovingToTarget:
                DebugManager.Instance?.DrawSphere(this.NavTarget, 2, Color.magenta);
                this.wanderTime += delta;
                float distance = Vector3.Distance(this.transform.position, this.NavTarget);
                if (wanderTime > 5.0f || distance <= this.agent.stoppingDistance)
                {
                    this.stateWandering = AIState_Wandering.ArrivedToTarget;
                    this.wanderTime = 0.0f;
                }
                break;
            case AIState_Wandering.ArrivedToTarget:
                this.stateWandering = AIState_Wandering.None;
                this.stateMain = AIState_Main.Idle;
                break;
        }
    }

    private void UpdateFSM_Combat(float delta)
    {
        this.stateCombat = AIState_Combat.Chasing;

        float distanceToTarget = Vector3.Distance(this.transform.position, this.Target.transform.position);
        if (distanceToTarget <= 20.0f)
        {
            this.stateCombat = AIState_Combat.Fighting;
        }

        if (this.healthController.Health <= 10.0f && this.retreatTime < 5.0f)
        {
            this.stateCombat = AIState_Combat.Retreating;
        }

        switch (this.stateCombat)
        {
            case AIState_Combat.None:
            default:
                this.stateCombat = AIState_Combat.Chasing;
                break;
            case AIState_Combat.Chasing:
                this.forwardAxis = 1.0f;
                this.NavTarget = this.Target.transform.position;
                
                break;
            case AIState_Combat.Fighting:
                // TODO : Implement more complex fighting logic with a custom FSM with distances based on whether this NPC can perform
                // ranged attacks or not, distance to player, and other stuff like that, etc...
                break;
            case AIState_Combat.Retreating:
                if (this.retreatTime >= 5.0f)
                {
                    this.retreatTime = 0.0f;
                }
                break;
        }
    }

    #endregion

    #region PrivateMethods - US

    private void UpdateUtilitySystem(float delta)
    {
        IUtilityAction chosenAction = null;
        float maxValue = 0.0f;
        foreach (var action in this.actions)
        {
            var val = action.Calculate(delta);
            // DebugManager.Instance?.Log($"{action.Name} : {val}");
            if (val > maxValue)
            {
                maxValue = val;
                chosenAction = action;
            }
        }

        foreach (var action in this.actions)
            action.Update(delta);

        chosenAction?.Execute(delta);
    }

    #endregion

    #region PrivateMethods - AI State

    private void UpdateAI(float delta)
    {
        UpdateFSM_Main(delta);
        UpdatePathing();
    }

    #endregion
}
