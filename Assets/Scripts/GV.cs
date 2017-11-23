using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GV {

    public static WS ws;
    public static MainScript ms;

    public static readonly float PlatformSunblock = .3f;

    public static readonly float NormalTimeOffset = 4f;
    public static readonly float SecondsPerHour = 5f;
    public static readonly int HoursPerDay = 24;
    public static readonly int SunriseHour = 4;
    public static readonly int SundownHour = 20;

    public static readonly float WaterDepletionRate = 0.005f;
    public static readonly float SpinnerSpeed = 2f;
    public static readonly float FoodMaximum = 60f;
    public static readonly float FoodHeightRatio = 1f;

    public static bool Paused = false;
    public static bool ClockStopped = false;

    public static void SetPause(bool _paused)
    {
        Paused = _paused;
    }

    public static void SetClockStopped(bool _clockStopped)
    {
        ClockStopped = _clockStopped;
    }

    public static float SunFactor(float sun)   // arg range: [0, 1];  return range: [0, 1]
    {
        return Mathf.Clamp(-1.1f * Mathf.Pow((sun - 1f), 2f) + 1f, 0, 1);

        //if (sun <= (11f / 60f))
        //    return 0;
        //else
        //    return Mathf.Clamp(0.5f * Mathf.Sin(1.2f * Mathf.PI * (Mathf.Clamp(sun, 0, 1) - 0.6f)) + 0.5f, 0, 1);
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

    public static float GetSunRotation(float normalTime)
    {
        return normalTime * (-360f / (float)GV.HoursPerDay) - 90f;
    }

    public static Vector2 GetRadialCoordinates(float angle, float radius, float verticalOffset)
    {
        angle = angle * 2f * Mathf.PI / 360f;
        float x = Mathf.Cos(angle) * radius;
        float y = (Mathf.Sin(angle) - verticalOffset) * radius;
        return new Vector2(x, y);
    }

    public static float GetAngle(Vector2 point)
    {
        float hypotenuse = GetDistance(point);
        float mult = 1f;
        if (point.y < 0f)
            mult = -1f;
        return Mathf.Acos(point.x / hypotenuse) * 360f / (2f * Mathf.PI) * mult;
    }

    public static float GetDistance(Vector2 point)
    {
        return Mathf.Sqrt(Mathf.Pow(point.x, 2) + Mathf.Pow(point.y, 2));
    }

    public static float BounceFactor(float progress)
    {
        return WaterFactor(progress);
        //return Mathf.Abs(Mathf.Sin(progress * Mathf.PI));
    }

    public static string GetWeekdaySting(int day)
    {
        day = day % 7;
        string weekdayString;
        switch (day)
        {
            case 0:
                weekdayString = "MONDAY";
                break;
            case 1:
                weekdayString = "TUESDAY";
                break;
            case 2:
                weekdayString = "WEDNESDAY";
                break;
            case 3:
                weekdayString = "THURSDAY";
                break;
            case 4:
                weekdayString = "FRIDAY";
                break;
            case 5:
                weekdayString = "SATURDAY";
                break;
            case 6:
                weekdayString = "SUNDAY";
                break;
            default:
                weekdayString = "";
                break;
        }
        return weekdayString;
    }
}
