using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

// NOTE : Both the ObjectPool and the ObjectPool<T> classes are kinda like the Singleton<T> and SingletonPersistent<T> classes. We should inherit from it to make
// custom pools rather than use it as a component, but it is still possible to do so if you wish...

// NOTE : This ObjectPool implementation uses dynamic reallocation with capcity growth up to a maximum limit.
// TODO : Maybe make this a class that requires a template parameter T, so that we can have a Pool<T> which stores the components directly for faster access?
public class ObjectPool : MonoBehaviour
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
    private List<GameObject> objects;

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
    public List<GameObject> Objects { get { return this.objects; } }

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

    public GameObject Get()
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
                x.SetActive(true);
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
                x.SetActive(false);
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
        this.objects = new List<GameObject>(this.initialCount);
        for (int i = 0; i < this.initialCount; ++i)
            this.Objects[i] = SpawnObject();
    }

    private GameObject SpawnObject()
    {
        // Spawn the object and attach it to this pool's gameobject's transform
        var obj = ObjectSpawner.Spawn(this.prefab, this.transform);
        obj.transform.parent = this.transform;

        // Add object to list
        this.objects.Add(obj); // NOTE : The total count value does not need to be increased when adding the object to the objects list, as the TotalCount property returns this.objects.Count

        // Deactivate the on spawn GameObject before returning
        obj.gameObject.SetActive(false);

        // Return the spawned object
        return obj;
    }

    #endregion
}
