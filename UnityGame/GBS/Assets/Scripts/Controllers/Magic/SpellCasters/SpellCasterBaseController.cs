using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCasterBaseController : MonoBehaviour, ISpellCaster
{
    #region Variables

    // NOTE : Each spell caster is responsible for dealing with its own spawn transforms and prefabs.

    [Header("Spell Caster Base Controller")]
    [SerializeField] protected GameObject[] spellPrefabs;
    [SerializeField] protected Transform[] spawnTransforms;

    protected ElementQueue elementQueue;
    private bool isCasting;

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
    }

    #endregion

    #region ProtectedMethods
    #endregion

    // TODO : Implement
    #region ISpellCaster

    public void StartCasting()
    { }
    public void StopCasting()
    { }
    public bool GetIsCasting()
    { }

    public void SetElementQueue(ElementQueue queue)
    { }
    public ElementQueue GetElementQueue()
    { }

    public void SetElements(Element[] elements)
    { }
    public Element[] GetElements()
    { }
    public void AddElement(Element element)
    { }

    public void SetForm(Form form)
    { }
    public Form GetForm()
    { }

    public void SetCastDuration(float time)
    { }
    public float GetCastDuration()
    { }

    #endregion
}
