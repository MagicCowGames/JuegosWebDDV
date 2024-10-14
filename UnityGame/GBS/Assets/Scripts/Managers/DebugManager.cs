using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DebugManager : Singleton<DebugManager>
{
    #region Variables

    [SerializeField] private bool debugEnabled = false;
    [SerializeField] private GameObject debugLineObject;
    
    
    private List<DebugLineRendererController> lineRenderers;
    private int lineRenderersUsed;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        // Init the list of line renderers and set the counters to 0
        this.lineRenderers = new List<DebugLineRendererController>();
        this.lineRenderersUsed = 0;
    }

    void Update()
    {

    }

    void LateUpdate()
    {
        if (!this.debugEnabled)
            return;

        DisableLineRenderers();
    }

    #endregion

    #region PublicMethods

    public void Log(string str)
    {
        if(this.debugEnabled)
            Debug.Log($"[Debug] : {str}");
    }

    public void Run(Action function)
    {
        if(this.debugEnabled)
            function?.Invoke();
    }

    public void SetDebugEnabled(bool value)
    {
        this.debugEnabled = value;
        this.DisableLineRenderers();
    }

    public bool GetDebugEnabled()
    {
        return this.debugEnabled;
    }

    #endregion

    #region PublicMethods - Drawing

    public void DrawLine(Color color, Vector3 start, Vector3 end)
    {
        if (!this.debugEnabled)
            return;

        var lineRenderer = this.GetLineRenderer();

        Vector3[] positions = { start, end };
        lineRenderer.lineRenderer.SetPositions(positions);
        lineRenderer.lineRenderer.startColor = color;
        lineRenderer.lineRenderer.endColor = color;

        Debug.Log("WTF");
    }

    #endregion

    #region PrivateMethods

    private void DisableLineRenderers()
    {
        foreach (var lineRenderer in this.lineRenderers)
            if (!lineRenderer.usedLastFrame)
                lineRenderer.gameObject.SetActive(false);

        foreach (var lineRenderer in this.lineRenderers)
            lineRenderer.usedLastFrame = false;

        this.lineRenderersUsed = 0;
    }

    public void AddLineRenderer()
    {
        var obj = ObjectSpawner.Spawn(this.debugLineObject);
        obj.transform.parent = this.transform;
        var lineRenderer = obj.GetComponent<DebugLineRendererController>();
        this.lineRenderers.Add(lineRenderer);
    }

    private DebugLineRendererController GetLineRenderer()
    {
        if (this.lineRenderersUsed >= this.lineRenderers.Count)
            AddLineRenderer();

        var ans = this.lineRenderers[this.lineRenderersUsed];
        ans.usedLastFrame = true;
        ++this.lineRenderersUsed;

        ans.gameObject.SetActive(true);

        return ans;
    }

    #endregion
}
