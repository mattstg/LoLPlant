using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaincloudManager : MonoBehaviour {

    public GameObject raindrop;
    List<RainCloud> rainclouds;

    public void Initialize(bool singleScreenLock)
    {
		GV.ws.raindrops = new GameObject ();
		GV.ws.raindrops.name = "Raindrops";
        rainclouds = new List<RainCloud>();
        foreach (Transform child in transform)
        {
            RainCloud rc = child.GetComponent<RainCloud>();
            rainclouds.Add(rc);
            rc.Initialize(singleScreenLock);
        }
    }

    public void Refresh(float dt)
    {
        foreach (RainCloud rc in rainclouds)
            rc.Refresh(dt);
    }

    public void SetRaining(bool raining)
    {
        foreach (RainCloud rc in rainclouds)
            rc.SetRaining(raining);
    }
}
