using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raindrop : MonoBehaviour {

	private Animation anim;
	private float timeToDestroy = 0;
	private float destroyTimer = 0;
	private bool markForDestroy = false;


	//start: to delete 
	void Start(){
		Initialize ();
	}
	void Update(){
		Refresh (Time.deltaTime);
	}
	//end: delete


	// Use this for initialization
	public void Initialize () {
		anim = GetComponent<Animation> ();
		timeToDestroy = anim.GetClip ("Raindrop_Pop").length;
	}



	public void Refresh(float dt){
		if (markForDestroy)
			destroyTimer += dt;
		if (destroyTimer > timeToDestroy) {
			GameObject.Destroy (gameObject);
		}
	}

	public void Pop(){
		anim.Play ();
		markForDestroy = true;
	}

	public void OnCollisionEnter2D(Collision2D coli)
	{
		switch(coli.gameObject.tag)
		{
		case "Player":
			break;
		case "Platform":
			Pop ();
			break;
		case "Water":
			break;
		}

	}
}
