using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ConnectionManager : SingletonPersistent<ConnectionManager>
{
    #region Classes

    public class RequestCallbacks
    {
        public Action<string> OnSuccess { get; set; }
        public Action<string> OnError { get; set; }
        public Action OnConnectionSuccess { get; set; }
        public Action OnConnectionError { get; set; }

        public RequestCallbacks(Action<string> onSuccess = null, Action<string> onError = null, Action onConnectionSuccess = null, Action onConnectionError = null)
        {
            this.OnSuccess = onSuccess;
            this.OnError = onError;
            this.OnConnectionSuccess = onConnectionSuccess;
            this.OnConnectionError = onConnectionError;
        }
    }

    #endregion

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
        // SendMessageHTTP("/score/add/pedro/69");
        // SendMessageHTTP("/score");

        // StartCoroutine(Request_POST("localhost:27015/users"));

        MakeRequest("GET", "localhost:27015", "/users", new RequestCallbacks(
            (ans) => {
                DebugManager.Instance?.Log($"OnSuccess : {ans}");
            },
            (err) => {
                DebugManager.Instance?.Log($"OnError : {err}");
            },
            () => {
                DebugManager.Instance?.Log("OnConnectSuccess");
            },
            () => {
                DebugManager.Instance?.Log("OnConnectError");
            }));
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

    #region PublicMethods - Make Request

    public void MakeRequest(string type, string url, string message, RequestCallbacks callbacks = null)
    {
        StartCoroutine(MakeRequestInternal(type, url, message, callbacks));
    }

    #endregion

    #region PrivateMethods - Make Request

    private IEnumerator MakeRequestInternal(string type, string url, string message, RequestCallbacks callbacks)
    {
        switch (type)
        {
            case "GET":
                yield return Request_GET(url, message, callbacks);
                break;
            case "POST":
                yield return Request_POST(url, message, callbacks);
                break;
            default:
                break;
        }
    }

    #endregion

    #region PrivateMethods - Request Types

    private IEnumerator Request_Generic_Run(UnityWebRequest uwr, RequestCallbacks callbacks)
    {
        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ConnectionError)
        {
            callbacks?.OnConnectionError?.Invoke();
        }
        else
        {
            callbacks?.OnConnectionSuccess?.Invoke();
            if (uwr.error == null)
            {
                callbacks?.OnSuccess?.Invoke(uwr.downloadHandler.text);
            }
            else
            {
                callbacks?.OnError?.Invoke(uwr.error);
            }
        }
    }

    private IEnumerator Request_GET(string url, string message, RequestCallbacks callbacks)
    {
        var uwr = new UnityWebRequest(url + message, "GET", new DownloadHandlerBuffer(), new UploadHandlerRaw(new byte[0]));
        yield return Request_Generic_Run(uwr, callbacks);
    }

    private IEnumerator Request_POST(string url, string message, RequestCallbacks callbacks)
    {
        var uwr = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(message); // message is of type JSON string
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");
        yield return Request_Generic_Run(uwr, callbacks);
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
