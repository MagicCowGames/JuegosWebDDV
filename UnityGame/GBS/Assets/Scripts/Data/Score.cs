using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ScoreDTO
{
    public string name;
    public long score;
}

[System.Serializable]
public struct ScoreListDTO
{
    public ScoreDTO[] scores;
}
