﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GV {

    public static WS ws;
    public static MainScript ms;

    public static readonly Vector2 playerDrag = new Vector2(.4f, .1f); //the x value is player's linear drag when not grounded; y value is drag when grounded
    public static readonly float defaultTutorialHour = 10;
    public static bool Sound_Active = true;
    public static readonly float SpinnerSpeed = 2f;

    public static readonly float worldWidth = 40.96f; //these dimension values do not actually control the world size; they just reflect it in order to inform other stuff like shadows
    public static readonly float worldHeight = 23.04f;

    //Player Controls
    public static readonly float YDiffToActivate = .27f;

	public static readonly float TimeBetweenAphidBites = 0.75f;
    //Plant
    public static readonly float SunReductionPerPlatform = .8f;
    public static readonly float WaterPerDrop = .0175f;
    public static readonly float WaterDepletionRate = 0.012f;
    public static readonly float FoodRate = 10f;
    public static readonly float FoodMaximum = 1500f;
    public static readonly float GrowthPerFood = 0.1f;
    public static readonly float HeightMaximum = 1000f;

    //Clouds (background)
    public static readonly Vector2 cloudSpeedRange = new Vector2(.075f, 0.3f);
    public static readonly Vector2 cloudAltitudeRange = new Vector2(-1f, 11f);
    public static readonly Vector2 tutCloudAltitudeRange = new Vector2(-1.75f, 2f);
    public static readonly Vector2 cloudTravelRange = new Vector2(-20f, 20f); //x value is background x value past which a cloud will respawn on right; y value is background x value where cloud will respawn

    public static readonly Vector2 cloudSingleScreenAltitudeRange = new Vector2(0.5f, 5f);
    public static readonly Vector2 cloudSingleScreenTravelRange = new Vector2(-13f, 13f); //x value is background x value past which a cloud will respawn on right; y value is background x value where cloud will respawn

    public static readonly float sortingLayerCriticalValue = .27f; //clouds moving faster than this speed will pass in front of mountains; slower ones will pass behind

    //Rainclouds
    public static readonly Vector2 raincloudSpeedRange = new Vector2(.2f,1.2f);
    public static readonly Vector2 raincloudAltitudeRange = new Vector2(9f, 13f);
    public static readonly Vector2 raincloudTravelRange = new Vector2(-40f, 40f);

	//raincloud sound
	public static readonly float rainHearingDist = worldWidth / 4;
	public static readonly float[] worldRange = {-8,7} ;

    public static readonly Vector2 rainRateRange = new Vector2(8, 20); //(10,34); //int >= 1. rainRate will lerp around randomly to values in this range. 
    public static readonly int maxDropsPerFrame = 5; // this many 'rain dice' will be rolled each frame, with each die having 1/rainRate chance of spawning a drop

    public static readonly Vector2 raincloudSpeedInterpolaterRange = new Vector2(.001f, .006f) ;
    public static readonly Vector2 rainRateInterpolaterRange = new Vector2(.0005f, .004f);

    //Shadows
    public static readonly float shadowBuffer = 15f;
    public static readonly float sunAngleBuffer = 2;
    public static readonly float raincloudShadowAlpha = .4f;

    public static readonly int LastTutorialProgressPoint = 5;    

	public static readonly float[] platformSpriteScales = { 0.4f, 0.5f, 0.8f, 1.0f, 1.2f, 1.4f, 1.5f};
	public static readonly float platformHeight = 0.07f;

    //Aphids
    public static readonly float AphidKnockbackVelo = 5; //If an aphid hits you, your velo is set to this away from it
    public static readonly float AphidKnockbackVeloReduced = 3; //If an aphid hits you and is above you (you jumped up into it), your velo is set to this instead
    public static readonly float FoodLostPerAphid = 50f;


    public static Sprite[] platformSprites;
    
    //see Raindrop.OnTriggerEnter2D(): waterDelta is weighted so that rain is worth more when you're lower on water and less when you're higher.
    //Current setting: At 0% water, the value of a drop is equal to waterPerDrop. At 100% water, a drop's value would be equal to (waterPerDrop / 3).

    public static Sprite GetPlatformSprite(int index)
    {
        if(platformSprites == null)
            platformSprites = Resources.LoadAll<Sprite>("Sprites/platform_sheet");
        return platformSprites[index];
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
        return LangDict.Instance.GetText("Day" + (day % 7));
    }

	public static float[] GetSpriteAndEdge(float scale){
		return GetSpriteAndEdge (scale, 3, 0);
	}

	public static float[] GetSpriteAndEdge(float scale, int i, int c){
		if(i > platformSpriteScales.Length || i < 0 || c > 3)
			return new float[] {1,3}; // default (size 1) plank
		float dif = scale - platformSpriteScales [i];
		if (Mathf.Abs(dif) < 0.01f)
			return new float[] { i, platformSpriteScales [i] };
		return GetSpriteAndEdge (scale, i + (int) (Mathf.Abs (dif) / dif), c + 1);
	}

    public static float SmoothVelocity(float progress)
    {
        if (progress >= 0 && progress <= 1)
            return -4 * Mathf.Abs(progress - 0.5f) + 2;
        else
            return 0;
    }

    public static float SmoothIntegral(float progress)
    {
        if (progress < 0)
            return 0;
        else if (progress > 1)
            return 1;
        else if (progress <= 0.5)
            return progress * SmoothVelocity(progress) / 2;
        else
            return 0.5f + ((progress - 0.5f) * (SmoothVelocity(progress) + 2) / 2);
    }
}
