using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO : Possibly replace this with a regular trigger when the Entity system is implemented?
// TODO : Finish implementing logic for all the other sensors, such as hearing and stuff like that.
public class AISensorController : MonoBehaviour
{
    #region Variables

    [Header("AI Controller")]
    [SerializeField] private TestDummyController aiController; // TODO : Move this logic so that it is the AI controller itself that handles this detect method stuff. This class should be just like the sensor classes and notify the AI controller through a callback or event.

    [Header("Sensors")]
    [SerializeField] private AISensorBase[] sensors;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        // Register the events for the sensors' callbacks.
        foreach (var sensor in sensors)
            sensor.OnSense += DetectEntity;
    }

    void Update()
    {
        
    }

    #endregion

    #region PublicMethods
    #endregion

    #region PrivateMethods

    // TODO : Modify the code within the sensors system to detect any kind of entity with a team system in the future. This could be done by having a bool checking
    // function within the base sensor class.
    // For now, just detect the player since all NPCs will be enemies for now.
    private void DetectEntity(GameObject obj)
    {
        this.aiController.Target = obj; // The player is instantly detected and targetted.
    }

    #endregion
}
