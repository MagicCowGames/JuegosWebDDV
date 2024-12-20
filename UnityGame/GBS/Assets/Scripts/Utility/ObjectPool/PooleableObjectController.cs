using UnityEngine;

// NOTE : A very simple component controller whose purpose is to literally contain an index value for the pool that this object is
// from so that Return() can be called in O(1)

// NOTE : This component will be added to all objects that are created by an ObjectPool

public class PooleableObjectController : MonoBehaviour
{
    #region Variables

    [SerializeField] private int index;
    
    public int Index { get { return this.index; } set { this.index = value; } }

    #endregion
}
