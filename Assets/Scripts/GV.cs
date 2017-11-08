using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GV {

    public static float SunFactor(float sun)   // arg range: [0, 1];  return range: [0, 1]
    {
        return Mathf.Clamp((27f / 25f) * Mathf.Pow(Mathf.Clamp(sun, 0, 1), 2) - 0.08f, 0, 1);
    }
    public static float AirFactor(float air)
    {
        return Mathf.Clamp(-(Mathf.Pow((Mathf.Clamp(air, 0, 1) - 1), 2)) + 1, 0, 1);
    }
    public static float WaterFactor(float water)
    {
        water = Mathf.Clamp(water, 0, 1);
        if (water < 1f / 3f)
            return -0.5f * Mathf.Cos(water * 3 * Mathf.PI) + 0.5f;
        else if (water > 2f / 3f)
            return -0.5f * Mathf.Cos(water * 3 * Mathf.PI - 3) + 0.5f;
        else return 1;
    }
}
