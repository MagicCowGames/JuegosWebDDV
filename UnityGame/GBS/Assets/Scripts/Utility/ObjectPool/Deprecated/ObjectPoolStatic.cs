using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolStatic_1
{
    #region Variables

    public GameObject Prefab;
    public GameObject[] Objects;
    public int Capacity;
    public int Count;

    #endregion

    #region Constructor

    public ObjectPoolStatic_1(GameObject prefab, int capacity)
    {
        this.Count = 0;
        this.Prefab = prefab;
        this.Capacity = capacity;
        this.Objects = new GameObject[this.Capacity];
        for (int i = 0; i < this.Capacity; ++i)
        {
            this.Objects[i] = ObjectSpawner.Spawn(this.Prefab);
            this.Objects[i].SetActive(false);
        }
    }

    #endregion

    #region PublicMethods

    public GameObject Get()
    {
        GameObject ans = null;
        if (this.Count < this.Capacity)
        {
            GameObject obj = this.Objects[this.Count];
            obj.SetActive(true);
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

    #endregion

    #region PrivateMethods
    #endregion
}
