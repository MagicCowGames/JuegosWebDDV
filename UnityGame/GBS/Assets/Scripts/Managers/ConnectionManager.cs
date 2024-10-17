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
