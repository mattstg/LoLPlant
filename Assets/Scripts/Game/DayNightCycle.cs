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
    public int totalDays = 5;

    public Vector2 sunPosition;
    public float groundToSunAngle = 0f;
    public float groundToSunMagnitude = 0f;
    public float ambientSunLevel = 1f;
    public float playerIlluminationOffset = 0f;

    public Transform sky;


    public void Initialize()
    {

    }

    public void Refresh(float dt)
    {
        if (!GV.ClockStopped)
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

        sunPosition = GV.GetRadialCoordinates(GV.GetSunRotation(normalTime), 1f, -0.5f);
        groundToSunAngle = GV.GetAngle(sunPosition);
        groundToSunMagnitude = GV.GetDistance(sunPosition);

        ambientSunLevel = Mathf.Min(Mathf.Max(sunPosition.y, 0) * (2f/3f) * 1.2f, 1f);

        sky.localPosition = new Vector3(sky.localPosition.x, GV.GetRadialCoordinates(GV.GetSunRotation(normalTime), 12.4f, 0f).y, sky.localPosition.z);

        float illumination = Mathf.Clamp(GV.GetRadialCoordinates(GV.GetSunRotation(normalTime), 2f / 3f, -0.5f).y, 0f, 1f);
        SpriteTinter.Instance.UpdateSpriteTints(1f - Mathf.Pow(illumination - 1f, 2f), playerIlluminationOffset);
    }

    public void SetTime(float _time)
    {
        time = _time;
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

    public void ToggleClockStopped()
    {
        GV.SetClockStopped(!GV.ClockStopped);
    }
}