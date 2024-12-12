using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    #region Variables

    [SerializeField] private Transform roomRoot;
    [SerializeField] private GameObject entryUp;
    [SerializeField] private GameObject entryRight;
    [SerializeField] private GameObject entryDown;
    [SerializeField] private GameObject entryLeft;

    private GameObject originalEntryUp;
    private GameObject originalEntryRight;
    private GameObject originalEntryDown;
    private GameObject originalEntryLeft;

    private Direction faceDirection = Direction.Up;

    #endregion

    #region MonoBehaviour

    void Awake()
    {
        this.originalEntryUp = this.entryUp;
        this.originalEntryRight = this.entryRight;
        this.originalEntryDown = this.entryDown;
        this.originalEntryLeft = this.entryLeft;
    }

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
                this.entryUp.gameObject?.SetActive(enabled);
                break;
            case Direction.Right:
                this.entryRight.gameObject?.SetActive(enabled);
                break;
            case Direction.Down:
                this.entryDown.gameObject?.SetActive(enabled);
                break;
            case Direction.Left:
                this.entryLeft.gameObject?.SetActive(enabled);
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

    public void SetFaceDirection(Direction direction)
    {
        this.faceDirection = direction;

        ResetRotation();

        int timesToRotate = 0;

        switch (direction)
        {
            default:
            case Direction.Up:
                // Do nothing, we're already facing up after the reset rotation call
                timesToRotate = 0;
                break;
            case Direction.Right:
                timesToRotate = 1;
                break;
            case Direction.Down:
                timesToRotate = 2;
                break;
            case Direction.Left:
                timesToRotate = 3;
                break;
        }

        Rotate90DegreesRight(timesToRotate);
    }

    #endregion

    #region PrivateMethods

    private void Rotate90DegreesRight(int times = 1)
    {
        for (int i = 0; i < times; ++i)
        {
            this.roomRoot.transform.Rotate(new Vector3(0.0f, 90.0f, 0.0f));

            var u = this.entryUp;
            var r = this.entryRight;
            var d = this.entryDown;
            var l = this.entryLeft;

            this.entryUp = l;
            this.entryRight = u;
            this.entryDown = r;
            this.entryLeft = d;
        }
    }

    private void ResetRotation()
    {
        this.roomRoot.rotation = Quaternion.identity;
        this.entryUp = this.originalEntryUp;
        this.entryRight = this.originalEntryRight;
        this.entryDown = this.originalEntryDown;
        this.entryLeft = this.originalEntryLeft;
    }

    #endregion
}
