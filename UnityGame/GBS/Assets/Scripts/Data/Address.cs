[System.Serializable]
public struct Address
{
    public string address;
    public string http;
    public string https;
    public string ws;

    public Address(string addr)
    {
        this.address = addr;
        this.http = $"http://{addr}";
        this.https = $"https://{addr}";
        this.ws = $"ws://{addr}";
    }
}
