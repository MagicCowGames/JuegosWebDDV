using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NOTE : This ObjectPool implementation uses dynamic reallocation with capcity growth up to a maximum limit.
public class ObjectPool : MonoBehaviour
{
    #region Variables

    [SerializeField] private GameObject prefab;
    [SerializeField] private int initialCount;
    [SerializeField] private int maxCount;

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
    public List<GameObject> Objects { get { return this.Objects; } }

    #endregion

    #region MonoBehaviour

    void Awake()
    {
        Init();
    }

    #endregion

    #region PublicMethods
    /* TODO : Implement
    public GameObject Get()
    {
        GameObject ans = null;
        if (this.Count < this.Capacity)
        {
            GameObject obj = this.Objects[this.Count];
            obj.SetActive(true);
            ++this.Count;
        }
        else
        if (this.Count < this.MaxCapacity)
        {
            var obj = ObjectSpawner.Spawn(this.Prefab);
            obj.SetActive(true);
            this.Objects.Add(obj);
            ++this.Count;
        }
        return ans;
    }

    // TODO : Replace the GameObject class with a custom PooleableGameObject class or something like that, and make it so that that class just inherits from the base
    // GameObject class and maybe adds some pool-local ID integer value so that the Return() function can be O(1)
    public void Return(GameObject obj)
    {
        for (int i = 0; i < this.Count; ++i)
        {
            if (this.Objects[i] == obj)
            {
                var temp = this.Objects[this.Count - 1];
                this.Objects[this.Count - 1] = this.Objects[i];
                this.Objects[i] = temp;
                this.Objects[i].SetActive(false);
                --this.Count;
            }
        }
    }
    */

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
        // Spawn the object
        var obj = ObjectSpawner.Spawn(this.prefab, this.transform);
        obj.transform.parent = this.transform;

        // Add pooleable object controller component
        var pooleable = obj.AddComponent<PooleableObjectController>();
        pooleable.Index = this.objects.Count;

        // Add object to list
        this.objects.Add(obj); // NOTE : The total count value is increased when adding the object to the objects list.

        // Return the spawned object
        return obj;
    }

    #endregion
}
