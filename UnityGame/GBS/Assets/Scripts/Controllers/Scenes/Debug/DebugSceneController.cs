using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugSceneController : MonoBehaviour
{
    #region Variables

    [SerializeField] private ObjectPoolController objectPool;
    [SerializeField] private float timeToDespawn = 2.0f;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    #endregion

    #region PublicMethods

    public void SpawnThing()
    {
        var obj = this.objectPool.Get();
        if (obj != null)
            StartCoroutine(DespawnThingCoroutine(obj));
    }

    #endregion

    #region PrivateMethods

    private IEnumerator DespawnThingCoroutine(GameObject obj)
    {
        yield return new WaitForSeconds(this.timeToDespawn);
        this.objectPool.Return(obj);
    }

    #endregion
}
