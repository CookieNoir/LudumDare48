using UnityEngine;

public static class Helpers
{
    public static float SmoothStep(float x)
    {
        return x * x * (3f - x - x);
    }

    public static float JumpFunction(float x)
    {
        float value = Mathf.Cos(Mathf.PI * (x - 0.5f));
        return value * value;
    }
}