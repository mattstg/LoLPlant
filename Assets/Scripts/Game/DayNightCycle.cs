using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public bool clockActive = true;
    private bool isDaytime = true;
    private bool stateIsDaytime = true;

    public float time = 0f;
    public float normalTime = 0f;
    public int day = 0;
    public int hour = 0;
    public int hour12 = 0;
    public bool isMorning = true;

    public Vector2 sunPosition;
    public float groundToSunAngle = 0f;
    public float ambientSunLevel = 1f;
    public float playerIlluminationOffset = 0f;

    public Transform sky;

    public static readonly float normalTimeOffset = 4f;
    public static readonly float secondsPerHour = 18.75f; //15: 4-minute daytime, 18.75: 5-minute daytime
    public static readonly int hoursPerDay = 24;
    public static readonly int sunriseHour = 4;
    public static readonly int sunsetHour = 20;

    public void Initialize(bool _clockActive = true)
    {
        clockActive = _clockActive;
    }

    public void Refresh(float dt)
    {
        if (!stateIsDaytime)
            NightUpdate(dt);
        else if (clockActive)
            DayUpdate(dt);
    }

    private void DayUpdate(float dt)
    {
        time += dt;
        UpdateDNC();
        if (isDaytime != stateIsDaytime)
            BeginNight();
    }

    private void NightUpdate(float dt)
    {

    }

    private void UpdateDNC()
    {
        normalTime = (time / secondsPerHour) + normalTimeOffset;
        day = (int)(normalTime / hoursPerDay);
        hour = (int)(normalTime % hoursPerDay);
        hour12 = (int)(normalTime % (hoursPerDay * 0.5f));
        if (hour12 == 0)
            hour12 = (int)(hoursPerDay * 0.5f);
        isMorning = (hour < hoursPerDay * 0.5f);
        float hourFloat = normalTime % hoursPerDay;
        isDaytime = (hourFloat >= sunriseHour && hourFloat < sunsetHour);

        sunPosition = GV.GetRadialCoordinates(GV.GetSunRotation(normalTime), 1f, -0.5f);
        groundToSunAngle = GV.GetAngle(sunPosition);

        ambientSunLevel = Mathf.Min(Mathf.Max(sunPosition.y, 0) * (2f / 3f) * 1.2f, 1f);

        sky.localPosition = new Vector3(sky.localPosition.x, GV.GetRadialCoordinates(GV.GetSunRotation(normalTime), 12.4f, 0f).y, sky.localPosition.z);

        float illumination = Mathf.Clamp(GV.GetRadialCoordinates(GV.GetSunRotation(normalTime), 2f / 3f, -0.5f).y, 0f, 1f);
        SpriteTinter.Instance.UpdateSpriteTints(1f - Mathf.Pow(illumination - 1f, 2f), playerIlluminationOffset);
    }

    private void BeginNight()
    {

    }

    private void BeginDay()
    {

    }

    public void SetTime(int _day, float _hourFloat)
    {
        _day = _day % 7;
        _hourFloat = _hourFloat % hoursPerDay;
        float nTime = _day * hoursPerDay + _hourFloat;
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

    public float GetSunAngle()
    {
        return GV.GetAngle(sunPosition);
    }
}
