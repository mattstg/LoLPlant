using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour {
	//variables
	private static float _airTimeUntilLand = 0.75f;
	private static float _airTimeUntilFall = 0.75f;
	private static float _timeBetweenJumpAnims = 1.5f;
	private static float _moveAnimSpeedMin = 0.75f;
	private static float _moveAnimSpeedModifier = 2;
	private static float _moveAnimSpeedDampener = 2;
	private static float _moveDampener = 5;
	private bool facingRight = true;


	private Animator anim;
	private Rigidbody2D player;
	public SpriteRenderer bodySprite;



	private float jumpTimer = 0;
	private float fallTimer = 0;
	private bool isGrounded = false;
	private bool canFall = true;

	// Use this for initialization

	public void Initialize()
	{
		anim = GetComponent<Animator> ();
		player = GetComponentInParent<Rigidbody2D> ();
		jumpTimer = _timeBetweenJumpAnims;
	}

	public virtual void Refresh(float dt){
	// Update is called once per frame
		if (!isGrounded) {
			fallTimer += dt;
			if (fallTimer > _airTimeUntilFall)
				Fall ();
		}
		else
			fallTimer = 0;
		jumpTimer += dt;
		Velo (Mathf.Abs(player.velocity.x), player.velocity.y);
		MoveSpeed (Mathf.Clamp(Mathf.Abs(player.velocity.x) / _moveAnimSpeedDampener, _moveAnimSpeedMin, _moveAnimSpeedModifier));
		Move (Mathf.Abs(player.velocity.x) / _moveDampener);
		if ((facingRight && player.velocity.x < -0.05f) || (!facingRight && player.velocity.x > 0.05f))
			Flip ();
	}
		
	public void Flip(){
		Vector3 newScale = gameObject.transform.localScale;
		gameObject.transform.localScale = new Vector3 (newScale.x * -1, newScale.y, newScale.z);
        facingRight = !facingRight;
        bodySprite.flipX = (GV.ws.dnc.isMorning) ? facingRight : !facingRight;
        GV.ws.pc.SetPlantSpriteFlip(facingRight);
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
