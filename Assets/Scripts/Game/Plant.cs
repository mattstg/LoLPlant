using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum FoodLossState {Normal, Frozen, Dropping};

public class Plant : MonoBehaviour {

    public int shadowCount = 0;
    public float shadowFactor = 1f;
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
    public int heightDampInt = 0;
    private float heightVelocity = 0;
    public bool isGrowing = false;
    public float growthTime;

    public float highScore = 0;
    
    public float dampTime = 0.25f;
    
    public void Initialize(float initialHeight)
    {
        height = initialHeight;
        growthTime = 2f;
    }

    public virtual void Refresh(float dt)
    {
        UpdateSun(dt);
        UpdateWater(dt);

        //force 0 shadows and 50% water, to determine max possible food production:
        //sun = sunDamp = GV.ws.dnc.ambientSunLevel;
        //sunFactor = GV.SunFactor(sun);
        //water = waterDamp = 0.5f;
        //waterFactor = GV.WaterFactor(water);

        UpdatePhotosynthesis(dt);
        UpdatePsProgress(dt);
        UpdateFood(dt);
        UpdateHeight(dt);
    }

    public void UpdateSun(float dt)
    {
        //sun = Mathf.Clamp(sun, 0, 1);
        if (shadowCount < 0)
            shadowCount = 0;
        shadowFactor = Mathf.Clamp01(Mathf.Pow(GV.PlatformSunblock, shadowCount)); //numOfShadowsBlocking
        sun = Mathf.Clamp(GV.ws.dnc.ambientSunLevel * shadowFactor, 0, 1);
        //sun = GV.ws.dnc.ambientSunLevel * (2f/3f);
        sunFactor = GV.SunFactor(sun);
        sunDamp = Mathf.SmoothDamp(sunDamp, sun, ref sunVelocity, dampTime, Mathf.Infinity, dt);
    }

    public void UpdateWater(float dt)
    {
        water -= (photosynthesis + sun) * GV.WaterDepletionRate * dt;
        water = Mathf.Clamp(water, 0, 1);
        waterFactor = GV.WaterFactor(water);
        waterDamp = Mathf.SmoothDamp(waterDamp, water, ref waterVelocity, dampTime, Mathf.Infinity, dt);
    }

    public void BeginGrowthSequence()
    {
        float totalTimeToGrow = DayNightCycle.lengthOfNight;
        //when finished send event: GrowthSequenceDone
    }

    public void UpdatePhotosynthesis(float dt)
    {
        photosynthesis = sunFactor * waterFactor;
        psDamp = Mathf.SmoothDamp(psDamp, photosynthesis, ref psVelocity, dampTime, Mathf.Infinity, dt);
    }

    public void UpdatePsProgress(float dt)
    {
        psProgress += photosynthesis * dt;
        psProgressDamp = Mathf.SmoothDamp(psProgressDamp, psProgress, ref psProgressVelocity, dampTime * 2f, Mathf.Infinity, dt);
    }

    public void UpdateFood(float dt)
    {
        if (!isGrowing)
            food += photosynthesis * dt;
        float foodDampTime = (isGrowing) ? growthTime : dampTime * 2f;
        foodDamp = Mathf.SmoothDamp(foodDamp, food, ref foodVelocity, foodDampTime, Mathf.Infinity, dt);
        if (Mathf.Abs(foodDamp - food) < 0.01f)
        {
            foodDamp = food;
            foodVelocity = 0f;
        }

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
                    foodLossDamp = Mathf.SmoothDamp(foodLossDamp, foodDamp, ref foodLossVelocity, foodDampTime, Mathf.Infinity, dt);
                    if (foodLossDamp < foodDamp + 0.05f)
                        foodLossState = FoodLossState.Normal;
                }
                else
                {
                    foodLossCounter += dt;
                    foodLossDamp = Mathf.SmoothDamp(foodLossDamp, foodLossTarget, ref foodLossVelocity, foodDampTime, Mathf.Infinity, dt);
                    if (foodLossDamp < foodLossTarget + 0.1f)
                    {
                        foodLossTarget = -1;
                        foodLossState = FoodLossState.Frozen;
                    }
                }
                break;
            default:
                break;
        }
    }

    public void UpdateHeight(float dt)
    {
        if (isGrowing)
        {
            if (Mathf.Abs(heightDamp - height) < 0.5f)
            {
                isGrowing = false;
                heightDamp = height;
                heightDampInt = (int)heightDamp;
                heightVelocity = 0f;
            }
            else
            {
                heightDamp = Mathf.SmoothDamp(heightDamp, height, ref heightVelocity, growthTime);
                heightDampInt = (int)heightDamp;
            }
        }
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

    public void ConvertFoodToHeight()
    {
        if (food > 0)
        {
            float score = (food);
            int scoreInt = (int)score;
            if (scoreInt > highScore)
                highScore = scoreInt;
            height += score;
            food = 0f;
            isGrowing = true;
        }
    }
}
