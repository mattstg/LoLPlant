using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainCloud : Cloud, CastsShadow {
    GameObject raindrop;
    public Transform[] leftRightEdges;
    public Transform rainEdgeLeft;
    public Transform rainEdgeRight;
    bool raining = false;
    Vector2 rainRateRange;
    float rainRate;
    float targetSpeed; //when targetSpeed is reached, a new targetSpeed is selected randomly from within speed range, and the speed is lerped towards it, for constantly varied speed. same for rainRate.
    float targetRainRate;
    int maxDropsPerFrame;

    Vector2 speedInterpolaterRange;
    Vector2 rainRateInterpolaterRange;
    float speedInterpolater;
    float rainRateInterpolater;
    float speedLerper = 0f;
    float rainRateLerper = 0f;

    public override void Initialize()
    {
        speedRange = GV.raincloudSpeedRange;
        altitudeRange = GV.raincloudAltitudeRange;
        travelRange = GV.raincloudTravelRange;
        rainRateRange = GV.rainRateRange;
        speedInterpolaterRange = GV.raincloudSpeedInterpolaterRange;
        rainRateInterpolaterRange = GV.rainRateInterpolaterRange;
        maxDropsPerFrame = GV.maxDropsPerFrame;

        speed = Random.Range(speedRange.x, speedRange.y);
        targetSpeed = Random.Range(speedRange.x, speedRange.y);
        altitude = Random.Range(altitudeRange.x, altitudeRange.y);
        rainRate = Random.Range(rainRateRange.x, rainRateRange.y);
        targetRainRate = Random.Range(rainRateRange.x, rainRateRange.y);
        speedInterpolater = Random.Range(speedInterpolaterRange.x, speedInterpolaterRange.y);
        rainRateInterpolater = Random.Range(rainRateInterpolaterRange.x, rainRateInterpolaterRange.y);
        raindrop = GV.ws.raincloudManager.raindrop;

        this.transform.Translate(0, altitude - this.transform.localPosition.y, 0);
        if (this.transform.localPosition.x >= -1 * (GV.worldWidth / 2) && this.transform.localPosition.x <= GV.worldWidth / 2) //if cloud is over game world
            raining = true;

        GV.ws.shadowManager.RegisterShadow(this, transform);   
    }

    public override void Refresh(float dt)
    {

        if (this.transform.localPosition.x >= travelRange.x) //if still in bounds, update speed and keep moving left
        {
            RefreshSpeed();
            this.transform.Translate(-speed * dt, 0, 0);
        } 

        else if (this.transform.localPosition.x < travelRange.x) //if past left limit, respawn on right
            Reinitialize();

        if (!raining && this.transform.localPosition.x >= -1 * (GV.worldWidth / 2) && this.transform.localPosition.x <= GV.worldWidth / 2)
            raining = true;

        else if (raining && !(this.transform.localPosition.x >= -1 * (GV.worldWidth / 2) && this.transform.localPosition.x <= GV.worldWidth / 2))
            raining = false;

        if (raining)
        {
            RefreshRainRate();
            Rain();
        }
            
    }

    void RefreshSpeed()
    {
        if (speed == targetSpeed)
        {
            targetSpeed = Random.Range(speedRange.x, speedRange.y);
            speedLerper = 0;
        }
            
        else
        {
            speed = Mathf.Lerp(speed, targetSpeed, speedLerper);
            speedLerper += speedInterpolater;
        }
    }

    void RefreshRainRate()
    {
        if (rainRate == targetRainRate)
        {
            targetRainRate = Random.Range(rainRateRange.x, rainRateRange.y);
            rainRateLerper = 0;
        }

        else
        {
            rainRate = Mathf.Lerp(rainRate, targetRainRate, rainRateLerper);
            rainRateLerper += rainRateInterpolater;
        }
    }

    void Rain()
    {
        
        for(int i = 0; i < maxDropsPerFrame; i++)
        {
            if (Random.Range(0, (int)rainRate) == 0)
            {
                Vector2 spawnPos = new Vector2(Random.Range(rainEdgeLeft.position.x, rainEdgeRight.position.x), rainEdgeLeft.position.y);

                Instantiate(raindrop, spawnPos, Quaternion.identity);
            }
        }
        
    }

    public override void Reinitialize() //called when a cloud passes far enough past the left edge of the background to 'respawn' on the right
    {
        this.transform.Translate(2 * travelRange.y, 0, 0);
        speed = Random.Range(speedRange.x, speedRange.y);
        altitude = Random.Range(altitudeRange.x, altitudeRange.y);
        this.transform.Translate(0, altitude - this.transform.localPosition.y, 0);
        speedInterpolater = Random.Range(speedInterpolaterRange.x, speedInterpolaterRange.y);
        rainRateInterpolater = Random.Range(rainRateInterpolaterRange.x, rainRateInterpolaterRange.y);
    }


    /////////////////////////////////////////////

    public Vector2[] RetrieveShadowEdges()
    {
        return new Vector2[] { leftRightEdges[0].transform.position, leftRightEdges[1].transform.position };
    }
}
