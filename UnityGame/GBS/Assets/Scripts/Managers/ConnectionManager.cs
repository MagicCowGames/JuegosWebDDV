using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ConnectionManager : SingletonPersistent<ConnectionManager>
{
    #region Structs - DTO

    // These structs are used as Data Transfer Objects (DTOs) to get the address table from the GET petition when the game is booted.

    public struct AddressTableEntryDTO
    {
        public string name { get; set; }
        public string ip { get; set; }
        public int port { get; set; }

        public AddressTableEntryDTO(string name = "default", string ip = "localhost", int port = 27015)
        {
            this.name = name;
            this.ip = ip;
            this.port = port;
        }
    }

    public struct AddressTableDTO
    {
        public List<AddressTableEntryDTO> addresses { get; set; }

        public AddressTableDTO(List<AddressTableEntryDTO> list = null)
        {
            this.addresses = list;
        }
    }

    #endregion

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

    [Header("Server Address - Custom")]
    [SerializeField] private string serverIp = "localhost";
    [SerializeField] private int serverPort = 27015;

    [Header("Server Address - Fetch")]
    [SerializeField] private string fetchLocation = "https://raw.githubusercontent.com/MagicCowGames/MagicCowGamesFiles/refs/heads/main/data/addresses.json";

    [Header("Server Address - Configuration")]
    [SerializeField] private bool fetchAddress = true; // Determines if the address is to be fetched or not. Useful during testing and / or when programming without an internet connection.

    public Address ServerAddress { get; set; }

    #endregion

    #region MonoBehaviour

    // WARNING!!! Ugly code ahead!!! Please kill me...
    public override void Awake()
    {
        // Call base class' awake method
        base.Awake();

        // Assign the server address to the custom / "fallback" values
        this.ServerAddress = new Address($"{this.serverIp}:{this.serverPort}");

        // Get the web server's address from the address files on github and assign it to the ServerAddress object.
        // If the operation fails to fetch the server addresses table, then we run with whatever address was manually configured.
        // Yes, a patchy as fuck solution, but this solves the problem until I can get a fucking domain of my own.
        // This is also a workaround to the facts that:
        // 1) We cannot access the site's ip through JS using location without dirty tricks on Unity's side, even for WebGL builds.
        // 2) We cannot guarantee that the site will have the same address as the server. For example, what happens when the game is hosted on itch.io? The web server is not on the same machine!
        if (this.fetchAddress)
        {
            MakeRequest("GET", this.fetchLocation, new RequestCallbacks(
                (ans) =>
                {
                    DebugManager.Instance?.Log($"OnSuccess : {ans}");

                    string json = ans;


                    string fetchIp = "";
                    int fetchPort = 0;
                    this.ServerAddress = new Address($"{fetchIp}:{fetchPort}");
                })
            );
        }
    }

    void Start()
    {
        // TODO : Clean up these test calls and remove them lol
        // SendMessageHTTP("/score/add/pedro/69");
        // SendMessageHTTP("/score");

        // StartCoroutine(Request_POST("localhost:27015/users"));

        /*
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
        */
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

    // This shit's pretty fucking dumb, but it exists to make GET requests less of a pain in the arse when we're
    // just passing through a string with both the base address and whatever resource we're trying to access.
    // Also, allows making POST requests with an empty body without having to write an empty string for the message.
    // Again, this shit's pretty fucking dumb, but whatever, I'm adding it because why not...
    // tbh, I doubt I'll use it much, but it is what it is.
    public void MakeRequest(string type, string urlMessage, RequestCallbacks callbacks = null)
    {
        MakeRequest(type, urlMessage, "", callbacks);
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
