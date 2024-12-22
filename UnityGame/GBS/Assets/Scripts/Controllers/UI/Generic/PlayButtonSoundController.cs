using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The hackiest solution in the entire planet to a completely fucking artificial problem.
// Thanks, deadlines, you made my code be shit!
public class PlayButtonSoundController : MonoBehaviour
{
    private float click2PlayDelay = 0.05f;
    private float timeElapsed = 0.0f;

    private void Update()
    {
        float delta = Time.deltaTime;
        this.timeElapsed += delta;
    }

    public void PlayClickSound()
    {
        SoundManager.Instance?.PlaySoundUI("click");
        #if UNITY_ANDROID
        Handheld.Vibrate();
        #endif
    }

    public void PlayClick2Sound()
    {
        if (this.timeElapsed < this.click2PlayDelay)
            return;
        SoundManager.Instance?.PlaySoundUI("click2");
        this.timeElapsed = 0.0f;
    }
}
