using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    InputManager im;

    public float YClickJumpThreshold = 1.5f; //Clicking this much units higher will cause the jump
	AnimationController anim;
	Rigidbody2D body;
    
    bool isGrounded;
    float moveForce = 10.5f;  //
    float maxSpeed = 6f;
    //All jump related
    float jumpForceInitial = 4f;
    float jumpForcePerSec = 7f;
    float jumpMaxHoldTime = 1.5f;
    float jumpHeldTime = 0; //Counter that increases while held
    bool isJumping = false;
    float mouseJumpTolerance = 1; //higher than that amt to jump
    bool inputActive = true;
    bool inputHardLock = false; //Use if you dont want popupmanager interfering
    float timeSinceInitialJumpBurst = 0;
    readonly float MaxTimeBetweenJumpBurst = 1;

    public void Initialize()
    {
		anim = GetComponentInChildren<AnimationController> ();
		body = GetComponent<Rigidbody2D> ();
        im = gameObject.AddComponent<InputManager>();
        im.Initialize(this);
		anim.Initialize();
        //Create both input managers & link here
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
    }

    public float RaycastToSun() //Efficency of raycast to the sun
    {
        //Need layer mask
        int layerMask = LayerMask.NameToLayer("Platform");
        float d = Vector2.Distance(transform.position, GV.ws.sun.transform.position);
        //need distance
        RaycastHit2D[] rayhits = Physics2D.RaycastAll(transform.position, transform.position - GV.ws.sun.transform.position, d, layerMask);
        if (rayhits == null)
            return 1;
        return 1 - rayhits.Length;
    }

	public void MouseDown(Vector2 mouseWorldPos, float _dt)
    {
        Vector2 relativePress = new Vector2(Mathf.Clamp(mouseWorldPos.x - transform.position.x,-1,1), Mathf.Clamp(mouseWorldPos.y - transform.position.y,-1,1));
        relativePress.y = (Mathf.Abs(relativePress.y) >= mouseJumpTolerance) ?-1:0;            
        KeysPressed(relativePress, _dt);
    }

	public void KeysPressed(Vector2 dir, float _dt)
    {
		if (dir.x != 0)
			Move (dir.x, _dt);
		if (dir.y != 0)
            
                Jump(dir.y, _dt);
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

    public void OnCollisionEnter2D(Collision2D coli)
    {
        switch(coli.gameObject.tag)
        {
            case "Platform":
                Debug.Log("touched platform: " + coli.gameObject.name);
                TouchedGround();
                break;
            case "Water":
                break;
            case "Aphid":
                Debug.Log("aphid touch");
                TouchedGround();
                if (!coli.gameObject.GetComponent<Aphid>().isOutCold)
                    GetHitByAphid(coli.gameObject.transform);
                break;
        }
    }

    private void TouchedGround()
    {
        Debug.Log("Touched ground");
        isGrounded = true;
        isJumping = false;
        jumpHeldTime = 0;
        anim.Grounded(true);
    }

    public void GetHitByAphid(Transform aphidTransform)
    {
        Debug.Log("Get hit by aphid");
    }

    public virtual void OnCollisionExit2D(Collision2D coli)
    {
        Debug.Log("Exit collider: " + coli.gameObject.name);
        switch (coli.gameObject.tag)
        {
		case "Platform":
				isGrounded = false;
				anim.Grounded (false);
                break;
        }
    }


}
