using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    InputManager im;
	AnimationController anim;
	Rigidbody2D body;
    public float YClickJumpThreshold = 1.5f; //Clicking this much units higher will cause the jump

    bool isJumping;
    bool isGrounded;

	public void Start(){
		//temp just so i can initialize in scene
		Initialize();
	}

    public void Initialize()
    {
        im = gameObject.AddComponent<InputManager>();
        im.Initialize(this);
		anim = GetComponent<AnimationController> ();
		body = GetComponent<Rigidbody2D> ();
        //Create both input managers & link here
    }

    public virtual void Refresh(float dt)
    {
        im.UpdateInput(); //update input
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

    public void MouseDown(Vector2 mouseWorldPos)
    {
        //This will recieve input of mouse
        Debug.Log("Mouse pressed/held: " + mouseWorldPos);

    }

    public void KeysPressed(Vector2 dir)
    {
        Debug.Log("Key pressed: " + dir);
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
