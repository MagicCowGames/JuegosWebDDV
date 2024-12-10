using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    #region Variables

    [SerializeField] private GameObject entryUp;
    [SerializeField] private GameObject entryRight;
    [SerializeField] private GameObject entryDown;
    [SerializeField] private GameObject entryLeft;

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

    public void RemoveWall(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                Destroy(this.entryUp.gameObject);
                break;
            case Direction.Right:
                Destroy(this.entryRight.gameObject);
                break;
            case Direction.Down:
                Destroy(this.entryDown.gameObject);
                break;
            case Direction.Left:
                Destroy(this.entryLeft.gameObject);
                break;
        }
    }

    #endregion

    #region PrivateMethods
    #endregion
}
