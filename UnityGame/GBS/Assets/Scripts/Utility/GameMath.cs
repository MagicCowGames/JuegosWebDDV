using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameMath
{
    public static float Sigmoid(float x)
    {
        return 1.0f / (1.0f + Mathf.Exp(-x));
    }
}
