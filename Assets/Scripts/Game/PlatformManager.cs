using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour {

    Platform[] platforms;

	public void Initialize()
    {
        //All children should be platforms
        int numOfChildren = transform.childCount;
        platforms = new Platform[numOfChildren];
        for (int i = 0; i < numOfChildren; i++)
        {
            platforms[i] = transform.GetChild(i).GetComponent<Platform>();
            platforms[i].Initialize();
        }
    }

    public void Refresh(float dt)
    {
        foreach (Platform p in platforms)
            p.Refresh(dt);
    }
}
