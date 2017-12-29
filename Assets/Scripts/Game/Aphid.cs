using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aphid : MonoBehaviour {

    SpriteRenderer sr;
    Platform parentPlatform;
    float timeToCrossPlatform = 3;  //Time to cross platform regardless of size
    float outCold = 0;
    float outColdMax = 3;
	bool collidersOn = true;
    bool headingRight = true;
    float progress = 0;
    public bool isOutCold { get { return outCold > 0; } }
	public Animator anim;
	Vector2 verticalOffset;
	bool hasRecover = true;

    
    public void Initialize(Platform _parentPlatform)
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        parentPlatform = _parentPlatform;
		headingRight = Random.Range (0, 2) == 0;
		sr.flipX = !headingRight;
        //verticalOffset = new Vector2(0,transform.localScale.y / 2 + _parentPlatform.gameObject.transform.localScale.y / 2) * 0.4f;
        verticalOffset = new Vector2(0f, 0.25f);
		transform.position = Vector3.Lerp(parentPlatform.GetSidePoint(headingRight) + verticalOffset, parentPlatform.GetSidePoint(!headingRight) + verticalOffset,progress);
    }

	public void EndRecoveryAnimation(){
		//called from within animator to indicate end of recovery animation
		flopColliders ();
		hasRecover = true;
	}

    public void HoppedOn()
    {
        if (!isOutCold)
        {
            outCold = outColdMax;
			anim.SetTrigger ("Die");
			flopColliders ();
			hasRecover = false;
        }
    }
  
    public void Refresh(float dt)
    {
		if (outCold > 0) {
			outCold -= dt;
			if (outCold <= 0) {
				outCold = 0;
				anim.SetTrigger ("Recover");
			}
		}
		else if (hasRecover) 
        {
            progress += (dt / timeToCrossPlatform);
            if(progress >= 1)
            {
                progress = 0;
                headingRight = !headingRight;
				sr.flipX = !headingRight;
            }
        }
		transform.position = Vector3.Lerp(parentPlatform.GetSidePoint(headingRight) + verticalOffset, parentPlatform.GetSidePoint(!headingRight) + verticalOffset,progress);
    }
		
	public void OnTriggerEnter2D(Collider2D other){
		if(other.gameObject.GetComponent<PlayerController> () != null)
			other.gameObject.GetComponent<PlayerController> ().GetHitByAphid(transform);
	}

	public void flopColliders(){
		collidersOn = !collidersOn;
		foreach (BoxCollider2D b in GetComponents<BoxCollider2D>())
			b.enabled = collidersOn;
	}

}
