using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

// TODO : Move some of this logic to a base NPC class or something like that so that we can more easily implement NPCs. Either that or make an NPC component.
// Or maybe just have a basic set of "Entity" related things on an Entity component (like the health bar and handling all of the life stuff and canDie stuff, wet / burning status, etc...)
// And also an "NPCController" or "AIController" component to handle all of the combat and path finding related stuff.
// For now, just bundle it all together in the same place.
public class NPCController : MonoBehaviour
{
    #region Variables - Inspector

    [Header("NPC Components")]
    [SerializeField] public HealthController healthController;
    [SerializeField] public CharacterController characterController;
    [SerializeField] public NavMeshAgent agent;

    [Header("NPC Config")]
    [SerializeField] private bool hasAi = false;
    [SerializeField] private bool canDie = false;
    [SerializeField] private float speed = 3.0f;
    // TODO : Add a turn speed configuration variable here that modifies the nav mesh agent's rotation speed...
    // TODO : Add isAttacking value to prevent enemies from starting attacks while an attack coro is already running (which is what is causing a bug as of now with weird attacks)
    [SerializeField] private float timeToDespawnOnDeath = 0.0f;
    [SerializeField] private int score = 150;

    [Header("NPC Weapons Components")] // Weapons systems
    [SerializeField] public SpellCasterController spellCaster;

    [Header("NPC Events")]
    [SerializeField] private UnityEvent OnNPCDeath;

    #endregion

    #region Variables - Properties and private variables

    private Vector3 gravityVector = new Vector3(0.0f, -9.8f, 0.0f);
    private bool hasAwardedPlayer = false; // NOTE : Would make more sense to replace this with a "hasDied" variable, which would have more uses, or maybe on the health controller just make it impossible to be resurrected unless a resurrection command is issued...

    public bool CanDie { get { return this.canDie; } set { this.canDie = value; } }

    private Vector3 navTarget;
    public Vector3 NavTarget { get { return this.navTarget; } set { this.navTarget = GetClosestNavPoint(value); } }
    
    public GameObject Target { get; set; }

    public float DistanceToNavTarget { get { return Vector3.Distance(this.transform.position, this.NavTarget); } }
    public float DistanceToTarget { get { return Vector3.Distance(this.transform.position, this.Target.transform.position); } }

    private float forwardAxis = 0.0f;
    public float ForwardAxis { get { return this.forwardAxis; } set { this.forwardAxis = value; } }

    public Vector3 Velocity { get; private set; } = Vector3.zero;

    #endregion

    #region Variables - AI Related

    public bool isFleeing = false;
    public bool hasLineOfSight = false;
    public bool isAttacking = false;

    public ObservableValue<float> detectionProgress = new ObservableValue<float>(0.0f);

    #endregion

    #region MonoBehaviour

    void Start()
    {
        Init();
    }

    void Update()
    {
        float delta = Time.deltaTime;
        UpdateAI(delta);

        // if(this.detectionProgress > 0.0f)
        //     DebugManager.Instance?.Log($"detection = {detectionProgress}");
    }

    #endregion

    #region PublicMethods

    // TODO : Maybe should rename Attack() to Cast()...?
    // TODO : In the future, support for multiple spell casters will be added to the NPCController, which will allow more complex NPCs to have more than one caster.
    // For it to be easy to work with, add an index input param for the Attack() function so that we can specify what specific spell caster to use for this attack.
    public void Attack(Element[] elements, Form form, float castDuration) // This version instantly queues all elements in one go
    {
        if (this.isAttacking)
            return;
        this.isAttacking = true;

        if (this.spellCaster.GetIsCasting())
            // return;
            this.spellCaster.StopCasting(); // This is an alternative but I am not sure if it makes things better or worse...

        // TODO : Use some kind of list of known spells or something...? or adjust the elements according to what the target uses, idk.
        this.spellCaster.AddElements(elements);
        this.spellCaster.SetForm(form);
        this.spellCaster.StartCasting();
        StartCoroutine(StopAttackCoroutine(castDuration));
    }

    public void Attack(Element[] elements, Form form, float castDuration, float queueTime) // queueTime determines how long it takes to queue each element.
    {
        if (this.isAttacking)
            return;
        this.isAttacking = true;

        if (this.spellCaster.GetIsCasting())
            // return;
            this.spellCaster.StopCasting();

        StartCoroutine(StartAttackCoroutine(elements, form, castDuration, queueTime));
    }

    private IEnumerator StartAttackCoroutine(Element[] elements, Form form, float castDuration, float queueTime)
    {
        this.spellCaster.SetForm(form);

        foreach(var element in elements)
        {
            this.spellCaster.AddElement(element);
            yield return new WaitForSeconds(queueTime);
        }
        this.spellCaster.StartCasting();
        
        yield return new WaitForSeconds(castDuration);
        
        if(this.spellCaster.GetIsCasting())
            this.spellCaster.StopCasting();
        this.isAttacking = false;
    }

    // Using coroutines for this makes me cry blood, but it is what it is! time constraints and deadlines for the win, baby! Long live crappy code!!!
    private IEnumerator StopAttackCoroutine(float castDuration)
    {
        yield return new WaitForSeconds(castDuration); // You thought that man was the Don? (fever!) but he gave you the Fever!
        if(this.spellCaster.GetIsCasting())
            this.spellCaster.StopCasting();
        this.isAttacking = false;
    }

    #endregion

    #region PrivateMethods

    private void Init()
    {
        this.healthController.OnDeath += HandleDeath;
        this.healthController.OnValueChanged += HandleDamaged;

        this.healthController.OnDeath += () => { GiveScore(); };

        this.agent.updatePosition = true;
        this.agent.updateRotation = true;

        this.Target = null;
        this.NavTarget = Vector3.zero;
    }

    private void UpdateAI(float delta)
    {
        // Stop the actor from moving if they reach a distance that is less than or equal to the stopping distance of the NavMeshAgent component.
        // NOTE : We do this before UpdateMovement() since the NPC behaviour controller could have modified the forward axis vector on its own, so
        // it's important to override it by hand here.
        if (Vector3.Distance(this.transform.position, this.NavTarget) <= this.agent.stoppingDistance)
        {
            this.forwardAxis = 0.0f;
        }

        // Update movement and pathing logic
        UpdateMovement(delta);
        UpdatePathing();
    }

    private void GiveScore()
    {
        if (this.hasAwardedPlayer)
            return;
        PlayerDataManager.Instance.GetPlayerScore().Score += this.score;
        this.hasAwardedPlayer = true;
    }

    private void HandleDeath()
    {
        if (!this.canDie)
            return;
        
        this.hasAi = false;

        if(this.spellCaster.GetIsCasting())
            this.spellCaster.StopCasting();
        this.spellCaster.RemoveElements();

        this.OnNPCDeath?.Invoke();

        Destroy(this.gameObject, this.timeToDespawnOnDeath);
    }

    private void HandleDamaged(float oldHealth, float newHealth)
    {
        this.detectionProgress.Value = 1.0f;
        this.Target = PlayerDataManager.Instance.GetPlayer().gameObject;
    }

    #endregion

    #region PrivateMethods - Movement

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
        // Just like in the player's code, this is a fucking hack... in this case, the update order of the NPC prefabs has blessed us and the character
        // controller's velocity value does work externally... but there is no guarantee that it will on the final build! so fuck that shit and let's do the
        // same hack yet again...
        this.Velocity = this.characterController.velocity;

        // AI independent movement
        UpdateMovementGravity(delta);

        // AI dependent movement
        if (!this.hasAi)
            return;

        // If the NPC is casting a spell, just like the player, it cannot move.
        if (this.spellCaster.GetIsCasting())
            return;

        UpdateMovementWalk(delta);

        this.Velocity = this.characterController.velocity;
    }

    #endregion

    #region PrivateMethods - Pathing

    // Pathing and AI Logical position handling

    #region Deprecated

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

    // private void UpdatePathing_OLD()
    // {
    //     // If the AI is disabled, just early return because no pathing logic should be performed at all.
    //     if (!this.hasAi)
    //         return;
    // 
    //     // Stupid fixes for Unity's NavMeshAgent being kinda weird...
    //     #region Comment - this.agent.Warp()
    //     /*
    //         This is such a fucking stupid situation that I had to make a region exclussively for this comment that is basically a wall of text...
    //         Stupid fix that exists because Unity's NavMeshAgent components seem to be allowed to move freely on their own away from their body if
    //         updatePosition is set to false...
    //         updatePosition in the documentation promises that the AI controller would not update its position allowing the user to use their own method
    //         for position updating, such as physics or a character controller, allwoing the NavMeshAgent component to be used as a logical unit for pathing...
    //         sadly this is not the case, and what it actually does is allow the logical pathing to move through the nav mesh independent of the body.
    //         Funnily enough, the origin of the GameObject changes, causing any other form of movement to spaz out.
    //         The solution in the official documentation hidden deep within layers and layers of indirection is legit, I shit you not, to warp the AI controller
    //         to the position where we want the NPC's physical representation to be located. What the fuck is this shit. Wouldn't it make more sense to just
    //         have a method to NOT update the AI's logical position and set it manually or use nextPosition? yes, yes it would. But the Unity people seem to disagree.
    // 
    //         In short: it's pretty cool that the logical position and physical position can be separated, but why can't we define the logical position to follow
    //         a GO's transform out of the box? I know it would do the same as updating it ourselves every single frame but... then why the fuck does it update and move
    //         on its own rather than taking as an input the new position?
    //     */
    //     #endregion
    //     #region Comment - NavMehs.SamplePosition() and Distance() > 2.0f
    // 
    //     // Only allow warping the logical agent position to the physical one if it is close enough to attach to a nav mesh.
    //     // This prevents any warnings from coming up. Could be discarded, but still, it's cleaner to use it.
    // 
    //     // Also only warp the NPC if the logical position has moved further than 2.0f units from the real position.
    //     // This crappy patchy solution allows Unity's built in nav mesh agent's rotation system to work and not shit its pants...
    // 
    //     #endregion
    // 
    //     NavMeshHit hit;
    //     bool hasHit = NavMesh.SamplePosition(this.characterController.transform.position, out hit, 1.0f, NavMesh.AllAreas);
    //     if (hasHit && Vector3.Distance(this.agent.nextPosition, this.transform.position) > 2.0f)
    //     {
    //         this.agent.Warp(this.characterController.transform.position);
    //     }
    // 
    //     // Only update AI / nav agent logic if the agent is within the bounds of a nav mesh.
    //     // Prevents the agent from trying to path if they are out of a nav mesh, which prevents any errors from coming up.
    //     if (this.agent.isOnNavMesh)
    //     {
    //         // Update the destination to the target position
    //         this.agent.destination = this.NavTarget;
    //     }
    // }

    #endregion

    // Updates the pathing of the nav mesh agent.
    // If the AI is enabled for this NPC, it will set the nav agent's destination to the nav target.
    private void UpdatePathing()
    {
        if (!this.hasAi)
            return;
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
}
