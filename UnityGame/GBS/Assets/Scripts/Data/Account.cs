using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Account
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
    public int Score { get; set; }
    public int Money { get; set; }
    public bool IsLoggedIn { get; set; }

    public Account()
    {
        this.Name = "None";
        this.Password = "None";
        this.Score = 0;
        this.Money = 0;
        this.IsLoggedIn = false;
    }
}
