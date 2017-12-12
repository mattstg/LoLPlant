using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aphid : MonoBehaviour {

    SpriteRenderer sr;
    Platform parentPlatform;
    float timeToCrossPlatform = 3;  //Time to cross platform regardless of size
    float outCold = 0;
    float outColdMax = 3;
    bool headingRight = true;
    Vector3 offset;
    float progress = 0;
    public bool isOutCold { get { return outCold > 0; } }
    GameObject koAnimation;

    
    public void Initialize(Platform _parentPlatform)
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        parentPlatform = _parentPlatform;
        offset = (headingRight) ? parentPlatform.GetSidePoint(headingRight): parentPlatform.GetSidePoint(!headingRight);
        transform.position = Vector3.Lerp(parentPlatform.GetSidePoint(headingRight), parentPlatform.GetSidePoint(!headingRight), progress);
        koAnimation = transform.GetChild(0).gameObject;
    }

    public void HoppedOn()
    {
        if (!isOutCold)
        {
            Debug.Log("Hopped on Aphid");
            outCold = outColdMax;
            koAnimation.SetActive(true);
        }
    }
  
    public void Refresh(float dt)
    {
        if(outCold > 0)
        {
            outCold -= dt;
            if (outCold <= 0)
            {
                outCold = 0;
                koAnimation.SetActive(false);
            }
        }
        else
        {
            progress += (dt / timeToCrossPlatform);
            if(progress >= 1)
            {
                progress = 0;
                headingRight = !headingRight;
				Flip ();
            }
        }
        transform.position = Vector3.Lerp(parentPlatform.GetSidePoint(headingRight), parentPlatform.GetSidePoint(!headingRight), progress);
    }

	public void Flip(){
		Vector3 newScale = gameObject.transform.localScale;
		gameObject.transform.localScale = new Vector3 (newScale.x * -1, newScale.y, newScale.z);
	}
}
