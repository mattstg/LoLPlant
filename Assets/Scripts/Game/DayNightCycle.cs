using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{

    public float time = 0f;
    public float normalTime = 0f;
    public int day = 0;
    public int hour = 0;
    public int hour12 = 0;
    public bool isMorning = true;

    public Vector2 sunPosition;
    public float groundToSunAngle = 0f;
    public float groundToSunMagnitude = 0f;
    public RectTransform sun;
    public RectTransform line;


    public void Initialize()
    {

    }

    public void Refresh(float dt)
    {
        time += dt;
        normalTime = (time / GV.SecondsPerHour) + GV.NormalTimeOffset;
        day = (int)(normalTime / GV.HoursPerDay);
        hour = (int)(normalTime % GV.HoursPerDay);
        hour12 = (int)(normalTime % (GV.HoursPerDay * 0.5f));
        if (hour12 == 0)
            hour12 = (int)(GV.HoursPerDay * 0.5f);
        if (hour < GV.HoursPerDay * 0.5f)
            isMorning = true;
        else
            isMorning = false;

        sunPosition = GetRadialCoordinates(GetSunRotation(), 45.5f, -0.5f);
        groundToSunAngle = GetAngle(sunPosition);
        groundToSunMagnitude = GetDistance(sunPosition);
        float lineScale = groundToSunMagnitude * (2f / (3f * 45.5f));

        sun.localPosition = sunPosition;
        line.eulerAngles = new Vector3(line.eulerAngles.x, line.eulerAngles.y, groundToSunAngle - 90f);
        line.localScale = new Vector3(line.localScale.x, lineScale, line.localScale.z);
    }

    public float GetSunRotation()
    {
        return normalTime * (-360f / (float)GV.HoursPerDay) - 90f;
    }

    public Vector2 GetRadialCoordinates(float angle, float radius, float verticalOffset)
    {
        angle = angle * 2f * Mathf.PI / 360f;
        float x = Mathf.Cos(angle) * radius;
        float y = (Mathf.Sin(angle) - verticalOffset) * radius;
        return new Vector2(x, y);
    }

    public float GetAngle(Vector2 point)
    {
        float hypotenuse = GetDistance(point);
        float mult = 1f;
        if (point.y < 0f)
            mult = -1f;
        return Mathf.Acos(point.x / hypotenuse) * 360f / (2f * Mathf.PI) * mult;
    }

    public float GetDistance(Vector2 point)
    {
        return Mathf.Sqrt(Mathf.Pow(point.x, 2) + Mathf.Pow(point.y, 2));
    }
}
