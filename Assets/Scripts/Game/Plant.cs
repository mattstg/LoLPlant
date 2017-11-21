using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FoodLossState {Normal, Frozen, Dropping};

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

    public FoodLossState foodLossState = FoodLossState.Normal;
    public float foodLossDelay = 2f;
    private float foodLossCounter = 0f;
    public float foodLossTarget = -1f;
    public float foodLossDamp = 0;
    private float foodLossVelocity = 0;

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
        float oldFoodDamp = foodDamp;
        foodDamp = Mathf.SmoothDamp(foodDamp, food, ref foodVelocity, dampTime * 2f);

        switch (foodLossState)
        {
            case FoodLossState.Normal:
                break;
            case FoodLossState.Frozen:
                foodLossCounter += dt;
                if (foodLossCounter >= foodLossDelay)
                {
                    foodLossCounter = foodLossDelay;
                    foodLossState = FoodLossState.Dropping;
                }
                break;
            case FoodLossState.Dropping:
                if (foodLossTarget < 0f)
                {
                    foodLossDamp = Mathf.SmoothDamp(foodLossDamp, foodDamp, ref foodLossVelocity, dampTime * 2f);
                    if (foodLossDamp < foodDamp + 0.05f)
                        foodLossState = FoodLossState.Normal;
                }
                else
                {
                    foodLossCounter += dt;
                    foodLossDamp = Mathf.SmoothDamp(foodLossDamp, foodLossTarget, ref foodLossVelocity, dampTime * 2f);
                    if (foodLossDamp < foodLossTarget + 0.1f)
                    {
                        foodLossTarget = -1;
                        foodLossState = FoodLossState.Frozen;;
                    }
                }
                break;
            default:
                break;
        }
    }

    public void UpdateHeight()
    {

    }

    public void LoseFood(float foodLost)
    {
        if (food > 0f)
        {
            food = Mathf.Max(food - foodLost, 0f);
            switch (foodLossState)
            {
                case FoodLossState.Normal:
                    foodLossState = FoodLossState.Frozen;
                    foodLossCounter = 0f;
                    foodLossTarget = -1f;
                    foodLossDamp = foodDamp;
                    break;
                case FoodLossState.Frozen:
                    foodLossCounter = 0f;
                    foodLossTarget = -1f;
                    break;
                case FoodLossState.Dropping:
                    foodLossCounter = 0f;
                    foodLossTarget = foodDamp;
                    break;
                default:
                    break;
            }
        }
    }
}
