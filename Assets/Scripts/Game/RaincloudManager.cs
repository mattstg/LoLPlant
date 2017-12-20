using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaincloudManager : MonoBehaviour {

    public GameObject raindrop;
    List<RainCloud> rainclouds;
	private float closestPos = 0;
	Vector2 charPos;

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
		charPos = GV.ws.pc.transform.position;
		closestPos = GV.worldWidth;
		foreach (RainCloud rc in rainclouds) {
			rc.Refresh (dt);
			float pos = Mathf.Abs(charPos.x - rc.transform.position.x);
			closestPos = (closestPos > pos) ? pos : closestPos;
		}
		SetSoundVolume (closestPos);
    }

	public void SetSoundVolume(float closestRain){
		float heightModifier = (charPos.y > GV.worldRange[1]) ? 1 : ((charPos.y - GV.worldRange[0]) / (GV.worldRange[1] - GV.worldRange[0]));
        heightModifier = Mathf.Clamp01(heightModifier);
		float toReturn = (closestRain > GV.rainHearingDist) ? 0 : (GV.rainHearingDist - closestRain) / GV.rainHearingDist;
        
		toReturn = (toReturn < 0) ? 0 : toReturn * heightModifier;
		LOLAudio.Instance.SetBGLevel(toReturn);
        Debug.Log("Rain Volume: " + toReturn);
	}
}
