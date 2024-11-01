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

    public void LogInternal(string str)
    {
        if (this.debugEnabled)
            Debug.Log(str);
    }

    public void Log(string str)
    {
        if (!this.debugEnabled)
            return;
        var p = $"[Debug] : {str}";
        UIManager.Instance?.GetConsoleUI().CmdPrintln(p);
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

    // NOTE : The only reason this fucking mess exists is literally to allow debug drawing on the final release build to be enabled through the console.
    // That's all there is to it.

    public void DrawLine(Vector3 start, Vector3 end, Color color, float width = 0.1f)
    {
        if (!this.debugEnabled)
            return;

        Vector3[] points = {start, end};
        DrawSegment(points, false, color, width);
    }

    public void DrawSegment(Vector3[] points, bool loop, Color color, float width = 0.1f)
    {
        if (!this.debugEnabled)
            return;

        var lineRenderer = this.GetLineRenderer();

        lineRenderer.lineRenderer.positionCount = points.Length;
        lineRenderer.lineRenderer.SetPositions(points);
        lineRenderer.lineRenderer.startColor = color;
        lineRenderer.lineRenderer.endColor = color;
        lineRenderer.lineRenderer.loop = loop;

        lineRenderer.lineRenderer.startWidth = width;
        lineRenderer.lineRenderer.endWidth = width;
    }

    public void DrawSphere(Vector3 origin, float radius, Color color, float width = 0.1f)
    {
        if (!this.debugEnabled)
            return;

        Vector3[] points1 = {
            new Vector3(0, -1, 0),
            new Vector3(0.707107f, -0.707107f, 0),
            new Vector3(1, 0, 0),
            new Vector3(0.707107f, 0.707107f, 0),
            new Vector3(0, 1, 0),
            new Vector3(-0.707107f, 0.707107f, 0),
            new Vector3(-1, 0, 0),
            new Vector3(-0.707107f, -0.707107f, 0)
        };

        Vector3[] points2 = {
            new Vector3(0, -1, 0),
            new Vector3(0, -0.707107f, 0.707107f),
            new Vector3(0, 0, 1),
            new Vector3(0, 0.707107f, 0.707107f),
            new Vector3(0, 1, 0),
            new Vector3(0, 0.707107f, -0.707107f),
            new Vector3(0, 0, -1),
            new Vector3(0, -0.707107f, -0.707107f)
        };

        Vector3[] points3 = {
            new Vector3(1, 0, 0),
            new Vector3(0.707107f, 0, 0.707107f),
            new Vector3(0, 0, 1),
            new Vector3(-0.707107f, 0, 0.707107f),
            new Vector3(-1, 0, 0),
            new Vector3(-0.707107f, 0, -0.707107f),
            new Vector3(0, 0, -1),
            new Vector3(0.707107f, 0, -0.707107f)
        };

        for (int i = 0; i < points1.Length; ++i)
            points1[i] = origin + points1[i] * radius;


        for (int i = 0; i < points2.Length; ++i)
            points2[i] = origin + points2[i] * radius;

        for (int i = 0; i < points3.Length; ++i)
            points3[i] = origin + points3[i] * radius;

        DrawSegment(points1, true, color, width);
        DrawSegment(points2, true, color, width);
        DrawSegment(points3, true, color, width);
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
