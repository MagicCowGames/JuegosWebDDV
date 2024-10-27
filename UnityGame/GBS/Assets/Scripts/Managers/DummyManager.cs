using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyManager : SingletonPersistent<DummyManager>
{
    void Start()
    {
        // This is not where this should be, but idk what class I should put global config in so here it goes lol...
        // This gives an error on web when running on a PC, but it doesn't matter cause it just does nothing so that's fine for now.
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }

    void Update()
    {
    }
}
