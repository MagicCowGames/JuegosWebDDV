// This file will contain an enum for all of the different FSMs that will be used by the AI states of the different NPCs and other AI agents in the game.

// NOTE : Each FSM has multiple special states with a specific meaning:
// · None  = Invalid state whose purpose is to be used to define malformed states and other such errors, allowing the FSM to jump back to a proper and correct state.
// · COUNT = Not really a valid AI state, it's used to store the number of states that exist within the enum. Depends on the fact that the first state is 0 and
//           the rest are automatically numbered from there.

public enum AIState_Main
{
    None = 0,
    Idle,
    Wandering,
    Combat
}

public enum AI_WalkState
{
    None = 0,
    Walking,
    Standing,
    Attacking,

    COUNT
}

public enum AIState_Wandering
{
    None = 0,
    SelectingTarget, // Start Wandering
    MovingToTarget, // Wandering in progress / ongoing
    ArrivedToTarget // End Wandering
}

public enum AIState_Combat
{
    None = 0,
    Chasing,
    Fighting,
    Retreating
}
