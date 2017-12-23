using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InputManager : MonoBehaviour {

    PlayerController pc;

    public void Initialize(PlayerController _pc)
    {
        pc = _pc;
    }

	public void UpdateInput(float dt)
    {
        bool inputProcessed = false;

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            pc.DownFirstPressed();
        }

            //Keyboard input
        Vector2 keysPressed = new Vector2();
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            keysPressed.x -= 1;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            keysPressed.x += 1;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Space))
            keysPressed.y = -1;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            keysPressed.y = 1;
        if (keysPressed.x != 0 || keysPressed.y != 0)
        {
            pc.KeysPressed(keysPressed, dt);
            inputProcessed = true;
        }
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
            GV.ws.popupManager.Next();

        //Mouse/touch click
        if (!inputProcessed)
        {
            if (Input.GetMouseButton(0))  //Continous press
            {
                _MouseClicked(Input.mousePosition, dt,true);
            }
            else if (Input.touchCount > 0 && (Input.GetTouch(0).phase == TouchPhase.Stationary || Input.GetTouch(0).phase == TouchPhase.Moved))
            {
                _MouseClicked(Input.GetTouch(0).position, dt, false);
            }
        }        

        
    }

	public void OnMouseOver()
	{
		MouseOver (Camera.main.ScreenToWorldPoint(Input.mousePosition));
	}

	private void _MouseClicked(Vector2 clickPos,float _dt, bool firstClick)
	{
		Vector2 clickedPos = Camera.main.ScreenToWorldPoint (clickPos);
		MouseDown (clickedPos,_dt, firstClick);
	}

	protected virtual void MouseOver(Vector2 mouseWorldPos)
	{

	}

	protected virtual void MouseDown(Vector2 mouseWorldPos, float _dt, bool firstClick)
	{
		pc.MouseDown(mouseWorldPos,_dt, firstClick);
	}
    
        
}
