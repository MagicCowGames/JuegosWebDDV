using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO : Possibly replace this with a regular trigger when the Entity system is implemented?
// TODO : Finish implementing logic for all the other sensors, such as hearing and stuff like that.
// TODO : Modify the logic on each specific sensor so that it would maybe use an interface rather than a base class, and also create some IDetectableComponent so that
// The NPCs can detect more stuff appart from the player, like other NPCs, money, etc... also the reactions to the things that the sensors detect should be more easily
// accessible from the behaviour component classes...
public class AISensorSystemController : MonoBehaviour
{
    #region Variables

    [Header("AI Controller")]
    [SerializeField] private NPCController aiController; // TODO : Move this logic so that it is the AI controller itself that handles this detect method stuff. This class should be just like the sensor classes and notify the AI controller through a callback or event.

    [Header("Sensors")]
    [SerializeField] private AISensorBase[] sensors;

    private float sensorDecay = 10.0f;
    private float decayTime = 0.5f; // The time it takes after the last detection has taken place for the detection value to start decaying.
    private float elapsedTime = 0.0f; // This measures the time since last detection.

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
        float delta = Time.deltaTime;
        bool canDecay = this.aiController.detectionProgress.Value < 1.0f && this.elapsedTime >= this.decayTime; // Once we detect, we can't decay detection.
        if (canDecay)
            this.aiController.detectionProgress.Value = Mathf.Clamp01(this.aiController.detectionProgress.Value - (this.sensorDecay * delta));
        this.elapsedTime += delta;
    }

    #endregion

    #region PublicMethods
    #endregion

    #region PrivateMethods

    // TODO : Modify the code within the sensors system to detect any kind of entity with a team system in the future. This could be done by having a bool checking
    // function within the base sensor class.
    // For now, just detect the player since all NPCs will be enemies for now.
    private void DetectEntity(GameObject obj, float detectionAmount)
    {
        this.elapsedTime = 0.0f; // Prevent the sensor from lowering the detection progress if an entity has been sensed.
        this.aiController.detectionProgress.Value = Mathf.Clamp01(this.aiController.detectionProgress.Value + detectionAmount);
        if (this.aiController.detectionProgress.Value >= 1.0f)
        {
            this.aiController.Target = obj;
        }
    }

    #endregion
}
