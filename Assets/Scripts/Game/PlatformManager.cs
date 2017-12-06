using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlatformManager : MonoBehaviour {

    Platform[] platforms;
    bool platformsActive;

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
        platformsActive = transform.gameObject.activeInHierarchy;
    }

    public void Refresh(float dt)
    {
        if(platformsActive)
            foreach (Platform p in platforms)
                p.Refresh(dt);
    }

    public void SetPlatformsActive(bool _setActive)
    {
        transform.gameObject.SetActive(_setActive);
        platformsActive = _setActive;
        if (!_setActive)
            GV.ws.aphidManager.SetAphidsActive(false); //cant have floating aphids now can we
    }

    public List<Platform> GetAllPlatformsWithAphids()
    {
        return platforms.Where(x => x.hasAphid).ToList();
    }

}
