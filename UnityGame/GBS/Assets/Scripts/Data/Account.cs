using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Old implementation. It was made with C#'s JsonSerializer in mind, which requires public {get;set;} properties for serialization, but since Unity does not
// work with that, I'm using the other implementation. Could keep both around and make the new one a DTO, but that's just fucking pointless tbh.
/*
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
*/

[System.Serializable]
public class Account
{
    public long id;
    public string name;
    public string password;
    public int score;
    public int money;

    public bool isLoggedIn;

    public Settings settings;

    public Account()
    {
        this.id = -1;
        this.name = "None";
        this.password = "None";
        this.score = 0;
        this.money = 0;
        this.isLoggedIn = false;
        this.settings = new Settings();
    }
}
