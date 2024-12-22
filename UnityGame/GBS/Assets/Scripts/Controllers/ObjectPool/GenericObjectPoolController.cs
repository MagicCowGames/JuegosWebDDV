using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NOTE : This ObjectPool implementation uses dynamic reallocation with capcity growth up to a maximum limit. This version of the class takes a template parameter
// T which allows pooling by referencing a component monobehaviour script, which caches it and makes it have faster access than using a GameObject pool and calling
// GetComponent<T>() every single time we want to access a gameobject. Each has its own use tbh, just pick the one that makes most sense.

// TODO : Altough... a thing that could be done in the future would be always using this one and using some kind of Pooleable Object controller with references to all
// needed components of the gameobject or something.

public class GenericObjectPool<T> : MonoBehaviour where T : Component
{
    #region Variables

    [Header("Object Pool Configuration")]
    [SerializeField] private GameObject prefab;
    [SerializeField] private int initialCount;
    [SerializeField] private int maxCount;

    #if UNITY_EDITOR
    
    [Header("Current Object Data")]
    [SerializeField] private int activeObjectCount;
    [SerializeField] private int totalObjectCount;
    
    #endif

    private int activeCount;
    private List<T> objects;

    #endregion

    #region Properties

    // Prefab
    public GameObject Prefab { get { return this.prefab; } set { this.prefab = value; } }

    // Counts
    public int InitialCount { get { return this.initialCount; } set { this.initialCount = value; } }
    public int MaxCount { get { return this.maxCount; } set { this.maxCount = value; } }
    public int ActiveCount { get { return this.activeCount; } }
    public int TotalCount { get { return this.objects.Count; } }

    // GameObjects list
    public List<T> Objects { get { return this.objects; } }

    #endregion

    #region MonoBehaviour

    void Awake()
    {
        Init();
    }

    #if UNITY_EDITOR

    void Update()
    {
        // Update the display values on the inspector during runtime.
        // A crappy and hacky way to make readonly parameters without making a custom Editor...
        // NOTE : These hacky readonly fields are only displayed and present on editor builds. Final builds discard them with the #if macros.

        if (this.objects != null)
        {
            this.activeObjectCount = this.activeCount;
            this.totalObjectCount = this.TotalCount;
        }
        else
        {
            this.activeObjectCount = 0;
            this.totalObjectCount = 0;
        }
    }

    #endif

#endregion

    #region PublicMethods

    public T Get()
    {
        if (this.activeCount >= this.objects.Count && this.activeCount < this.maxCount)
        {
            var obj = SpawnObject();
            obj.gameObject.SetActive(true);
            ++this.activeCount;
            return obj;
        }

        foreach (var x in this.objects)
        {
            if (!x.gameObject.activeSelf)
            {
                x.gameObject.SetActive(true);
                ++this.activeCount;
                return x;
            }
        }

        return null;
    }

    public void Return(GameObject obj)
    {
        foreach (var x in this.objects)
        {
            if (x == obj)
            {
                x.gameObject.SetActive(false);
                --this.activeCount;
                return;
            }
        }
    }

    #endregion

    #region PrivateMethods

    private void Init()
    {
        this.activeCount = 0;
        this.objects = new List<T>(this.initialCount);
        for (int i = 0; i < this.initialCount; ++i)
            this.Objects[i] = SpawnObject();
    }

    // Discarded method that uses the pooleable object controller with index thing...
    private T SpawnObject()
    {
        // Spawn the object
        var obj = ObjectSpawner.Spawn(this.prefab, this.transform);
        obj.transform.parent = this.transform;

        // Get the component reference
        var component = obj.GetComponent<T>();

        // Add object to list
        this.objects.Add(component); // NOTE : The total count value is increased when adding the object to the objects list.

        // Deactivate the on spawn GameObject before returning
        obj.gameObject.SetActive(false);

        // Return the spawned object by its component reference (we can access the GameObject with component.gameObject later on)
        return component;
    }

    #endregion
}
