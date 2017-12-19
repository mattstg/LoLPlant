using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaincloudManager : MonoBehaviour {

    public GameObject raindrop;
    List<RainCloud> rainclouds;

    public void Initialize(bool singleScreenLock)
    {
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
}
