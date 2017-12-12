using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GV {

    public static WS ws;
    public static MainScript ms;

    public static bool Sound_Active = true;
    public static readonly float PlatformSunblock = .5f;

    public static readonly float WaterDepletionRate = 0.005f;
    public static readonly float SpinnerSpeed = 2f;
    public static readonly float FoodMaximum = 60f;
    public static readonly float FoodHeightRatio = 1f;

    //Clouds (background)
    public static readonly Vector2 cloudSpeedRange = new Vector2(.3f, 1.8f);
    public static readonly Vector2 cloudAltitudeRange = new Vector2(-2f, 6f);
    public static readonly Vector2 cloudTravelRange = new Vector2(-20f, 20f); //x value is background x value past which a cloud will respawn on right; y value is background x value where cloud will respawn
    public static readonly float sortingLayerCriticalValue = 1.3f; //clouds moving faster than this speed will pass in front of mountains; slower ones will pass behind

    public static readonly float worldWidth = 40.96f; //these dimension values do not actually control the world size; they just reflect it in order to inform other stuff like shadows
    public static readonly float worldHeight = 23.04f;
    public static readonly float shadowBuffer = 20f;
    public static readonly float sunAngleBuffer = 2; 

    public static bool Paused = false;

    public static void SetPause(bool _paused)
    {
        Paused = _paused;
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
        return normalTime * (-360f / (float)DayNightCycle.hoursPerDay) - 90f;
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

    public static float WaveFactor(float progress)
    {
        return -0.5f * Mathf.Cos(2f * Mathf.PI * progress) + 0.5f;
    }

    public static float ZoomBounceIn(float progress, float peakScale = 1.2f)
    {
        float k = Mathf.Max(peakScale, 1.01f);
        float h = 1f - ((k - 1f) * (Mathf.Sqrt((1f / (k - 1f)) + 1f) - 1f));
        float a = (1f - k) / Mathf.Pow((1f - h), 2f);
        if (progress <= 0f)
            return 0f;
        else if (progress >= 1f)
            return 1f;
        else if (progress >= h)
            return Mathf.Clamp(((1f - k) / 2f) * Mathf.Cos((Mathf.PI * (progress - 1f)) / (1f - h)) + ((1f + k) / 2f), 0f, k);
        else
            return Mathf.Clamp(a * Mathf.Pow(progress - h, 2f) + k, 0f, k);
    }

    public static float ZoomBounceOut(float progress, float peakScale = 1.2f)
    {
        float k = Mathf.Max(peakScale, 1.01f);
        float h = (k - 1f) * (Mathf.Sqrt((1f / (k - 1f)) + 1f) - 1f);
        float a = (1f - k) / Mathf.Pow(h, 2f);
        if (progress <= 0f)
            return 1f;
        else if (progress >= 1f)
            return 0f;
        else if (progress <= h)
            return Mathf.Clamp(((1f - k) / 2f) * Mathf.Cos((Mathf.PI * progress) / h) + ((1f + k) / 2f), 0f, k);
        else
            return Mathf.Clamp(a * Mathf.Pow(progress - h, 2f) + k, 0f, k);
    }

    public static Vector2 RadianToVector2(float radian)
    {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }
    public static Vector2 RadianToVector2(float radian, float length)
    {
        return RadianToVector2(radian) * length;
    }
    public static Vector2 DegreeToVector2(float degree)
    {
        return RadianToVector2(degree * Mathf.Deg2Rad);
    }
    public static Vector2 DegreeToVector2(float degree, float length)
    {
        return RadianToVector2(degree * Mathf.Deg2Rad) * length;
    }

    public static void SetAlpha(Color color, float alpha)
    {
        color = new Color(color.r, color.g, color.b, alpha);
    }

    public static string GetWeekdayString(int day)
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
