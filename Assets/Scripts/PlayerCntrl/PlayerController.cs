using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    InputManager im;
    public float YClickJumpThreshold = 1.5f; //Clicking this much units higher will cause the jump

    bool isJumping;
    bool isGrounded;

    public void Initialize()
    {
        im = gameObject.AddComponent<InputManager>();
        im.Initialize(this);
        //Create both input managers & link here
    }

    public void Refresh(float dt)
    {
        im.UpdateInput(); //update input
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
                break;
            case "Water":
                break;
        }

    }

    public void OnCollisionExit2D(Collision2D coli)
    {
        switch (coli.gameObject.tag)
        {
            case "Platform":
                isGrounded = false;
                break;

        }

    }


}
