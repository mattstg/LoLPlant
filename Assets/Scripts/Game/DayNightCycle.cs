﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public bool clockActive = true;
    private bool isDaytime = true;
    private bool stateIsDaytime = true;

    public static float time = 0f;
    public float normalTime = 0f;
    public int day = 0;
    public int hour = 0;
    public int hour12 = 0;
    public float hourFloat = 0;
    public bool isMorning = true;

    public Vector2 sunPosition;
    public float groundToSunAngle = 0f;
    public float ambientSunLevel = 1f;
    public float playerIlluminationOffset = 0f;

    public Transform sky;

    public static readonly float secondsPerHour = 15f; //15: 4-minute daytime, 18.75: 5-minute daytime
    public static readonly int hoursPerDay = 24;
    public static readonly int sunriseHour = 4;
    public static readonly int sunsetHour = 20;
    public static readonly float growthDuration = 8f;

    private bool isGrowing = false;
    private float growthProgress = 0f;
    private float sourceTime;
    private float targetTime;

    private bool isZooming = false;
    private float zoomDuration = 2f;
    private float zoomProgress = 0f;
    private float zoom;
    private float originalZoom;
    private float finalZoom;
    private float sourceZoom;
    private float targetZoom;
    private float sourcePIO;
    private float targetPIO;

    private bool isJumping = false;
    private float jumpDuration = 2f;
    private float jumpProgress = 0f;
    private float sourceJumpTime;
    private float targetJumpTime;

    public void Initialize(bool _clockActive = true)
    {
        clockActive = _clockActive;
        zoom = originalZoom = Camera.main.orthographicSize;
        finalZoom = originalZoom * 0.7f;
        playerIlluminationOffset = 0f;
    }

    public void Refresh(float dt)
    {
        if (stateIsDaytime)
            DayUpdate(dt);
        else
            NightUpdate(dt);
    }

    private void DayUpdate(float dt)
    {
        if (clockActive && !isGrowing && !isJumping)
            time += dt;
        bool updatedDNC = UpdateJump(dt);
        if(!updatedDNC)
            UpdateDNC();
        if (!isDaytime && stateIsDaytime && !isJumping)
            BeginNight();
    }

    private void NightUpdate(float dt)
    {
        if (isJumping)
            UpdateJump(dt);

        if (isZooming)
        {
            zoomProgress += dt;
            if (zoomProgress >= zoomDuration)
            {
                isZooming = false;
                zoomProgress = zoomDuration;
                zoom = Camera.main.orthographicSize = targetZoom;
                playerIlluminationOffset = targetPIO;
                float cameraOffsetX = targetPIO * 1.2f;
                //float cameraOffsetY = targetPIO * 0.5f;
                Vector3 offset = GV.ws.cameraManager.offset;
                offset.x = cameraOffsetX;
                //offset.y = cameraOffsetY;
                GV.ws.cameraManager.offset = offset;
                float illumination = Mathf.Clamp01(GV.GetRadialCoordinates(GV.GetSunRotation(normalTime), 2f / 3f, -0.5f).y);
                SpriteTinter.Instance.UpdateSpriteTints(1f - Mathf.Pow(illumination - 1f, 2f), playerIlluminationOffset);
                TAEventManager.Instance.ReceiveActionTrigger("ZoomComplete");
            }
            else
            {
                float integral = GV.SmoothIntegral(zoomProgress / zoomDuration);
                zoom = targetZoom * integral + sourceZoom * (1f - integral);
                Camera.main.orthographicSize = zoom;
                playerIlluminationOffset = targetPIO * integral + sourcePIO * (1f - integral);
                float cameraOffsetX = playerIlluminationOffset * 1.2f;      //too lazy to make a new independent set of transition variables for camera offset
                //float cameraOffsetY = playerIlluminationOffset * 0.5f;      //works well though because playerIlluminationOffset always moves back and forth between 0 and 1
                Vector3 offset = GV.ws.cameraManager.offset;
                offset.x = cameraOffsetX;
                //offset.y = cameraOffsetY;
                GV.ws.cameraManager.offset = offset;
                float illumination = Mathf.Clamp01(GV.GetRadialCoordinates(GV.GetSunRotation(normalTime), 2f / 3f, -0.5f).y);
                SpriteTinter.Instance.UpdateSpriteTints(1f - Mathf.Pow(illumination - 1f, 2f), playerIlluminationOffset);
            }

        }

        if (isGrowing)
        {
            growthProgress += dt;

            if (growthProgress >= growthDuration)
            {
                isGrowing = false;
                growthProgress = growthDuration;
                time = targetTime;
                UpdateDNC();
                TAEventManager.Instance.ReceiveActionTrigger("GrowthComplete");
            }
            else
            {
                float integral = GV.SmoothIntegral(growthProgress / growthDuration);
                time = targetTime * integral + sourceTime * (1f - integral);
                UpdateDNC();
            }
        }
    }

    private void UpdateDNC()
    {
        UpdateTimeData();

        sunPosition = GV.GetRadialCoordinates(GV.GetSunRotation(normalTime), 1f, -0.5f);
        groundToSunAngle = GV.GetAngle(sunPosition);

        ambientSunLevel = Mathf.Clamp01(sunPosition.y * (2f / 3f) * 1.2f);

        sky.localPosition = new Vector3(sky.localPosition.x, GV.GetRadialCoordinates(GV.GetSunRotation(normalTime), 12.4f, 0f).y, sky.localPosition.z);

        float illumination = Mathf.Clamp01(GV.GetRadialCoordinates(GV.GetSunRotation(normalTime), 2f / 3f, -0.5f).y);
        SpriteTinter.Instance.UpdateSpriteTints(1f - Mathf.Pow(illumination - 1f, 2f), playerIlluminationOffset);
    }

    public void UpdateTimeData()
    {
        normalTime = (time / secondsPerHour) + sunriseHour;
        day = (int)(normalTime / hoursPerDay);
        hour = (int)(normalTime % hoursPerDay);
        hour12 = (int)(normalTime % (hoursPerDay * 0.5f));
        if (hour12 == 0)
            hour12 = (int)(hoursPerDay * 0.5f);
        isMorning = (hour < hoursPerDay * 0.5f);
        hourFloat = normalTime % hoursPerDay;
        isDaytime = (hourFloat >= sunriseHour && hourFloat < sunsetHour);
    }

    private bool UpdateJump(float dt)
    {
        bool updatedDNC = false;
        if (isJumping)
        {
            jumpProgress += dt;

            if (isGrowing)
            {
                isJumping = false;
                jumpProgress = jumpDuration;
            }
            else if (jumpProgress >= jumpDuration)
            {
                isJumping = false;
                jumpProgress = jumpDuration;
                time = targetJumpTime;
                UpdateDNC();
                updatedDNC = true;
                TAEventManager.Instance.ReceiveActionTrigger("JumpComplete");
            }
            else
            {
                float integral = GV.SmoothIntegral(jumpProgress / jumpDuration);
                time = targetJumpTime * integral + sourceJumpTime * (1f - integral);
                UpdateDNC();
                updatedDNC = true;
            }
        }
        return updatedDNC;
    }

    private void BeginNight()
    {
        stateIsDaytime = false;
        TAEventManager.Instance.ReceiveActionTrigger("BeginNight");
    }

    public void BeginDay()
    {
        stateIsDaytime = true;
        TAEventManager.Instance.ReceiveActionTrigger("BeginDay");
    }

    public void BeginZoomIn()
    {
        isZooming = true;
        sourcePIO = 0f;
        targetPIO = 1f;
        zoom = sourceZoom = originalZoom;
        targetZoom = finalZoom;
        zoomProgress = 0f;
    }

    public void BeginZoomOut()
    {
        isZooming = true;
        sourcePIO = 1f;
        targetPIO = 0f;
        zoom = sourceZoom = finalZoom;
        targetZoom = originalZoom;
        zoomProgress = 0f;
    }

    public void BeginGrowing()
    {
        isGrowing = true;
        sourceTime = time;
        targetTime = GetTime(day + 1, sunriseHour);
        growthProgress = 0f;
    }

    public void BeginJumping(float _targetTime)
    {
        isJumping = true;
        sourceJumpTime = time;
        targetJumpTime = _targetTime;
        jumpProgress = 0f;
    }

    public void JumpToMorning()
    {
        BeginJumping(GetTime(day + ((hourFloat <= 6f) ? 0 : 1), 6f));
    }

    public void JumpToSunset()
    {
        BeginJumping(GetTime(day + ((hourFloat <= sunsetHour) ? 0 : 1), sunsetHour));
    }

    public void JumpToPreSunset()
    {
        BeginJumping(GetTime(day + ((hourFloat <= sunsetHour) ? 0 : 1), sunsetHour) - 16f);
    }

    public void SetTime(int _day, float _hourFloat)
    {
        time = GetTime(_day, _hourFloat);
        UpdateDNC();

        if (!isDaytime && stateIsDaytime)
            BeginNight();
    }

    public float GetTime(int _day, float _hourFloat)
    {
        _day = _day % 7;
        _hourFloat = _hourFloat % hoursPerDay;
        float nTime = _day * hoursPerDay + _hourFloat;
        float _time = (nTime - sunriseHour) * secondsPerHour;
        return _time;
    }

    public float GetNextSunset()
    {
        int nextSunsetDay = day + ((hourFloat > sunsetHour) ? 1 : 0);
        return GetTime(nextSunsetDay, sunsetHour);
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

    public bool GetStateIsDaytime()
    {
        return stateIsDaytime;
    }

    public bool GrowingOrJumpingOrZooming()
    {
        return isGrowing || isJumping || isZooming;
    }
}
