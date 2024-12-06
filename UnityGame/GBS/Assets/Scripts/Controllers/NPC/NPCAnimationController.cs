using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NOTE : This is a temporary solution, in the future it would be ideal to merge this with the Entity animation system I was planning, but deadlines are are
// fucking my plans in the asshole as much as humanly possible...

// TODO : Get rid of this class and try to figure out how to merge this in a cleaner way with the Entity animation system that I was making... same with the player
// animation controller... or maybe just keep the player and NPC impls as separate things, who knows what will be better in the long run!

public class NPCAnimationController : MonoBehaviour
{
    #region Variables

    [SerializeField] private CharacterController characterController;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        
    }

    void Update()
    {
        DebugManager.Instance?.Log($"velocity = {characterController.velocity}");
    }

    #endregion

    #region PublicMethods
    #endregion

    #region PrivateMethods
    #endregion
}
