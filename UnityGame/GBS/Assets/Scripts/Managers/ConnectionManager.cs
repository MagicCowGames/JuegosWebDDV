using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ConnectionManager : SingletonPersistent<ConnectionManager>
{
    #region Variables

    [Header("Server Address")]
    [SerializeField] private string serverIp = "localhost";
    [SerializeField] private int serverPort = 27015;

    public Address ServerAddress { get; set; }

    #endregion

    #region MonoBehaviour

    public override void Awake()
    {
        base.Awake();

        this.ServerAddress = new Address($"{this.serverIp}:{this.serverPort}");
    }

    void Start()
    {
        SendMessageHTTP("/score/add/pedro/69");
        SendMessageHTTP("/score");

        // StartCoroutine(Request_POST("localhost:27015/users"));
    }

    void Update()
    {
        
    }

    #endregion

    #region PublicMethods - Address

    public Address GetServerAddress()
    {
        return this.ServerAddress;
    }

    public void SetServerAddress(string addr)
    {
        this.ServerAddress = new Address(addr);
    }

    #endregion

    #region PublicMethods - Messaging

    public void SendMessageHTTP(string message)
    {
        Http(message);
    }

    #endregion

    #region PublicMethods - Request Types

    public IEnumerator Request_Generic_Run(UnityWebRequest uwr, Action<string> onSuccess, Action<string> onError, Action onConnectionSuccess, Action onConnectionError)
    {
        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ConnectionError)
        {
            onConnectionError?.Invoke();
        }
        else
        {
            onConnectionSuccess?.Invoke();
            if (uwr.error == null)
            {
                onSuccess?.Invoke(uwr.downloadHandler.text);
            }
            else
            {
                onError?.Invoke(uwr.error);
            }
        }
    }

    public IEnumerator Request_GET(string url, string message, Action<string> onSuccess, Action<string> onError, Action onConnectionSuccess, Action onConnectionError)
    {
        var uwr = new UnityWebRequest(url + message, "GET", new DownloadHandlerBuffer(), new UploadHandlerRaw(new byte[0]));
        yield return Request_Generic_Run(uwr, onSuccess, onError, onConnectionSuccess, onConnectionError);
    }

    public IEnumerator Request_POST(string url, string message, Action<string> onSuccess, Action<string> onError, Action onConnectionSuccess, Action onConnectionError)
    {
        var uwr = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(message); // message is of type JSON string
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");
        yield return Request_Generic_Run(uwr, onSuccess, onError, onConnectionSuccess, onConnectionError);
    }

    #endregion

    #region PrivateMethods

    private void Http(string message)
    {
        StartCoroutine(HttpCoroutine(this.ServerAddress.http, message));
    }

    private IEnumerator HttpCoroutine(string address, string message)
    {
        UnityWebRequest webRequest = new UnityWebRequest(address + message, "GET", new DownloadHandlerBuffer(), new UploadHandlerRaw(new byte[0]));
        yield return webRequest.SendWebRequest();
        if (webRequest.error == null)
        {
            DebugManager.Instance?.Log($"HTTP RESPONSE : {webRequest.downloadHandler.text}");
        }
        else
        {
            DebugManager.Instance?.Log($"HTTP ERROR : {webRequest.error}");
        }
        webRequest.Dispose();
    }

    #endregion
}
