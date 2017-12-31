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
    public int foodScore = -1;
    public int oldFoodScore = -2;
    
    public FoodLossState foodLossState = FoodLossState.Normal;
    private float foodLossDelay = 2f;
    private float foodLossCounter = 0f;
    private float foodLossTarget = -1f;
    public float foodLossDamp = 0;
    private float foodLossVelocity = 0;

    private float _height = 0;
    public float height { set { _height = value; UpdateHeightGraphic(); } get { return _height; } }
    public int heightInt { get { return (int)_height; } }
    public int oldHeightInt = -1;

    public bool isGrowing = false;
    private float growthDuration = 8f;
    private float growthProgress = 0f;
    private float sourceFood = 0f;
    private float targetFood = 1f;
    private float sourceHeight = 0f;
    private float targetHeight = 1f;

    public int newScore = 0;

    private float startHeight = 0; //Used to setup plant start height before player initialized is called and graphics are ready.
    
    public float dampTime = 0.25f;
    
    public void Initialize(int initialHeight)
    {
        height = initialHeight;
        water = .5f;
        Debug.Log("water is now: " + water);
    }

    public virtual void Refresh(float dt)
    {
        UpdateSun(dt);
        UpdateWater(dt);
        UpdatePhotosynthesis(dt);
        UpdatePsProgress(dt);
        UpdateFood(dt);
        UpdateHeight(dt);
    }

    public void UpdateSun(float dt)
    {
        if (shadowCount < 0)
            shadowCount = 0;
        shadowFactor = Mathf.Clamp01(Mathf.Pow(GV.SunReductionPerPlatform, shadowCount));
        sun = Mathf.Clamp(GV.ws.dnc.ambientSunLevel * shadowFactor, 0, 1);
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

    public void UpdatePhotosynthesis(float dt)
    {
        photosynthesis = sunFactor * waterFactor;
        // to measure max food production / growth per day:
        //photosynthesis = GV.SunFactor(Mathf.Clamp01(GV.ws.dnc.ambientSunLevel));
        psDamp = Mathf.SmoothDamp(psDamp, photosynthesis, ref psVelocity, dampTime, Mathf.Infinity, dt);
    }

    public void UpdatePsProgress(float dt)
    {
        psProgress += photosynthesis * dt;
        psProgressDamp = Mathf.SmoothDamp(psProgressDamp, psProgress, ref psProgressVelocity, dampTime * 2f, Mathf.Infinity, dt);
    }

    public void SetFoodTutorial(float _newValue)
    { //Used by TADelegate, tutorial gives you a set amt of food so you have some growth, so it doesn't allow you to reduce it
      //DO NOT USE THIS OUTSIDE TAEVENTMANAGER TUTORIAL
      if(food < _newValue)
            food = _newValue;
    }

    public void UpdateFood(float dt)
    {
        float foodDampTime = dampTime * 2f;

        if (!isGrowing)
        {
            food += photosynthesis * GV.FoodRate * dt;
            foodDamp = Mathf.SmoothDamp(foodDamp, food, ref foodVelocity, foodDampTime, Mathf.Infinity, dt);
            if (Mathf.Abs(foodDamp - food) < 0.01f)
            {
                foodDamp = food;
                foodVelocity = 0f;
            }
        }
        oldFoodScore = foodScore;
        foodScore = (int)foodDamp;

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
            growthProgress += dt;

            if (growthProgress >= growthDuration)
            {
                growthProgress = growthDuration;
                food = targetFood;
                height = targetHeight;
                isGrowing = false;
                //ProgressTracker.Instance.SetMaxHeight((int)height); //redundant; already done in TAEventManager, cases 6/7
                //TAEventManager.Instance.ReceiveActionTrigger("GrowthComplete"); //already sent by DNC
            }
            else
            {
                float integral = GV.SmoothIntegral(growthProgress / growthDuration);
                food = foodDamp = targetFood * integral + sourceFood * (1f - integral);
                oldHeightInt = heightInt;
                height = targetHeight * integral + sourceHeight * (1f - integral);
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

    public void BeginGrowing()
    {
        growthDuration = DayNightCycle.growthDuration;
        growthProgress = 0f;
        sourceFood = food;
        targetFood = 0f;
        float score = (food * GV.GrowthPerFood);
        sourceHeight = height;
        targetHeight = height + score;
        newScore = (int)score;
        isGrowing = true;
    }

	private void UpdateHeightGraphic()
	{
        if (GV.ws.pc != null)
        {
            GV.ws.pc.UpdateHeight(height);
        }
        Vector3 offset = GV.ws.cameraManager.offset;
        offset.y = Mathf.Max(1.1f * (height - 50f) / 950f, 0f);
        GV.ws.cameraManager.offset = offset;
    }
}
