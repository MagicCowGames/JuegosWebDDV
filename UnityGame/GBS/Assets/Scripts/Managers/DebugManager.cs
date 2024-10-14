using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : Singleton<DebugManager>
{
    #region Variables
    
    [SerializeField] private bool debugEnabled = false;
    [SerializeField] private LineRenderer lineRenderer;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        this.UpdateDebugComponents(); // Update all debug components on start
    }

    void Update()
    {

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
        this.UpdateDebugComponents();
    }

    public bool GetDebugEnabled()
    {
        return this.debugEnabled;
    }

    public void DrawLine(Color color, Vector3 start, Vector3 end)
    {
        if (this.debugEnabled && this.lineRenderer != null)
        {
            Vector3[] positions = { start, end };
            this.lineRenderer.SetPositions(positions);
            this.lineRenderer.startColor = color;
            this.lineRenderer.endColor = color;
        }
    }

    #endregion

    #region PrivateMethods

    private void UpdateDebugComponents()
    {
        this.lineRenderer.gameObject.SetActive(this.debugEnabled);
    }

    #endregion
}
