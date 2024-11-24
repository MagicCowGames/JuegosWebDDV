using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Yet another dumb class award... but hey, at least all this crap goes pretty good with Unity's component getting system...
// Literally the same as the fucking money controller class.
public class ScoreController : MonoBehaviour
{
    #region Variables

    [SerializeField] private int score;

    public int Score { get { return this.score; } set { this.score = value; } }

    #endregion
}
