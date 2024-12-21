using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

// NOTE : This is a deprecated backup copy of the object pool controller script, saved just in case.

// NOTE : This ObjectPool implementation uses dynamic reallocation with capcity growth up to a maximum limit.
// TODO : Maybe make this a class that requires a template parameter T, so that we can have a Pool<T> which stores the components directly for faster access?
public class ObjectPoolControllerDeprecated : MonoBehaviour
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

    // NOTE : This implementation has been discarded because Unity has a weird issue with timings for index based operations despite being single threaded.
    // TODO : Figure out what the fuck is wrong and fix it...
    #region PublicMethods - IdxBased
    /*
    public GameObject GetIdxBased()
    {
        // If all objects in the pool are occupied, return null
        if (this.activeCount >= this.maxCount)
            return null;

        // Spawn a new object if the capacity is filled
        if (this.activeCount >= this.objects.Count)
            SpawnObject();

        // Pick the first inactive object, activate it and return it
        var obj = this.objects[this.activeCount];
        obj.gameObject.SetActive(true);

        // Increase the active count
        ++this.activeCount;

        return obj;
    }

    // NOTE : This system can be severily misused if the user returns an object with a pooleable component that does not belong to this pool...
    // To solve this problem, we could have each pooleable object contain a value that says from which pool is it that they are from... or never let the user handle that crap by hand themselves.
    public void ReturnIdxBased(GameObject obj)
    {
        Debug.Log("Return() has been called");

        var pooleable = obj.GetComponent<PooleableObjectController>();
        if (pooleable == null)
            return; // Cannot return the obejct to the pool because it is not a pooleable object.
        int idx = pooleable.Index;

        Debug.Log("Return() has been called and has taken effect");
        this.objects[idx].gameObject.SetActive(false);
        --this.activeCount;

        Swap(this.activeCount, idx);
    }
    */

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

    // Discarded method that uses the pooleable object controller with index thing...
    private GameObject SpawnObjectOld()
    {
        // Spawn the object
        var obj = ObjectSpawner.Spawn(this.prefab, this.transform);
        obj.transform.parent = this.transform;

        // Add pooleable object controller component
        var pooleable = obj.AddComponent<PooleableObjectController>();
        pooleable.Index = this.objects.Count;

        // Add object to list
        this.objects.Add(obj); // NOTE : The total count value is increased when adding the object to the objects list.

        // Deactivate the on spawn GameObject before returning
        obj.gameObject.SetActive(false);

        // Return the spawned object
        return obj;
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

    // A simple method to swap 2 objects within the pooled objects list. Used when activating and reactivating objects. Weird, but makes things O(1) lolololo
    private void Swap(int idx1, int idx2)
    {
        // If both indices are the same, we don't need to do anything
        if (idx1 == idx2)
            return;

        // Swap the objects within this list
        var temp = this.objects[idx1];
        this.objects[idx1] = this.objects[idx2];
        this.objects[idx2] = temp;

        // Swap the indices within the pooleable object controllers
        this.objects[idx1].GetComponent<PooleableObjectController>().Index = idx2;
        this.objects[idx2].GetComponent<PooleableObjectController>().Index = idx1;
    }

    #endregion
}
