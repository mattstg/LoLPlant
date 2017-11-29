using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour {
	//variables
	private static float _airTimeUntilLand = 0.75f;
	private static float _airTimeUntilFall = 0.75f;
	private static float _timeBetweenJumpAnims = 1.5f;
	private static float _moveAnimSpeedMin = 0.75f;
	private static float _moveAnimSpeedModifier = 4;
	private static float _moveAnimSpeedDampener = 2;
	private static float _moveDampener = 5;


	private Animator anim;
	private Rigidbody2D player;


	private float jumpTimer = 0;
	private float fallTimer = 0;
	private bool isGrounded = false;
	private bool canFall = true;

	// Use this for initialization
	void Start () {
		anim = GetComponentInParent<Animator> ();
		player = GetComponentInParent<Rigidbody2D> ();
		jumpTimer = _timeBetweenJumpAnims;
	}
	
	// Update is called once per frame
	void Update () {
		float dTime = Time.deltaTime;
		if (!isGrounded) {
			fallTimer += dTime;
			if (fallTimer > _airTimeUntilFall)
				Fall ();
		}
		else
			fallTimer = 0;
		jumpTimer += dTime;
		Velo (player.velocity.x, player.velocity.y);
		MoveSpeed (Mathf.Clamp(player.velocity.x / _moveAnimSpeedDampener, _moveAnimSpeedMin, _moveAnimSpeedModifier));
		Move (player.velocity.x / _moveDampener);
	}
		
	public void Grounded(bool input){
		anim.SetBool ("isGrounded", input);
		if (input) {
			canFall = true;
			if (fallTimer > _airTimeUntilLand) {
				Land ();
				canFall = false;
			}
			isGrounded = true;
			fallTimer = 0;
		} else {
			isGrounded = false;
			canFall = true;
		}
	}

	public void Fall(){
		if (canFall) {
			anim.SetTrigger ("fall");
			canFall = false;
		}
	}

	public void Land(){
		anim.SetTrigger ("land");
	}

	//call from player
	public void Jump(){
		if (jumpTimer > _timeBetweenJumpAnims) {
			anim.SetTrigger ("jump");
		}
	}

	public void Velo(float input_x, float input_y){
		anim.SetFloat ("fall_x", input_x);
		anim.SetFloat ("fall_y", input_y);
	}

	public void Move(float input){
		anim.SetFloat ("move", input);
	}

	public void MoveSpeed(float input){
		anim.SetFloat ("movSpeed", input);
	}
}
