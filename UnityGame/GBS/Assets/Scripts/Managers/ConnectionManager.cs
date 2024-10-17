using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        
    }

    void Update()
    {
        
    }

    #endregion

    #region PublicMethods

    public Address GetServerAddress()
    {
        return this.ServerAddress;
    }

    public void SetServerAddress(string addr)
    {
        this.ServerAddress = new Address(addr);
    }

    #endregion

    #region PrivateMethods
    #endregion
}
