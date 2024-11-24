using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreboardMenuController : MonoBehaviour
{
    #region Variables

    [SerializeField] private UI_ScoreEntryController[] scoreEntries;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        FetchEntries();
    }

    void Update()
    {
        
    }

    #endregion

    #region PublicMethods

    public void Button_Return()
    {
        SceneLoadingManager.Instance?.LoadSceneMainMenu();
    }

    #endregion

    #region PrivateMethods

    private void SetEntriesEmpty(string emptyStr = "...")
    {
        for (int i = 0; i < this.scoreEntries.Length; ++i)
        {
            this.scoreEntries[i].SetData(i + 1, emptyStr, emptyStr);
        }
    }

    private void SetEntries(ScoreListDTO scores)
    {
        for (int i = 0; i < scores.scores.Length; ++i)
        {
            var name = scores.scores[i].name;
            var score = scores.scores[i].score;
            this.scoreEntries[i].SetData(i + 1, name, score);
        }
    }

    private void FetchEntries()
    {
        // Reset the entires on the screen.
        SetEntriesEmpty("...");

        // Fetch the scores from the server.
        int len = 10;
        var callbacks = new ConnectionManager.RequestCallbacks();
        callbacks.OnSuccess += (ans) => {
            DebugManager.Instance?.Log($"scores = {ans}");
            var list = JsonUtility.FromJson<ScoreListDTO>(ans);
            DebugManager.Instance.Log($"list : {list}, list.scores = {list.scores}");
            // Set the entries to empty and then fill in however many entries have been returned by the server.
            SetEntriesEmpty("N/A");
            SetEntries(list);
        };
        ConnectionManager.Instance.MakeRequestToServer("GET", $"/score/descending/slice/0/{len}", callbacks);
    }

    #endregion
}
