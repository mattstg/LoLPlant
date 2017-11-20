using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour {

    public float sun = 0;
    public float sunFactor = 0;
    public float sunDamp = 0;
    private float sunVelocity = 0;

    public float water = 0;
    public float waterFactor = 0;
    public float waterDamp = 0;
    private float waterVelocity = 0;

    public float photosynthesis = 0;
    public float psDamp = 0;
    private float psVelocity = 0;

    public float psProgress = 0;
    public float psProgressDamp = 0;
    public float psProgressVelocity = 0;

    public float food = 0;
    public float foodDamp = 0;
    private float foodVelocity = 0;

    public float height = 0;
    public float heightDamp = 0;
    private float heightVelocity = 0;

    public float dampTime = 0.25f;

    public void Initialize()
    {

    }

    public void Refresh(float dt)
    {
        UpdateSun();
        UpdateWater(dt);
        UpdatePhotosynthesis();
        UpdatePsProgress(dt);
        UpdateFood(dt);
        UpdateHeight();
    }

    public void UpdateSun()
    {
        sun = Mathf.Clamp(sun, 0, 1);
        sunFactor = GV.SunFactor(sun);
        sunDamp = Mathf.SmoothDamp(sunDamp, sun, ref sunVelocity, dampTime);
    }

    public void UpdateWater(float dt)
    {
        water -= (photosynthesis + sun) * GV.WaterDepletionRate * dt;
        water = Mathf.Clamp(water, 0, 1);
        waterFactor = GV.WaterFactor(water);
        waterDamp = Mathf.SmoothDamp(waterDamp, water, ref waterVelocity, dampTime);
    }

    public void UpdatePhotosynthesis()
    {
        photosynthesis = sunFactor * waterFactor;
        psDamp = Mathf.SmoothDamp(psDamp, photosynthesis, ref psVelocity, dampTime);
    }

    public void UpdatePsProgress(float dt)
    {
        psProgress += photosynthesis * dt;
        psProgressDamp = Mathf.SmoothDamp(psProgressDamp, psProgress, ref psProgressVelocity, dampTime * 2f);
    }

    public void UpdateFood(float dt)
    {
        food += photosynthesis * dt;
        foodDamp = Mathf.SmoothDamp(foodDamp, food, ref foodVelocity, dampTime * 2f);
    }

    public void UpdateHeight()
    {

    }

}
