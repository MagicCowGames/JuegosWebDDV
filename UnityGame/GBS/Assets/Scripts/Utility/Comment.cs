using UnityEngine;

public class Comment : MonoBehaviour
{
#if UNITY_EDITOR

    #region Variables

    [SerializeField][TextArea] private string comment;

    #endregion

#endif
}
