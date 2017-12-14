using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raindrop : MonoBehaviour {

	private Animator anim;

	//start: to delete 
	void Start(){
		Initialize ();
	}

	// Use this for initialization
	public void Initialize () {
		anim = GetComponent<Animator> ();
	}

	public void Destroy(){
		GameObject.Destroy (gameObject);
	}

	public void Pop(){
		anim.SetTrigger ("Pop");
	}

    public void OnTriggerEnter2D(Collider2D coli)
    {
        switch (coli.gameObject.tag)
        {
            case "Platform":
                Pop();
                break;
			case "Aphid":
				Pop();
				break;
            case "Water":
                break;
            case "RainCollector":
                Destroy();
                float waterDelta = GV.waterPerDrop * (1f - (GV.ws.plant.water * 3f / 4f));
                GV.ws.plant.water = Mathf.Clamp01(GV.ws.plant.water + waterDelta);
                break;
        }
    }
}
