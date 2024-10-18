public struct User
{
    public long id;
    public string name;
    public int score;
    public int money;

    public User(long id = -1, string name = "None", int score = 0, int money = 0)
    {
        this.id = id;
        this.name = "None";
        this.score = 0;
        this.money = 0;
    }
}
