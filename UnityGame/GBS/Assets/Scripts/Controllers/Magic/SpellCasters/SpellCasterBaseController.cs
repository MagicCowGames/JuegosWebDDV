using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCasterBaseController : MonoBehaviour, ISpellCaster
{
    #region Variables

    // NOTE : Each spell caster is responsible for dealing with its own spawn transforms and prefabs.
    // README!!!! : The idea should be scrapped altogether. This cleaner code should be moved to the spell caster controller, and then we should just have specific functions for handling each spell type there. That way, we could also add element based form handling in the future if the project evolves like that...

    [Header("Spell Caster Base Controller")]
    [SerializeField] protected GameObject[] spellPrefabs; // NOTE : Maybe the magic manager should be the one to have a field with all of the prefabs stored so that we do not have multiple copies needlessly stored in memory?
    [SerializeField] protected Transform[] spawnTransforms;

    // protected Form form;
    protected ElementQueue elementQueue;
    protected bool isCasting;

    protected float castDuration;

    #endregion

    #region MonoBehaviour

    void Awake()
    {
        Init();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    #endregion

    #region PublicMethods
    #endregion

    #region PrivateMethods

    private void Init()
    {
        // Initialize variables to default values.
        this.elementQueue = null;
        this.isCasting = false;
        this.castDuration = 0.0f;
    }

    #endregion

    #region ProtectedMethods
    #endregion

    // TODO : Implement
    #region ISpellCaster

    public void StartCasting()
    {
        // Can't cast if the element queue is null or if it has no elements queued up, so we bail out with an early return.
        if (this.elementQueue == null || this.elementQueue.Count <= 0)
            return;

        // Update isCasting status.
        this.isCasting = true;

        HandleStartCasting();

        // After casting the spell, clear the element queue. The element queue is cleared "as soon as the casting starts" (kinda), not after it is finished, so during
        // sustained casting the player will already see the queue as empty.
        this.elementQueue.Clear();
    }
    public void StopCasting()
    {
        this.isCasting = false;
        HandleStopCasting(); // Here, specific impls for sustained spells such as beam spells will handle cleaning up their own spawned spells when they are no longer needed.
    }
    public bool GetIsCasting()
    {
        return this.isCasting;
    }

    public void SetElementQueue(ElementQueue queue)
    {
        this.elementQueue = queue;
    }
    public ElementQueue GetElementQueue()
    {
        return this.elementQueue;
    }

    public void SetElements(Element[] elements)
    {
        this.elementQueue.Add(elements); // Add the elements within the array one by one to the element queue to make sure that combinations are handled properly.
    }
    public Element[] GetElements()
    {
        return this.elementQueue.Elements;
    }
    public void AddElement(Element element)
    { }

    /*
    public void SetForm(Form form)
    {
        this.form = form;
    }
    public Form GetForm()
    {
        return this.form;
    }
    */

    public void SetCastDuration(float time)
    {
        this.castDuration = time;
    }
    public float GetCastDuration()
    {
        return this.castDuration;
    }

    #endregion

    #region ISpellCaster - Virtual

    // NOTE : These are some shitty default implementations that should never be called, as each specific type should call their own specific impl,
    // but here they are anyway, so fuck it lol.
    protected virtual void HandleStartCasting()
    {
        DebugManager.Instance?.Log("SpellCasterBaseController::HandleStartCasting()");
    }
    protected virtual void HandleStopCasting()
    {
        DebugManager.Instance?.Log("SpellCasterBaseController::HandleStopCasting()");
    }

    #endregion
}
