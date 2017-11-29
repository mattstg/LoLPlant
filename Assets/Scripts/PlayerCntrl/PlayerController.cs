using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    InputManager im;

    public float YClickJumpThreshold = 1.5f; //Clicking this much units higher will cause the jump
	AnimationController anim;
	Rigidbody2D body;
    bool isJumping;
    bool isGrounded;

    public void Initialize()
    {
		anim = GetComponentInChildren<AnimationController> ();
		body = GetComponent<Rigidbody2D> ();
        im = gameObject.AddComponent<InputManager>();
        im.Initialize(this);
		anim.Initialize();
        //Create both input managers & link here
    }

    public virtual void Refresh(float dt)
    {
        im.UpdateInput(dt); //update input
		anim.Refresh(dt);
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
        //This will recieve input of mouse
        Debug.Log("Mouse pressed/held: " + mouseWorldPos);
		Move (mouseWorldPos.x, _dt);
    }

	public void KeysPressed(Vector2 dir, float _dt)
    {
        Debug.Log("Key pressed: " + dir);
    }

	public void Jump(float direction, float _dt){
		body.AddForce (new Vector2(0, direction / Mathf.Abs(direction) * _dt), ForceMode2D.Impulse);
	}

	public void Move(float direction, float _dt){
		body.AddForce (new Vector2(direction / Mathf.Abs(direction) * _dt * 1000, 0), ForceMode2D.Force);
	}

    public void OnCollisionEnter2D(Collision2D coli)
    {
        switch(coli.gameObject.tag)
        {
            case "Platform":
                isGrounded = true;
				anim.Grounded(true);
                break;
            case "Water":
                break;
        }

    }

    public virtual void OnCollisionExit2D(Collision2D coli)
    {
        switch (coli.gameObject.tag)
        {
		case "Platform":
				isGrounded = false;
				anim.Grounded (false);
                break;

        }

    }


}
