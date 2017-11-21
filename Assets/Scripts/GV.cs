using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GV {

    public static WS ws;
    public static MainScript ms;

    public static readonly float PlatformSunblock = .3f;

    public static readonly float NormalTimeOffset = 4f;
    public static readonly float SecondsPerHour = 2f;
    public static readonly int HoursPerDay = 24;
    public static readonly int SunriseHour = 4;
    public static readonly int SundownHour = 20;

    public static readonly float WaterDepletionRate = 0.01f;
    public static readonly float SpinnerSpeed = 2f;
    public static readonly float FoodMaximum = 60f;
    public static readonly float FoodHeightRatio = 1f;

    public static float SunFactor(float sun)   // arg range: [0, 1];  return range: [0, 1]
    {
        if (sun <= (11f / 60f))
            return 0;
        else
            return Mathf.Clamp(0.5f * Mathf.Sin(1.2f * Mathf.PI * (Mathf.Clamp(sun, 0, 1) - 0.6f)) + 0.5f, 0, 1);
        //return Mathf.Clamp((27f / 25f) * Mathf.Pow(Mathf.Clamp(sun, 0, 1), 2) - 0.08f, 0, 1);
    }

    public static float WaterFactor(float water)
    {
        water = Mathf.Clamp(water, 0, 1);
        return 4 * water - 4 * Mathf.Pow(water, 2);
        //return Mathf.Clamp(Mathf.Sqrt(1 - Mathf.Pow(2 * water - 1, 2)), 0, 1);
    }

    public static float GetMinimalRotation(float source, float target)
    {
        float delta = target - source;
        while (delta > 180)
            delta -= 360;
        while (delta < -180)
            delta += 360;
        return delta;
    }

    public static float NormalizeAngle180(float angle) // [-180 - +180]
    {
        while (angle > 180)
            angle -= 360;
        while (angle < -180)
            angle += 360;
        return angle;
    }

    public static float NormalizeAngle360(float angle) // [0 - +360[
    {
        while (angle >= 360)
            angle -= 360;
        while (angle < 0)
            angle += 360;
        return angle;
    }
}
