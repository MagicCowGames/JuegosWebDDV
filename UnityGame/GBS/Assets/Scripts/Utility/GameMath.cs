using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameMath
{
    // Calculate the sigmoid
    public static float Sigmoid(float x)
    {
        return 1.0f / (1.0f + Mathf.Exp(-x));
    }

    // Maps value from an input range to an output range
    public static float MapValue(float value, float inputMin, float inputMax, float outputMin, float outputMax)
    {
        float inputRange = inputMax - inputMin;
        float outputRange = outputMax - outputMin;
        return (((value / inputRange) * outputRange) + outputMin);
    }
}
