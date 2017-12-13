using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public bool clockActive = true;

    public float time = 0f;
    public float normalTime = 0f;
    public int day = 0;
    public int hour = 0;
    public int hour12 = 0;
    public bool isMorning = true;
    public int totalDays = 5;

    public Vector2 sunPosition;
    public float groundToSunAngle = 0f;
    public float groundToSunMagnitude = 0f;
    public float ambientSunLevel = 1f;
    public float playerIlluminationOffset = 0f;

    public Transform sky;

    public static readonly float normalTimeOffset = 4f;
    public static readonly float secondsPerHour = 15f;
    public static readonly int hoursPerDay = 24;
    public static readonly int sunriseHour = 4;
    public static readonly int sundownHour = 20;

    public void Initialize(bool _clockActive = true)
    {
        clockActive = _clockActive;
    }

    public void Refresh(float dt)
    {
        if (clockActive)
        {
            time += dt;
            UpdateDNC();
        }
    }

    public void UpdateDNC()
    {
        normalTime = (time / secondsPerHour) + normalTimeOffset;
        day = (int)(normalTime / hoursPerDay);
        hour = (int)(normalTime % hoursPerDay);
        hour12 = (int)(normalTime % (hoursPerDay * 0.5f));
        if (hour12 == 0)
            hour12 = (int)(hoursPerDay * 0.5f);
        isMorning = (hour < hoursPerDay * 0.5f);

        sunPosition = GV.GetRadialCoordinates(GV.GetSunRotation(normalTime), 1f, -0.5f);
        groundToSunAngle = GV.GetAngle(sunPosition);
        groundToSunMagnitude = GV.GetDistance(sunPosition);

        ambientSunLevel = Mathf.Min(Mathf.Max(sunPosition.y, 0) * (2f / 3f) * 1.2f, 1f);

        sky.localPosition = new Vector3(sky.localPosition.x, GV.GetRadialCoordinates(GV.GetSunRotation(normalTime), 12.4f, 0f).y, sky.localPosition.z);

        float illumination = Mathf.Clamp(GV.GetRadialCoordinates(GV.GetSunRotation(normalTime), 2f / 3f, -0.5f).y, 0f, 1f);
        SpriteTinter.Instance.UpdateSpriteTints(1f - Mathf.Pow(illumination - 1f, 2f), playerIlluminationOffset);
    }

    public void SetTime(int _day, int _hour)
    {
        _day = _day % 7;
        _hour = _hour % hoursPerDay;
        float nTime = _day * hoursPerDay + _hour;
        time = (nTime - normalTimeOffset) * secondsPerHour;
        UpdateDNC();
    }

    public void TogglePause()
    {
        GV.SetPause(!GV.Paused);
        if (GV.Paused)
        {
            if (GV.ws.dm.sunControl)
                GV.ws.dm.sunControl.interactable = false;
            if (GV.ws.dm.waterControl)
                GV.ws.dm.waterControl.interactable = false;
        }
        else
        {
            if (GV.ws.dm.sunControl)
                GV.ws.dm.sunControl.interactable = true;
            if (GV.ws.dm.waterControl)
                GV.ws.dm.waterControl.interactable = true;
        }
    }

    public void ToggleClockActive()
    {
        SetClockActive(!clockActive);
    }

    public void SetClockActive(bool _clockActive)
    {
        clockActive = _clockActive;
    }
}