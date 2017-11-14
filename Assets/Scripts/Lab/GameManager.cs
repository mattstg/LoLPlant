using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager {

    #region Singleton
    private static GameManager instance;

    private GameManager() { }

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameManager();
            }
            return instance;
        }
    }
    #endregion

    private float sun = 0;
    private float air = 0;
    private float water = 0;
    private float growth = 0;
    private float height = 0;
    private float time = 0;

    public float UpdateGrowth()
    {
        float sunFactor = GV.SunFactor(sun);
        float airFactor = GV.AirFactor(air);
        float waterFactor = GV.WaterFactor(water);
        growth = sunFactor * airFactor * waterFactor * 10;
        return growth;
    }
}
