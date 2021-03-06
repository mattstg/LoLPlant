﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    InputManager im;
	public SpriteRenderer plantSprite;
    
    public float YClickJumpThreshold = 1.5f; //Clicking this much units higher will cause the jump
	protected AnimationController anim;
	protected Rigidbody2D body;
    
	protected bool isGrounded;
    float moveForce = 6.7f;  //
    float maxSpeed = 2.25f;
    //All jump related
    float jumpForceInitial = 4f;
    float jumpForcePerSec = 7f;
    float jumpMaxHoldTime = 1.5f;
    float jumpHeldTime = 0; //Counter that increases while held
    bool isJumping = false;
    float mouseJumpTolerance = .4f; //higher than that amt to jump
    bool inputActive = true;
    bool inputHardLock = false; //Use if you dont want popupmanager interfering
	protected float timeSinceInitialJumpBurst = 0;
    readonly float MaxTimeBetweenJumpBurst = 1;
    float dropForcePerSec = 9; //pressing down on purpose to drop
    Vector2 playerDrag = GV.playerDrag;
	float getHitByAphidTime = 0f;

    //Dropping through platforms
    List<Platform> platformsTouching = new List<Platform>();
    public bool playerCanDropThroughPlatforms = true;
    bool droppingThroughPlatform;
    float timeDropping;
    float maxTimeDrop = 1; //time where colliders inactive
    Transform bottomOfFeet;

    public void Initialize()
    {
        string n = transform.name;
        gameObject.SetActive(true);
        plantSprite = transform.GetComponentInChildren<PlantSpriteTag>().GetComponent<SpriteRenderer>();
        anim = GetComponentInChildren<AnimationController> ();
		body = GetComponent<Rigidbody2D> ();
        im = gameObject.AddComponent<InputManager>();
        im.Initialize(this);
		anim.Initialize();
        bottomOfFeet = transform.Find("BottomFoot");
    }

    public void SetInputActive(bool _setActive, bool hardLock = false)
    {
        if (hardLock)
        {
            inputHardLock = hardLock;
            inputActive = _setActive;
        }
        else if(!inputHardLock) //&& !hardlock
            inputActive = _setActive;
        
    }

    public virtual void Refresh(float dt)
    {
        if(inputActive)
            im.UpdateInput(dt); //update input
		anim.Refresh(dt);
        timeSinceInitialJumpBurst += dt;
		if (isGrounded)
			body.drag = playerDrag.y;
		else
			body.drag = playerDrag.x;
        RaycastToSun();
    }

    public virtual void RaycastToSun() //Efficency of raycast to the sun
    {
        //Need layer mask
        var layerMask = (1 << LayerMask.NameToLayer("Platform")) | (1 << LayerMask.NameToLayer("Cloud")) ;
        //need distance
        Vector2 shadowAngle = GV.DegreeToVector2(GV.ws.dnc.groundToSunAngle);
        RaycastHit2D[] rayhits = Physics2D.RaycastAll(transform.position, shadowAngle, 30, layerMask);
        if (rayhits == null || rayhits.Length <= 0)
            GV.ws.plant.shadowCount = 0;
        else
            GV.ws.plant.shadowCount = rayhits.Length;
    }

	public void MouseDown(Vector2 mouseWorldPos, float _dt, bool firstPress)
    {
        if (!GV.ws.es.IsPointerOverGameObject())
        {
            Vector2 relativePress = new Vector2(Mathf.Clamp(mouseWorldPos.x - transform.position.x, -1, 1), Mathf.Clamp(mouseWorldPos.y - transform.position.y, -1, 1));
            relativePress.y *= -1; //inverted cntrls?
            if (Mathf.Abs(relativePress.y) < GV.YDiffToActivate)
                relativePress.y = 0;

            //relativePress.y = (Mathf.Abs(relativePress.y) >= mouseJumpTolerance) ? -1 : 0;
            KeysPressed(relativePress, _dt);
            if (firstPress && relativePress.y != 0 && Mathf.Sign(relativePress.y) >= 0)
                DownFirstPressed();
        }
    }

    public void DownFirstPressed()
    {
        if(isGrounded && platformsTouching.Count > 0)
        {
            foreach (Platform p in platformsTouching)
                p.SetPlatformTraversable();
        }
    }

	public void KeysPressed(Vector2 dir, float _dt)
    {
			if (dir.x != 0)
				Move (dir.x, _dt);

			if (dir.y < 0)
				Jump (dir.y, _dt);
			else if (dir.y > 0)
				DropDown (_dt);
    }

    private void DropDown(float dt)
    {
        body.AddForce(new Vector2(0, -1*dropForcePerSec * dt), ForceMode2D.Impulse);
    }

	public void Jump(float direction, float _dt){
        
        if (jumpHeldTime < jumpMaxHoldTime)
        {
            float jumpForce = 0;
            if (isGrounded && !isJumping && timeSinceInitialJumpBurst >= MaxTimeBetweenJumpBurst)
            {
				anim.Jump ();
                timeSinceInitialJumpBurst = 0;
                jumpForce = jumpForceInitial;
                isJumping = true;
            }
            else
            {
                jumpForce = jumpForcePerSec * _dt;
                jumpHeldTime += _dt;
            }
            body.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }
        
    }

	public void Move(float direction, float _dt)
    {
        float moveForce = _dt * this.moveForce * direction;
        body.AddForce (new Vector2(moveForce, 0), ForceMode2D.Impulse);
        if (Mathf.Abs(body.velocity.x) > maxSpeed)
            body.velocity = new Vector2(maxSpeed * Mathf.Sign(body.velocity.x),body.velocity.y);
    }

    private void TouchedGround()
    {
        string n = transform.name;
        if (body.velocity.y <= 0)
            LOLAudio.Instance.PlayAudio(LOLAudio.land);
        isGrounded = true;
        isJumping = false;
        jumpHeldTime = 0;
        anim.Grounded(true);

    }

    public void GetHitByAphid(Transform aphidTransform)
    {
        //body.velocity.Set (0, 0);
		if(Time.time - getHitByAphidTime > GV.TimeBetweenAphidBites){
			getHitByAphidTime = Time.time;
	        float knockbackVelo = (transform.position.y < aphidTransform.position.y)? GV.AphidKnockbackVeloReduced : GV.AphidKnockbackVelo;

	        body.velocity = ((transform.position - aphidTransform.position).normalized * knockbackVelo);
	        if (GV.ws.plant != null) //quick dirty check for if it's main menu scene
	        {
	            GV.ws.plant.LoseFood(GV.FoodLostPerAphid);
	            LOLAudio.Instance.PlayAudio(LOLAudio.aphidHit);
	        }
		}

    }

	private GameObject[] colies = new GameObject[3];
	private int coliCounter = 0;

	private void AddColi(GameObject _coli){
		if (coliCounter < 3 && coliCounter > -1) {
			colies [coliCounter] = _coli;
			coliCounter++;
		} else {
			Debug.Log ("WHAT?! YOU ARE STANDING ON MORE THAN 3 PLATFORMS?!?!");
		}
	}

	private bool RemoveColi(GameObject _coli){
		int c = 0;
		while (c < 3) {
			if (colies [c] == _coli) {
				colies [c] = null;
				if (c != coliCounter - 1) {
					while (c < 2) {
						colies [c] = colies [c + 1];
						c++;
					}
				} 
				coliCounter--;
				c = 3;
				return true;
			}
			c++;
		}
		return false;
	}

    public virtual void OnCollisionEnter2D(Collision2D coli)
    {
        switch(coli.gameObject.tag)
        {
		case "Platform":
			TouchedGround ();
			Platform plat = coli.gameObject.GetComponent<Platform> ();
			if (plat != null)
			plat.AddPlayer (this.gameObject);
			AddColi (coli.gameObject);
            if(coli.gameObject.GetComponent<Platform>())
                platformsTouching.Add(coli.gameObject.GetComponent<Platform>());
            break;
        case "Water":
            break;
		case "Aphid":
                if (bottomOfFeet.position.y <= coli.transform.position.y)
                {
                    GetHitByAphid(coli.transform);
                }
                else
                {
                    TouchedGround();
                    if (!coli.gameObject.GetComponent<Aphid>().isOutCold)
                        coli.gameObject.GetComponent<Aphid>().HoppedOn();
                }
                
            break;
        }
    }

    public virtual void OnCollisionExit2D(Collision2D coli)
    {
        switch (coli.gameObject.tag)
        {
		case "Platform":
			RemoveColi (coli.gameObject);
			if (coliCounter == 0) {
				isGrounded = false;
				anim.Grounded (false);
                if (coli.gameObject.GetComponent<Platform>())
                    platformsTouching.Remove(coli.gameObject.GetComponent<Platform>());
			}
			Platform plat = coli.gameObject.GetComponent<Platform> ();
			if (plat != null)
				plat.RemovePlayer ();
			break;
        }
    }

	public void UpdateHeight(float height)
	{
        float heightScale = -0.0070f;
        float maxHeight = -4.53f;     //=1000mm
        float minHeight = -1.097f;    //=50mm
        float zeroOffset = -0.87f;
        if (plantSprite != null)
            plantSprite.transform.localPosition = new Vector2(0, Mathf.Clamp(height * heightScale + zeroOffset, maxHeight + zeroOffset, minHeight));
	}

    public void SetPlantSpriteFlip(bool flipped)
    {
        plantSprite.flipX = flipped;
    }
}
