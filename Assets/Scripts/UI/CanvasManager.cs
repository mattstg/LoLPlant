using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour {

    public float sun = 0;
    public float air = 0;
    public float water = 0;
    public float sunFactor = 0;
    public float airFactor = 0;
    public float waterFactor = 0;
    public float growth = 0;
    public float height = 0;
    public float time = 0;
    public Transform growthMeter;

    void Update()
    {
        time += Time.deltaTime;
        UpdateGrowth();
        height += growth * Time.deltaTime / 10;
        water -= Time.deltaTime * (growth / 10 + sun) / 50;
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
        growthMeter.position = new Vector3(growthMeter.position.x, growth/2f-4f, growthMeter.position.z);

    }
}
