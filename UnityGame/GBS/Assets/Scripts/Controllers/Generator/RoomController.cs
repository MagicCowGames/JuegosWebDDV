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

    public void SetWallEnabled(Direction direction, bool enabled)
    {
        switch (direction)
        {
            case Direction.Up:
                this.entryUp.gameObject.SetActive(enabled);
                break;
            case Direction.Right:
                this.entryRight.gameObject.SetActive(enabled);
                break;
            case Direction.Down:
                this.entryDown.gameObject.SetActive(enabled);
                break;
            case Direction.Left:
                this.entryLeft.gameObject.SetActive(enabled);
                break;
        }
    }

    public void RemoveWall(Direction direction)
    {
        SetWallEnabled(direction, false);
    }

    public void AddWall(Direction direction)
    {
        SetWallEnabled(direction, true);
    }

    #endregion

    #region PrivateMethods
    #endregion
}
