using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour {

    public float air;
    public float water;
    public float sun;
    public float drainRate;
    public float growthRate;

    public void UpdatePlant(float dt)
    {
        growthRate = Mathf.Min(air, water, sun); //Calculate growth rate
        
        //Cause the plant to grow
        
        //Deduct Air, Water & Sun
        air   = Mathf.Clamp01(air   - drainRate);
        water = Mathf.Clamp01(water - drainRate);
        sun   = Mathf.Clamp01(sun   - drainRate);
    }

    
}
