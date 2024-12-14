using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The hackiest solution in the entire planet to a completely fucking artificial problem.
// Thanks, deadlines, you made my code be shit!
public class PlayButtonSoundController : MonoBehaviour
{
    public void PlayClickSound()
    {
        SoundManager.Instance?.PlaySoundUI("click");
    }
}
