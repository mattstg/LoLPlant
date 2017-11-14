using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeterTest : MonoBehaviour
{

    public float sun = 0;
    public float air = 0;
    public float water = 0;
    public float sunFactor = 0;
    public float airFactor = 0;
    public float waterFactor = 0;
    public float growth = 0;
    public float height = 0;
    public float time = 0;
    public float dampTime = 0.25f;
    private float growthVelocity = 0;
    private float sunVelocity = 0;
    private float airVelocity = 0;
    private float waterVelocity = 0;
    public float growthDamp = 0;
    public float sunDamp = 0;
    public float airDamp = 0;
    public float waterDamp = 0;
    public Transform growthMeter;
    public Transform heightMeter;
    public TextMesh heightText;
    public Transform clockMeter;
    public TextMesh clockText;
    public Transform sunMeter;
    public Transform airMeter;
    public Transform waterMeter;

    void Update()
    {
        time += Time.deltaTime;
        UpdateClock();
        UpdateInputs();
        UpdateGrowth();
        UpdateHeight();
        DepleteWater();
    }

    public void UpdateClock()
    {
        clockMeter.eulerAngles = new Vector3(clockMeter.eulerAngles.x, clockMeter.eulerAngles.y, -4 * time + 30);
        float timeNormal = (time * (24f / 90f)) + 4;
        int hour = (int)(timeNormal % 12f);
        if (hour == 0)
            hour = 12;
        string amOrPm = (timeNormal % 24f < 12f) ? "am" : "pm";
        clockText.text = hour + "<size=36> " + amOrPm + "  " + "Monday</size>";     
    }

    public void UpdateInputs()
    {
        sun = Mathf.Clamp(sun, 0, 1);
        air = Mathf.Clamp(air, 0, 1);
        water = Mathf.Clamp(water, 0, 1);

        sunDamp = Mathf.SmoothDamp(sunDamp, sun, ref sunVelocity, dampTime);
        sunMeter.eulerAngles = new Vector3(sunMeter.eulerAngles.x, sunMeter.eulerAngles.y, -180 * sunDamp);

        airDamp = Mathf.SmoothDamp(airDamp, air, ref airVelocity, dampTime);
        airMeter.eulerAngles = new Vector3(airMeter.eulerAngles.x, airMeter.eulerAngles.y, -180 * airDamp);

        waterDamp = Mathf.SmoothDamp(waterDamp, water, ref waterVelocity, dampTime);
        waterMeter.eulerAngles = new Vector3(waterMeter.eulerAngles.x, waterMeter.eulerAngles.y, -180 * waterDamp);
    }

    public void UpdateGrowth()
    {
        sunFactor = GV.SunFactor(sun);
        airFactor = GV.AirFactor(air);
        waterFactor = GV.WaterFactor(water);
        growth = sunFactor * airFactor * waterFactor * 10;
        growthDamp = Mathf.SmoothDamp(growthDamp, growth, ref growthVelocity, dampTime);
        float meterAngle = -90 * (growthDamp / 10);
        growthMeter.eulerAngles = new Vector3(growthMeter.eulerAngles.x, growthMeter.eulerAngles.y, meterAngle);
    }

    public void UpdateHeight()
    {
        height += growth * Time.deltaTime / 10;
        heightMeter.eulerAngles = new Vector3(heightMeter.eulerAngles.x, heightMeter.eulerAngles.y, height * -360 - 90);
        int heightInteger = (int)height;
        int heightDecimal = (int)((height - (float)heightInteger) * 10f);
        heightText.text = heightInteger + "<size=36>." + heightDecimal + " cm</size>";
    }

    public void DepleteWater()
    {
        water -= Time.deltaTime * (growth / 10 + sun) / 100;
        if (water < 0)
            water = 0;
        waterFactor = GV.WaterFactor(water);
    }
}
