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
    public float growthDamp = 0;
    public float height = 0;
    public float time = 0;
    public float dampTime = 0.5f;
    private float velocity = 0;
    public Transform growthMeter;
    public Transform heightMeter;
    public TextMesh heightText;

    void Update()
    {
        time += Time.deltaTime;
        UpdateGrowth();
        height += growth * Time.deltaTime / 10;
        heightMeter.eulerAngles = new Vector3(heightMeter.eulerAngles.x, heightMeter.eulerAngles.y, height * -360 - 90);
        heightText.text = ((float)((int)(height * 10f))/10f).ToString("0.0") + "\n<size=24>cm</size>";
        water -= Time.deltaTime * (growth / 10 + sun) / 100;
        if (water < 0)
            water = 0;
        waterFactor = GV.WaterFactor(water);
    }
    public void UpdateGrowth()
    {
        sunFactor = GV.SunFactor(sun);
        airFactor = GV.AirFactor(air);
        waterFactor = GV.WaterFactor(water);
        growth = sunFactor * airFactor * waterFactor * 10;
        growthDamp = Mathf.SmoothDamp(growthDamp, growth, ref velocity, dampTime);
        float meterAngle = -90 * (growthDamp / 10);
        growthMeter.eulerAngles = new Vector3(growthMeter.eulerAngles.x, growthMeter.eulerAngles.y, meterAngle);

    }
}
